// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Models;

namespace Richasy.AgentKernel.Connectors.Google.Models;

/// <summary>
/// Configuration for Google translation service.
/// </summary>
public sealed class GoogleTranslationServiceConfig(string apiKey) : TranslationServiceConfig(apiKey)
{
}
