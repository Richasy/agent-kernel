// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.AI;
using Microsoft.Windows.AI.Text;
using System.Runtime.CompilerServices;
using Windows.Foundation;

namespace Richasy.AgentKernel.Connectors.Windows.Core;

/// <summary>
/// Windows Chat Client.
/// </summary>
public sealed class WindowsChatClient : IChatClient
{
    private LanguageModel? _model;
    private TextRewriter? _textRewriter;

    /// <summary>
    /// Initializes a new instance of the <see cref="WindowsChatClient"/> class.
    /// </summary>
    public WindowsChatClient()
        => Metadata = new("windows", default, "PhiSilica");

    /// <inheritdoc/>
    public ChatClientMetadata Metadata { get; }

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

        _textRewriter ??= new TextRewriter(_model);
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

    /// <inheritdoc/>
    public void Dispose()
    {
        _model?.Dispose();
        _model = null;
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
        if (_model != null)
        {
            return;
        }

        cancellationToken.ThrowIfCancellationRequested();

        if (!IsAvailable())
        {
            await LanguageModel.EnsureReadyAsync();
        }

        cancellationToken.ThrowIfCancellationRequested();
        _model = await LanguageModel.CreateAsync();
    }

    private static string GetPrompt(IEnumerable<ChatMessage> history)
    {
        if (!history.Any())
        {
            return string.Empty;
        }

        var prompt = string.Empty;
        for (var i = 0; i < history.Count(); i++)
        {
            var message = history.ElementAt(i);
            var msgText = message.Text ?? string.Empty;
            if (message.Role == ChatRole.System)
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

    private async IAsyncEnumerable<string> GenerateStreamResponseAsync(string prompt, ChatOptions? options = null, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        if (_model == null)
        {
            throw new InvalidOperationException("Language model is not loaded.");
        }

        _textRewriter ??= new TextRewriter(_model);
        var currentResponse = string.Empty;
        using var newPartEvent = new ManualResetEventSlim(false);

        IAsyncOperationWithProgress<LanguageModelResponseResult, string>? progress;

        progress = _textRewriter.RewriteAsync(prompt);

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
