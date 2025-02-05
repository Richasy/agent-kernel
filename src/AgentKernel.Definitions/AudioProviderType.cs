// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

namespace Richasy.AgentKernel;

/// <summary>
/// 讲述人类型.
/// </summary>
public enum AudioProviderType
{
    /// <summary>
    /// Open AI.
    /// </summary>
    OpenAI,

    /// <summary>
    /// Azure Open AI.
    /// </summary>
    AzureOpenAI,

    /// <summary>
    /// Azure 语音服务.
    /// </summary>
    Azure,

    /// <summary>
    /// Edge 语音服务.
    /// </summary>
    Edge,

    /// <summary>
    /// Windows 语音服务.
    /// </summary>
    Windows,
}
