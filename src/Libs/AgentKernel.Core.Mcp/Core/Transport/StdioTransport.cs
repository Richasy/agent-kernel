// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Richasy.AgentKernel.Core.Mcp.Internal;
using Richasy.AgentKernel.Core.Mcp.Models;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using System.Threading;
using System.Threading.Channels;

namespace Richasy.AgentKernel.Core.Mcp.Core;

internal sealed class StdioTransport : IMcpTransport
{
    private readonly McpServerConfig _config;
    private readonly ILogger _logger;
    private readonly string _loggerName;

    private Process? _process;
    private Task? _readTask;
    private CancellationTokenSource? _shutdownCts;
    private bool _processStarted;
    private Func<string, object?> _currentParser;
    private TaskCompletionSource<object?>? _currentCompletionSource;

    public StdioTransport(string id, McpServerConfig config, ILogger? logger = null)
    {
        _config = config;
        _logger = logger ?? NullLogger.Instance;
        _loggerName = $"MCP-STDIO-{id}";
    }

    public bool Running { get; private set; }

    public async ValueTask DisposeAsync()
    {
        await CleanupAsync(CancellationToken.None).ConfigureAwait(false);
        GC.SuppressFinalize(this);
    }

    public async Task ConnectAsync(CancellationToken token = default)
    {
        if (Running)
        {
            _logger.LogWarning($"Transport {_loggerName} is already running.");
            throw new InvalidOperationException("Transport is already running.");
        }

        try
        {
            _logger.LogInformation($"{_loggerName} connecting...");
            _shutdownCts = new CancellationTokenSource();

            var command = McpGlobalConfig.UseCmdAsDefaultCommand ? "cmd" : _config.Command;
            _config.Arguments ??= Array.Empty<string>();
            var arguments = McpGlobalConfig.UseCmdAsDefaultCommand
                ? _config.Arguments?.FirstOrDefault() == "/c"
                    ? _config.Arguments
                    : new[] { "/c", _config.Command }.Concat(_config.Arguments).ToArray()
                : _config.Arguments;

            var startInfo = new ProcessStartInfo
            {
                FileName = command,
                Arguments = string.Join(' ', arguments ?? []),
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = _config.WorkingDirectory ?? Environment.CurrentDirectory,
            };

            if (_config.Environments != null)
            {
                foreach (var (key, value) in _config.Environments)
                {
                    startInfo.Environment[key] = value;
                }
            }

            _logger.LogInformation($"{_loggerName} starting process: {startInfo.FileName} {startInfo.Arguments}");
            _process = new Process
            {
                StartInfo = startInfo,
                EnableRaisingEvents = true,
            };

            _process.ErrorDataReceived += (sender, e) => _logger.LogError($"{_loggerName} error: {e.Data ?? "No error"}");

            if (!_process.Start())
            {
                _logger.LogError($"{_loggerName} failed to start process.");
                throw new InvalidOperationException("Failed to start process.");
            }

            _logger.LogInformation($"{_loggerName} process started.");
            _processStarted = true;
            _process.BeginErrorReadLine();

            _readTask = Task.Run(() => ReadMessageAsync(_shutdownCts.Token), CancellationToken.None);
            Running = true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"{_loggerName} failed to connect.");
            await CleanupAsync(token).ConfigureAwait(false);
            throw;
        }
    }

    public async Task<T?> SendRequestAsync<T>(RpcRequest request, JsonTypeInfo<T> typeInfo, CancellationToken token = default)
    {
        if (!Running || _process?.HasExited == true)
        {
            _logger.LogWarning($"{_loggerName} is not running.");
            throw new InvalidOperationException("Transport is not running.");
        }

        if (_currentCompletionSource != null && _currentCompletionSource.Task?.IsCompletedSuccessfully != true)
        {
            _logger.LogWarning($"{_loggerName} previous request is not completed.");
            _currentCompletionSource?.TrySetCanceled();
            _currentCompletionSource = null;
        }

        var requestId = request.Id?.ToString() ?? "(no id)";
        try
        {
            var json = JsonSerializer.Serialize(request, JsonGenContext.Default.RpcRequest);
            _logger.LogInformation($"{_loggerName} sending request: {json}");
            _currentCompletionSource = new TaskCompletionSource<object?>();
            _currentParser = (json) => JsonSerializer.Deserialize(json, typeInfo);
            await _process!.StandardInput.WriteLineAsync(json.AsMemory(), token).ConfigureAwait(false);
            await _process.StandardInput.FlushAsync(token).ConfigureAwait(false);
            _logger.LogInformation($"{_loggerName} request sent: {requestId}");

            return (T?)await _currentCompletionSource.Task.ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"{_loggerName} failed to send request: {requestId}");
            throw;
        }
    }

    private async Task ReadMessageAsync(CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation($"{_loggerName} reading messages...");
            using var reader = _process!.StandardOutput;
            while (!cancellationToken.IsCancellationRequested && !_process.HasExited)
            {
                var line = await reader.ReadLineAsync(cancellationToken).ConfigureAwait(false);
                if (line == null)
                {
                    break;
                }

                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                _logger.LogInformation($"{_loggerName} received: {line}");
                await ProcessMessageAsync(line, cancellationToken).ConfigureAwait(false);
            }

            _logger.LogInformation($"{_loggerName} read task completed.");
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation($"{_loggerName} read task cancelled.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"{_loggerName} read task failed.");
        }
        finally
        {
            await CleanupAsync(cancellationToken).ConfigureAwait(false);
        }
    }

    private async Task ProcessMessageAsync(string line, CancellationToken cancellationToken)
    {
        line = line.Trim();
        if (string.IsNullOrWhiteSpace(line) || (!line.StartsWith('[') && !line.StartsWith('{')))
        {
            _logger.LogDebug($"{_loggerName} received invalid message: {line}");
            return;
        }

        if (_currentParser != null)
        {
            var obj = _currentParser(line);
            if(obj != null)
            {
                _currentCompletionSource?.TrySetResult(obj);
            }
            else
            {
                _logger.LogWarning($"{_loggerName} failed to parse message: {line}");
                _currentCompletionSource?.TrySetException(new InvalidOperationException("Failed to parse message."));
            }
        }

        await Task.CompletedTask;
    }

    private async Task CleanupAsync(CancellationToken cancellationToken)
    {
        if (_process != null && _processStarted && !_process.HasExited)
        {
            try
            {
                _logger.LogInformation($"{_loggerName} cleaning up...");
                _process.StandardInput.Close();

                _logger.LogInformation($"{_loggerName} waiting for process to exit...");
                var pid = _process.Id;
                RunProcessAndWaitForExit(
                    "taskkill",
                    $"/T /F /PID {pid}",
                    TimeSpan.FromSeconds(5),
                    out var _);

                _process.WaitForExit(TimeSpan.FromSeconds(5));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, $"{_loggerName} failed to clean up.");
            }

            _process.Dispose();
            _process = null;
        }

        if (_shutdownCts is { } shutdownCts)
        {
            await shutdownCts.CancelAsync().ConfigureAwait(false);
            shutdownCts.Dispose();
            _shutdownCts = null;
        }

        if (_readTask is { } readTask)
        {
            try
            {
                _logger.LogInformation($"{_loggerName} read task cleaning up...");
                await readTask.WaitAsync(TimeSpan.FromSeconds(5), cancellationToken).ConfigureAwait(false);
            }
            catch (TimeoutException)
            {
                _logger.LogError($"{_loggerName} read task is timed out.");
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation($"{_loggerName} read task is cancelled.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{_loggerName} read task failed to clean up.");
            }

            _readTask = null;
            _logger.LogInformation($"{_loggerName} read task cleaned up.");
        }

        Running = false;
        _logger.LogInformation($"{_loggerName} cleaned up.");
    }

    private static int RunProcessAndWaitForExit(string fileName, string arguments, TimeSpan timeout, out string? stdout)
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = fileName,
            Arguments = arguments,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
        };

        stdout = null;

        var process = Process.Start(startInfo);
        if (process == null)
            return -1;

        if (process.WaitForExit((int)timeout.TotalMilliseconds))
        {
            stdout = process.StandardOutput.ReadToEnd();
        }
        else
        {
            process.Kill();
        }

        return process.ExitCode;
    }
}
