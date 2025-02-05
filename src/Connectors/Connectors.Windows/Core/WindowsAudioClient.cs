// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Audio;
using Richasy.AgentKernel.Models;
using Windows.Media.SpeechSynthesis;
using System.Runtime.InteropServices.WindowsRuntime;
using Win = Windows.Storage.Streams;

namespace Richasy.AgentKernel.Connectors.Windows.Core;

/// <summary>
/// Represents an audio client that uses Windows.
/// </summary>
public sealed class WindowsAudioClient : IAudioClient
{
    private readonly SpeechSynthesizer _synthesizer;

    /// <summary>
    /// Initializes a new instance of the <see cref="WindowsAudioClient"/> class.
    /// </summary>
    public WindowsAudioClient() => _synthesizer = new SpeechSynthesizer();

    /// <inheritdoc/>
    public AudioClientMetadata Metadata { get; } = new("windows", "local");

    /// <inheritdoc/>
    public void Dispose() => _synthesizer.Dispose();

    /// <inheritdoc/>
    public async Task<BinaryData> TextToSpeechAsync(string text, AudioOptions? options, CancellationToken cancellationToken = default)
    {
        var voice = SpeechSynthesizer.AllVoices.FirstOrDefault(p => p.Id == options?.VoiceId) ?? SpeechSynthesizer.DefaultVoice;
        _synthesizer.Voice = voice;
        _synthesizer.Options.SpeakingRate = options?.Speed ?? 1.0;
        var stream = await _synthesizer.SynthesizeTextToStreamAsync(text).AsTask(cancellationToken).ConfigureAwait(false);
        var buffer = new Win.Buffer((uint)stream.Size);
        await stream.ReadAsync(buffer, (uint)stream.Size, Win.InputStreamOptions.None).AsTask().ConfigureAwait(false);
        return new BinaryData(buffer.ToArray(), "audio/wav");
    }
}
