// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Consoles.Chat;
using System.Text;
using System.Text.Json;

/// <summary>
/// Program entry.
/// </summary>
internal partial class Program
{
    private static ChatConfiguration? _config;

    private static void ConfigureConsole() => Console.OutputEncoding = Encoding.UTF8;

    private static async Task LoadConfigurationAsync()
    {
        var configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "env.json");
        if (!File.Exists(configPath))
        {
            throw new FileNotFoundException("Config file not found.");
        }

        var configContent = await File.ReadAllTextAsync(configPath);
        _config = JsonSerializer.Deserialize(configContent, JsonGenerationContext.Default.ChatConfiguration);
    }
}
