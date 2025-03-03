// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.AI;
using Microsoft.Windows.AI.ContentModeration;
using Microsoft.Windows.AI.Generative;
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
    private const LanguageModelSkill DefaultLanguageModelSkill = LanguageModelSkill.General;
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
    public event EventHandler<PackageDeploymentResult>? ModelDownloaded;
#pragma warning restore CA1003 // 使用泛型事件处理程序实例

    /// <inheritdoc/>
    public void Dispose()
    {
        _model?.Dispose();
        _model = null;
    }

    /// <inheritdoc/>
    public Task<ChatResponse> GetResponseAsync(IList<ChatMessage> chatMessages, ChatOptions? options = null, CancellationToken cancellationToken = default)
        => GetStreamingResponseAsync(chatMessages, options, cancellationToken).ToChatResponseAsync(cancellationToken: cancellationToken);

    /// <inheritdoc/>
    public async IAsyncEnumerable<ChatResponseUpdate> GetStreamingResponseAsync(IList<ChatMessage> chatMessages, ChatOptions? options = null, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await InitializeAsync(cancellationToken).ConfigureAwait(false);
        if (_model == null)
        {
            throw new InvalidOperationException("Language model is not loaded.");
        }

        var prompt = GetPrompt(chatMessages);

        await foreach (var part in GenerateStreamResponseAsync(prompt, options, cancellationToken).ConfigureAwait(false))
        {
            yield return new ChatResponseUpdate
            {
                Role = ChatRole.Assistant,
                Text = part,
            };
        }
    }

    /// <inheritdoc/>
    public object? GetService(Type serviceType, object? serviceKey = null)
    {
        ArgumentNullException.ThrowIfNull(serviceType);
        return serviceKey is null && serviceType.IsInstanceOfType(this) ? this : null;
    }

    private static (LanguageModelOptions? ModelOptions, ContentFilterOptions? FilterOptions) GetModelOptions(ChatOptions? options)
    {
        if (options == null)
        {
            return (null, null);
        }

        var languageModelOptions = new LanguageModelOptions
        {
            Skill = options.AdditionalProperties?.TryGetValue("skill", out LanguageModelSkill skill) == true ? skill : DefaultLanguageModelSkill,
            Temp = options.Temperature ?? DefaultTemperature,
            Top_k = (uint)(options.TopK ?? DefaultTopK),
            Top_p = (uint)(options.TopP ?? DefaultTopP),
        };

        var contentFilterOptions = new ContentFilterOptions();

        if (options.AdditionalProperties?.TryGetValue("input_moderation", out SeverityLevel inputModeration) == true && inputModeration != SeverityLevel.None)
        {
            contentFilterOptions.PromptMinSeverityLevelToBlock = new TextContentFilterSeverity
            {
                HateContentSeverity = inputModeration,
                SexualContentSeverity = inputModeration,
                ViolentContentSeverity = inputModeration,
                SelfHarmContentSeverity = inputModeration
            };
        }

        if (options.AdditionalProperties?.TryGetValue("output_moderation", out SeverityLevel outputModeration) == true && outputModeration != SeverityLevel.None)
        {
            contentFilterOptions.ResponseMinSeverityLevelToBlock = new TextContentFilterSeverity
            {
                HateContentSeverity = outputModeration,
                SexualContentSeverity = outputModeration,
                ViolentContentSeverity = outputModeration,
                SelfHarmContentSeverity = outputModeration
            };
        }

        return (languageModelOptions, contentFilterOptions);
    }

    private string GetPrompt(IList<ChatMessage> history)
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

        for (var i = 0; i < history.Count; i++)
        {
            var message = history[i];
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
            return LanguageModel.IsAvailable();
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
            var operation = LanguageModel.MakeAvailableAsync();
            operation.Progress += HandleDeploymentProgress;
        }

        cancellationToken.ThrowIfCancellationRequested();

        _model = await LanguageModel.CreateAsync();
    }

    private void HandleDeploymentProgress(IAsyncOperationWithProgress<PackageDeploymentResult, PackageDeploymentProgress> asyncInfo, PackageDeploymentProgress progressInfo)
    {
        if (progressInfo.Status is PackageDeploymentProgressStatus.InProgress or PackageDeploymentProgressStatus.Queued)
        {
            DownloadingModel?.Invoke(this, progressInfo);
        }
        else
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

        if (!_model.IsPromptLargerThanContext(prompt))
        {
            IAsyncOperationWithProgress<LanguageModelResponse, string>? progress;
            if (options == null)
            {
                progress = _model.GenerateResponseWithProgressAsync(new LanguageModelOptions(), prompt, new ContentFilterOptions(), _modelContext);
            }
            else
            {
                var (modelOptions, filterOptions) = GetModelOptions(options);
                progress = _model.GenerateResponseWithProgressAsync(modelOptions, prompt, filterOptions, _modelContext);
            }

            progress.Progress = (result, value) =>
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
                LanguageModelResponseStatus.PromptBlockedByPolicy => "\nPrompt blocked by policy",
                LanguageModelResponseStatus.ResponseBlockedByPolicy => "\nResponse blocked by policy",
                _ => string.Empty,
            };
        }
        else
        {
            yield return "Prompt is too large for this model. Please submit a smaller prompt";
        }
    }
}
