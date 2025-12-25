// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Audio;
using Richasy.AgentKernel.Connectors.Azure.Models.Audio;
using Richasy.AgentKernel.Models;
using RichasyKernel;
using System.Net.WebSockets;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Richasy.AgentKernel.Connectors.Azure.Core;

/// <summary>
/// Edge audio client.
/// </summary>
public sealed class EdgeAudioClient : IAudioClient
{
    private const string EDGE_SPEECH_URL = "wss://speech.platform.bing.com/consumer/speech/synthesize/readaloud/edge/v1";
    private const string EDGE_API_TOKEN = "6A5AA1D4EAFF4E9FB37E23D68491D6F4";
    private const string CHROMIUM_FULL_VERSION = "143.0.3650.75";
    private const string SUGGEST_CODEC = "audio-24khz-48kbitrate-mono-mp3";

    /// <summary>
    /// Metadata.
    /// </summary>
    public AudioClientMetadata Metadata { get; } = new("edge", default);

    /// <inheritdoc/>
    public void Dispose() { }

    /// <summary>
    /// Text to speech.
    /// </summary>
    /// <param name="text"></param>
    /// <param name="options"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<BinaryData> TextToSpeechAsync(string text, AudioOptions? options, CancellationToken cancellationToken = default)
    {
        var connectionId = Guid.NewGuid().ToString("N");
        var binary = new List<byte>();
        var taskCompletionSource = new TaskCompletionSource<BinaryData>();

        using var client = new ClientWebSocket();

        // 设置请求头
        var majorVersion = CHROMIUM_FULL_VERSION.Split('.')[0];
        client.Options.SetRequestHeader("User-Agent",
            $"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/{majorVersion}.0.0.0 Safari/537.36 Edg/{majorVersion}.0.0.0");
        client.Options.SetRequestHeader("Accept-Encoding", "gzip, deflate, br, zstd");
        client.Options.SetRequestHeader("Accept-Language", "en-US,en;q=0.9");
        client.Options.SetRequestHeader("Pragma", "no-cache");
        client.Options.SetRequestHeader("Cache-Control", "no-cache");
        client.Options.SetRequestHeader("Origin", "chrome-extension://jdiccldimpdaibmpdkjnbmckianbfold");
        client.Options.SetRequestHeader("Cookie", $"muid={GenerateMuid()};");

        // 构建完整 URL（包含 ConnectionId）
        var url = $"{EDGE_SPEECH_URL}?ConnectionId={connectionId}&TrustedClientToken={EDGE_API_TOKEN}&Sec-MS-GEC={GenerateSecMsGecToken()}&Sec-MS-GEC-Version=1-{CHROMIUM_FULL_VERSION}";

        await client.ConnectAsync(new Uri(url), cancellationToken).ConfigureAwait(false);

        var receiveTask = Task.Run(async () =>
        {
            var buffer = new byte[1024 * 16]; // 增大 buffer
            while (client.State == WebSocketState.Open)
            {
                var result = await client.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None).ConfigureAwait(false);
                if (result.MessageType == WebSocketMessageType.Text)
                {
                    var data = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    if (data.Contains("Path:turn.end", StringComparison.OrdinalIgnoreCase))
                    {
                        if (binary.Count > 0)
                        {
                            taskCompletionSource.SetResult(new BinaryData([.. binary], "audio/mp3"));
                        }
                        else
                        {
                            taskCompletionSource.SetException(new KernelException("Edge speech result is empty."));
                        }
                        break;
                    }
                }
                else if (result.MessageType == WebSocketMessageType.Binary)
                {
                    var data = new ArraySegment<byte>(buffer, 0, result.Count).ToArray();
                    // 使用 header length 方式解析
                    if (data.Length > 2)
                    {
                        var headerLength = (data[0] << 8) | data[1]; // Big-endian Int16
                        if (data.Length > headerLength + 2)
                        {
                            var audioData = data.Skip(2 + headerLength).ToArray();
                            binary.AddRange(audioData);
                        }
                    }
                }
                else if (result.MessageType == WebSocketMessageType.Close)
                {
                    taskCompletionSource.SetException(new KernelException(result.CloseStatusDescription ?? "Edge speech connection closed."));
                    break;
                }
            }
        }, cancellationToken);

        var timestamp = DateTime.UtcNow.ToString("R");

        var config = new EdgeSpeechConfig
        {
            Context = new EdgeContext
            {
                Synthesis = new EdgeSynthesis
                {
                    Audio = new EdgeAudio
                    {
                        MetadataOptions = new EdgeMetadataOptions
                        {
                            SentenceBoundaryEnabled = false,
                            WordBoundaryEnabled = true
                        },
                        OutputFormat = SUGGEST_CODEC
                    }
                }
            }
        };
        var configJson = JsonSerializer.Serialize(config, JsonGenContext.Default.EdgeSpeechConfig);
        var configMessage = $"Content-Type: application/json; charset=utf-8\r\nPath: speech.config\r\nX-Timestamp: {timestamp}\r\n\r\n{configJson}";
        await client.SendAsync(Encoding.UTF8.GetBytes(configMessage), WebSocketMessageType.Text, true, cancellationToken).ConfigureAwait(false);

        // 发送 SSML 消息
        var ssml = text.StartsWith("<speak", StringComparison.OrdinalIgnoreCase)
            ? text
            : ConvertToSsmlText(options!.LanguageCode!, options!.VoiceId!, options!.Speed ?? 1d, text);

        var ssmlMessage = $"Content-Type: application/ssml+xml\r\nPath: ssml\r\nX-RequestId: {connectionId}\r\nX-Timestamp: {timestamp}\r\n\r\n{ssml}";
        await client.SendAsync(Encoding.UTF8.GetBytes(ssmlMessage), WebSocketMessageType.Text, true, cancellationToken).ConfigureAwait(false);

        await receiveTask.ConfigureAwait(false);
        return await taskCompletionSource.Task.ConfigureAwait(false);
    }

    private static string GenerateSecMsGecToken()
    {
        const long WIN_EPOCH_OFFSET = 11644473600L;
        const long S_TO_NS = 10_000_000L;

        long ticks = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        ticks += WIN_EPOCH_OFFSET;
        ticks -= ticks % 300;
        ticks *= S_TO_NS;

        var strToHash = $"{ticks}{EDGE_API_TOKEN}";
        return Convert.ToHexString(SHA256.HashData(Encoding.ASCII.GetBytes(strToHash)));
    }

    private static string GenerateMuid()
    {
        var bytes = new byte[16];
        RandomNumberGenerator.Fill(bytes);
        return Convert.ToHexString(bytes);
    }

    private static string ConvertToSsmlText(string lang, string voice, double speed, string text)
        => $"<speak version='1.0' xmlns='http://www.w3.org/2001/10/synthesis' xml:lang='{lang}'><voice name='{voice}'><prosody pitch='+0Hz' rate='{FormatPercentage(speed)}'>{SecurityElement.Escape(text)}</prosody></voice></speak>";

    private static string FormatPercentage(double input)
        => $"{(input - 1) * 100:+0;-0;+0}%";
}

