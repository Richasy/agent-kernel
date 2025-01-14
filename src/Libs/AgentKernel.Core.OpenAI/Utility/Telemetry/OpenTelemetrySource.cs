using Richasy.AgentKernel.Core.OpenAI.Chat;
using System;

namespace Richasy.AgentKernel.Core.OpenAI.Telemetry;

internal class OpenTelemetrySource
{
    private const string ChatOperationName = "chat";
    private readonly bool IsOTelEnabled = AppContextSwitchHelper
        .GetConfigValue("OpenAI.Experimental.EnableOpenTelemetry", "OPENAI_EXPERIMENTAL_ENABLE_OPEN_TELEMETRY");

    private readonly string _serverAddress;
    private readonly int _serverPort;
    private readonly string _model;

    public OpenTelemetrySource(string model, Uri endpoint)
    {
        _serverAddress = endpoint.Host;
        _serverPort = endpoint.Port;
        _model = model;
    }

    public OpenTelemetryScope StartChatScope(ChatCompletionOptions completionsOptions)
    {
        return IsOTelEnabled
            ? OpenTelemetryScope.StartChat(_model, ChatOperationName, _serverAddress, _serverPort, completionsOptions)
            : null;
    }

}
