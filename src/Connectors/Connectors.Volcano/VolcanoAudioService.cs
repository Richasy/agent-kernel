// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Audio;
using Richasy.AgentKernel.Connectors.Volcano.Core;
using Richasy.AgentKernel.Connectors.Volcano.Models;
using Richasy.AgentKernel.Models;
using RichasyKernel;

namespace Richasy.AgentKernel.Connectors.Volcano;

/// <summary>
/// 火山AI音频服务.
/// </summary>
public sealed class VolcanoAudioService : IAudioService
{
    private VolcanoAudioServiceConfig? _config;
    private AudioModel[]? _defaultModels;

    /// <inheritdoc/>
    public IAudioClient? Client { get; set; }

    /// <inheritdoc/>
    public AIServiceConfig? Config { get; private set; }

    /// <inheritdoc/>
    public void Initialize(AIServiceConfig? config)
    {
        if (config is not VolcanoAudioServiceConfig volcanoConfig)
        {
            throw new KernelException("Configuration is invalid");
        }

        if (_config != null && volcanoConfig.Equals(_config))
        {
            return;
        }

        _config = volcanoConfig;
        Client?.Dispose();
        Client = new VolcanoAudioClient(volcanoConfig);
    }

    /// <inheritdoc/>
    public IReadOnlyList<AudioModel> GetPredefinedModels()
    {
        if (_defaultModels == null)
        {
            //List<AudioVoice> basicVoices = [
            //    new ("BV701_streaming", "擎苍", VoiceGender.Male, "zh-CN", "en-US"),
            //    new ("BV123_streaming", "阳光青年", VoiceGender.Male, "zh-CN", "en-US"),
            //    new ("BV120_streaming", "反卷青年", VoiceGender.Male, "zh-CN", "en-US"),
            //    new ("BV119_streaming", "通用赘婿", VoiceGender.Male, "zh-CN", "en-US"),
            //    new ("BV115_streaming", "古风少御", VoiceGender.Female, "zh-CN", "en-US"),
            //    new ("BV107_streaming", "霸气青叔", VoiceGender.Male, "zh-CN", "en-US"),
            //    new ("BV100_streaming", "质朴青年", VoiceGender.Male, "zh-CN", "en-US"),
            //    new ("BV104_streaming", "温柔淑女", VoiceGender.Female, "zh-CN", "en-US"),
            //    new ("BV004_streaming", "开朗青年", VoiceGender.Male, "zh-CN", "en-US"),
            //    new ("BV113_streaming", "甜宠少御", VoiceGender.Male, "zh-CN", "en-US"),
            //    new ("BV102_streaming", "儒雅青年", VoiceGender.Male, "zh-CN", "en-US"),
            //    ];
            List<AudioVoice> bigVoices = [
                new ("zh_male_qingcang_mars_bigtts", "擎苍", VoiceGender.Male, "zh-CN"),
                new ("zh_male_changtianyi_mars_bigtts", "悬疑解说", VoiceGender.Male, "zh-CN"),
                new ("zh_male_fanjuanqingnian_mars_bigtts", "反卷青年", VoiceGender.Male, "zh-CN"),
                new ("zh_female_gufengshaoyu_mars_bigtts", "古风少御", VoiceGender.Female, "zh-CN"),
                new ("zh_male_baqiqingshu_mars_bigtts", "霸气青叔", VoiceGender.Male, "zh-CN"),
                new ("zh_male_yangguangqingnian_mars_bigtts", "活力小哥", VoiceGender.Male, "zh-CN"),
                new ("zh_female_wenroushunv_mars_bigtts", "温柔淑女", VoiceGender.Female, "zh-CN"),
                new ("zh_male_ruyaqingnian_mars_bigtts", "儒雅青年", VoiceGender.Male, "zh-CN"),
                ];

            _defaultModels = [
                //new AudioModel{
                //    Id = "VolcanoBasic",
                //    Name = "标准音库",
                //    Voices = basicVoices
                //},
                new AudioModel {
                    Id = "VolcanoBig",
                    Name = "大模型音库",
                    Voices = bigVoices
                }];
        }

        return _defaultModels;
    }
}
