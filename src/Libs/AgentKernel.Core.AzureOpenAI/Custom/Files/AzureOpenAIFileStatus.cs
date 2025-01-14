// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace AgentKernel.Core.AzureOpenAI.Files;

public enum AzureOpenAIFileStatus
{
    Unknown,
    Uploaded,
    Pending,
    Running,
    Processed,
    Error,
    Deleting,
    Deleted
}
