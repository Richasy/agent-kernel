// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Audio;
using Richasy.AgentKernel.Models;
using RichasyKernel;
using System.Net.WebSockets;
using System.Security.Cryptography;
using System.Text;

namespace Richasy.AgentKernel.Connectors.Azure.Core;

/// <summary>
/// Edge audio client.
/// </summary>
public sealed class EdgeAudioClient : IAudioClient
{
    private const string _suggestCodec = "audio-24khz-48kbitrate-mono-mp3";

    /// <inheritdoc/>
    public AudioClientMetadata Metadata { get; } = new("edge", default);

    /// <inheritdoc/>
    public void Dispose()
    {
    }

    /// <inheritdoc/>
    public async Task<BinaryData> TextToSpeechAsync(string text, AudioOptions? options, CancellationToken cancellationToken = default)
    {
        const string BinaryDelim = "Path:audio\r\n";
        var sendRequestId = Guid.NewGuid().ToString("N");
        var binary = new List<byte>();

        var taskCompletionSource = new TaskCompletionSource<BinaryData>();
        using var client = new ClientWebSocket();
        await client.ConnectAsync(new Uri($"wss://speech.platform.bing.com/consumer/speech/synthesize/readaloud/edge/v1?TrustedClientToken=6A5AA1D4EAFF4E9FB37E23D68491D6F4&Sec-MS-GEC={GenerateSecMsGecToken()}&Sec-MS-GEC-Version=1-130.0.2849.68"), cancellationToken).ConfigureAwait(false);
        var receiveTask = Task.Run(async () =>
        {
            var buffer = new byte[1024 * 4];
            while (client.State == WebSocketState.Open)
            {
                var result = await client.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None).ConfigureAwait(false);
                if (result.MessageType == WebSocketMessageType.Text)
                {
                    var data = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    if (data.Contains("Path:turn.end", StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (binary.Count > 0)
                        {
                            var content = new BinaryData([.. binary], "audio/mp3");
                            taskCompletionSource.SetResult(content);
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
                    if (data.Length >= 3 && data[0] == 0x00 && data[1] == 0x67 && data[2] == 0x58)
                    {
                        // Last (empty) audio fragment.
                    }
                    else
                    {
                        var index = Encoding.UTF8.GetString(data).IndexOf(BinaryDelim, StringComparison.InvariantCultureIgnoreCase) + BinaryDelim.Length;
                        if (index < BinaryDelim.Length)
                        {
                            binary.AddRange(data);
                        }
                        else
                        {
                            var curVal = data[index..];
                            binary.AddRange(curVal);
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

        await Task.Run(() =>
        {
            client.SendAsync(Encoding.UTF8.GetBytes(ConvertToAudioFormatWebSocketString(_suggestCodec)), WebSocketMessageType.Text, true, CancellationToken.None).Wait();
            if (text.StartsWith("<speak", StringComparison.InvariantCultureIgnoreCase))
            {
                client.SendAsync(Encoding.UTF8.GetBytes(ConvertToWebSocketString(sendRequestId, text)), WebSocketMessageType.Text, true, CancellationToken.None).Wait();
            }
            else
            {
                client.SendAsync(Encoding.UTF8.GetBytes(ConvertToWebSocketString(sendRequestId, ConvertToSsmlText(options!.LanguageCode!, options!.VoiceId!, options!.Speed ?? 1d, text))), WebSocketMessageType.Text, true, CancellationToken.None).Wait();
            }
        }, cancellationToken).ConfigureAwait(false);

        await receiveTask.ConfigureAwait(false);
        return await taskCompletionSource.Task.ConfigureAwait(false);
    }

    private static string GenerateSecMsGecToken()
    {
        var ticks = DateTime.Now.ToFileTimeUtc();
        ticks -= ticks % 3_000_000_000;
        return ToHexString(HashData(Encoding.ASCII.GetBytes(ticks + "6A5AA1D4EAFF4E9FB37E23D68491D6F4")));
    }

    private static string ToHexString(byte[] byteArray)
        => Convert.ToHexString(byteArray).ToUpperInvariant();

    private static byte[] HashData(byte[] data)
        => SHA256.HashData(data);

    private static string ConvertToSsmlText(string lang, string voice, double speed, string text)
        => $"<speak version='1.0' xmlns='http://www.w3.org/2001/10/synthesis' xml:lang='{lang}'><voice name='{voice}'><prosody pitch='+0Hz' rate='{FormatPercentage(speed)}'>{text}</prosody></voice></speak>";

    private static string ConvertToAudioFormatWebSocketString(string outputformat)
        => "Content-Type:application/json; charset=utf-8\r\nPath:speech.config\r\n\r\n{\"context\":{\"synthesis\":{\"audio\":{\"metadataoptions\":{\"sentenceBoundaryEnabled\":\"false\",\"wordBoundaryEnabled\":\"false\"},\"outputFormat\":\"" + outputformat + "\"}}}}";

    private static string ConvertToWebSocketString(string requestId, string msg)
        => $"X-RequestId:{requestId}\r\nContent-Type:application/ssml+xml\r\nPath:ssml\r\n\r\n{msg}";

    private static string FormatPercentage(double input)
        => ((input - 1) * 100) + "%";
}
