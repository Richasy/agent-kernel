// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.AI;
using Richasy.AgentKernel;
using Richasy.AgentKernel.Connectors.AzureOpenAI.Models;

var kernel = Kernel.CreateBuilder()
    .AddAzureOpenAIChatCompletion()
    .Build();

var aoaiService = kernel.GetChatCompletionService("AzureOpenAI");
aoaiService.Initialize(new AzureAIServiceConfig(new Uri("https://richasy-dalle.openai.azure.com"), "ea236cce4e7541b8b57c33bd52af040d", "gpt-4o"));
var input = Console.ReadLine();
var msg = new ChatMessage(ChatRole.User, input);
var options = new ChatOptions
{
    Temperature = 0.7f,
};
var response = await aoaiService.Client!.CompleteAsync([msg], options);
if (response is null)
{
    Console.WriteLine("Failed to complete chat.");
}
else
{
    var firstMsg = response.Message.Text;
    Console.WriteLine(firstMsg);
}

Console.ReadKey();