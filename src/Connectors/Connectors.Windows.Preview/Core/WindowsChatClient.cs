// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.AI;
using Microsoft.Windows.AI;
using Microsoft.Windows.AI.ContentSafety;
using Microsoft.Windows.AI.Text;
using Microsoft.Windows.Management.Deployment;
using System.Runtime.CompilerServices;
using Windows.Foundation;

namespace Richasy.AgentKernel.Connectors.Windows.Preview.Core;

/// <summary>
/// Windows Chat Client.
/// </summary>
public sealed class WindowsChatClient : IChatClient
{
    // Search Options
    private const int DefaultTopK = 50;
    private const float DefaultTopP = 0.9f;
    private const float DefaultTemperature = 1;

    private LanguageModel? _model;
    private LanguageModelContext? _modelContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="WindowsChatClient"/> class.
    /// </summary>
    public WindowsChatClient()
        => Metadata = new("windows", default, "PhiSilica");

    /// <inheritdoc/>
    public ChatClientMetadata Metadata { get; }

    /// <summary>
    /// 开始下载模型.
    /// </summary>
    public event EventHandler? StartDownloadingModel;

    /// <summary>
    /// 正在下载模型.
    /// </summary>
#pragma warning disable CA1003 // 使用泛型事件处理程序实例
    public event EventHandler<PackageDeploymentProgress>? DownloadingModel;

    /// <summary>
    /// 下载模型完成.
    /// </summary>
    public event EventHandler<AIFeatureReadyResult>? ModelDownloaded;
#pragma warning restore CA1003 // 使用泛型事件处理程序实例

    /// <inheritdoc/>
    public void Dispose()
    {
        _model?.Dispose();
        _model = null;
    }

    /// <inheritdoc/>
    public Task<ChatResponse> GetResponseAsync(IEnumerable<ChatMessage> messages, ChatOptions? options = null, CancellationToken cancellationToken = default)
        => GetStreamingResponseAsync(messages, options, cancellationToken).ToChatResponseAsync(cancellationToken: cancellationToken);

    /// <inheritdoc/>
    public async IAsyncEnumerable<ChatResponseUpdate> GetStreamingResponseAsync(IEnumerable<ChatMessage> messages, ChatOptions? options = null, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await InitializeAsync(cancellationToken).ConfigureAwait(false);
        if (_model == null)
        {
            throw new InvalidOperationException("Language model is not loaded.");
        }

        var prompt = GetPrompt(messages);

        await foreach (var part in GenerateStreamResponseAsync(prompt, options, cancellationToken).ConfigureAwait(false))
        {
            yield return new ChatResponseUpdate
            {
                Role = ChatRole.Assistant,
                Contents = [new TextContent(part)]
            };
        }
    }

    /// <inheritdoc/>
    public object? GetService(Type serviceType, object? serviceKey = null)
    {
        ArgumentNullException.ThrowIfNull(serviceType);
        return serviceKey is null && serviceType.IsInstanceOfType(this) ? this : null;
    }

    private static LanguageModelOptions? GetModelOptions(ChatOptions? options)
    {
        if (options == null)
        {
            return default;
        }

        var languageModelOptions = new LanguageModelOptions
        {
            Temperature = options.Temperature ?? DefaultTemperature,
            TopK = (uint)(options.TopK ?? DefaultTopK),
            TopP = (uint)(options.TopP ?? DefaultTopP),
        };

        var contentFilterOptions = new ContentFilterOptions();

        if (options.AdditionalProperties?.TryGetValue("input_moderation", out int inputModeration) == true)
        {
            contentFilterOptions.PromptMaxAllowedSeverityLevel = new TextContentFilterSeverity
            {
                Hate = (SeverityLevel)inputModeration,
                Sexual = (SeverityLevel)inputModeration,
                Violent = (SeverityLevel)inputModeration,
                SelfHarm = (SeverityLevel)inputModeration
            };
        }

        if (options.AdditionalProperties?.TryGetValue("output_moderation", out int outputModeration) == true)
        {
            contentFilterOptions.ResponseMaxAllowedSeverityLevel = new TextContentFilterSeverity
            {
                Hate = (SeverityLevel)outputModeration,
                Sexual = (SeverityLevel)outputModeration,
                Violent = (SeverityLevel)outputModeration,
                SelfHarm = (SeverityLevel)outputModeration
            };
        }

        languageModelOptions.ContentFilterOptions = contentFilterOptions;
        return languageModelOptions;
    }

    private string GetPrompt(IEnumerable<ChatMessage> history)
    {
        if (!history.Any())
        {
            return string.Empty;
        }

        var prompt = string.Empty;
        var firstMessage = history.FirstOrDefault();

        _modelContext = firstMessage?.Role == ChatRole.System ?
            _model?.CreateContext(firstMessage.Text, new ContentFilterOptions()) :
            _model?.CreateContext();

        for (var i = 0; i < history.Count(); i++)
        {
            var message = history.ElementAt(i);
            var msgText = message.Text ?? string.Empty;
            if (message.Role == ChatRole.System && i != 0)
            {
                prompt += $"<|system|>\n{msgText}\n";
            }
            else if (message.Role == ChatRole.User)
            {
                prompt += $"<|user|>\n{msgText}\n";
            }
            else if (message.Role == ChatRole.Assistant)
            {
                prompt += $"<|assistant|>\n{msgText}\n";
            }
        }

        return prompt;
    }

    /// <summary>
    /// 大模型是否可用.
    /// </summary>
    /// <returns></returns>
    public static bool IsAvailable()
    {
#pragma warning disable CA1031 // 不捕获常规异常类型
        try
        {
            return LanguageModel.GetReadyState() == Microsoft.Windows.AI.AIFeatureReadyState.Ready;
        }
        catch (Exception)
        {
            return false;
        }
#pragma warning restore CA1031 // 不捕获常规异常类型
    }

    /// <summary>
    /// 初始化.
    /// </summary>
    private async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (!IsAvailable())
        {
            StartDownloadingModel?.Invoke(this, EventArgs.Empty);
            var operation = LanguageModel.EnsureReadyAsync();
            operation.Progress += HandleDeploymentProgress;
        }

        cancellationToken.ThrowIfCancellationRequested();

        _model = await LanguageModel.CreateAsync();
    }

    private void HandleDeploymentProgress(IAsyncOperationWithProgress<AIFeatureReadyResult, double> asyncInfo, double progressInfo)
    {
        if (progressInfo >= 0 && progressInfo < 100)
        {
            DownloadingModel?.Invoke(this, new PackageDeploymentProgress
            {
                Status = PackageDeploymentProgressStatus.InProgress,
                Progress = progressInfo,
            });
        }
        else if (progressInfo == 100)
        {
            ModelDownloaded?.Invoke(this, asyncInfo.GetResults());
        }
    }

    private async IAsyncEnumerable<string> GenerateStreamResponseAsync(string prompt, ChatOptions? options = null, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        if (_model == null)
        {
            throw new InvalidOperationException("Language model is not loaded.");
        }

        var currentResponse = string.Empty;
        using var newPartEvent = new ManualResetEventSlim(false);

        IAsyncOperationWithProgress<LanguageModelResponseResult, string>? progress;

        var modelOptions = GetModelOptions(options);
        if ((ulong)prompt.Length > _model.GetUsablePromptLength(_modelContext, prompt))
        {
            yield return "\nPrompt larger than context";
            yield break;
        }

        progress = _model.GenerateResponseAsync(_modelContext, prompt, modelOptions);

        progress.Progress = (_, value) =>
        {
            currentResponse = value;
            newPartEvent.Set();
            if (cancellationToken.IsCancellationRequested)
            {
                progress.Cancel();
            }
        };

        while (progress.Status != AsyncStatus.Completed)
        {
            await Task.CompletedTask.ConfigureAwait(ConfigureAwaitOptions.ForceYielding);

            if (newPartEvent.Wait(10, cancellationToken))
            {
                yield return currentResponse;
                newPartEvent.Reset();
            }
        }

        var response = await progress;

        yield return response?.Status switch
        {
            LanguageModelResponseStatus.BlockedByPolicy => "\nBlocked by policy",
            LanguageModelResponseStatus.PromptBlockedByContentModeration => "\nPrompt blocked by content moderation",
            LanguageModelResponseStatus.ResponseBlockedByContentModeration => "\nResponse blocked by content moderation",
            LanguageModelResponseStatus.PromptLargerThanContext => "\nPrompt larger than context",
            LanguageModelResponseStatus.Error => "\nError",
            _ => string.Empty,
        };
    }
}
