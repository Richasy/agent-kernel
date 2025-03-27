// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using Richasy.AgentKernel.Audio;
using Richasy.AgentKernel.Connectors.Tencent.Core;
using Richasy.AgentKernel.Connectors.Tencent.Models;
using Richasy.AgentKernel.Models;
using RichasyKernel;

namespace Richasy.AgentKernel.Connectors.Tencent;

/// <summary>
/// 腾讯TTS服务.
/// </summary>
public sealed class TencentAudioService : IAudioService
{
    private TencentAudioServiceConfig? _config;
    private AudioModel[]? _defaultModels;

    /// <inheritdoc/>
    public IAudioClient? Client { get; set; }

    /// <inheritdoc/>
    public AIServiceConfig? Config => _config;

    /// <inheritdoc/>
    public void Initialize(AIServiceConfig? config)
    {
        if (config is not TencentAudioServiceConfig tencentConfig)
        {
            throw new KernelException("Configuration is invalid");
        }

        if (_config != null && tencentConfig.Equals(_config))
        {
            return;
        }

        _config = tencentConfig;
        Client?.Dispose();
        Client = new TencentAudioClient(tencentConfig);
    }

    /// <inheritdoc/>
    public IReadOnlyList<AudioModel> GetPredefinedModels()
    {
        if (_defaultModels == null)
        {
            // 看上去超自然大模型音色不支持http调用.
            //var superVoices = new List<AudioVoice>
            //{
            //    new("502001", "智小柔", VoiceGender.Female, "zh-CN"),
            //};

            var bigVoices = new List<AudioVoice>
            {
                new("501000", "智斌", VoiceGender.Male, "zh-CN"),
                new("501001", "智兰", VoiceGender.Female, "zh-CN"),
                new("501002", "智菊", VoiceGender.Female, "zh-CN"),
                new("501003", "智宇", VoiceGender.Male, "zh-CN"),
                new("501004", "月华", VoiceGender.Female, "zh-CN"),
                new("601001", "爱小洛", VoiceGender.Female, "zh-CN"),
                new("601003", "爱小荷", VoiceGender.Female, "zh-CN"),
                new("601006", "爱小耀", VoiceGender.Male, "zh-CN"),
                new("501008", "WeJames", VoiceGender.Female, "en-US"),
                new("501009", "WeWinny", VoiceGender.Male, "en-US"),
            };

            var basicVoices = new List<AudioVoice>
            {
                new("100510000","智逍遥", VoiceGender.Male, "zh-CN"),
                new("101001","智瑜", VoiceGender.Female, "zh-CN"),
                new("101003","智美", VoiceGender.Female, "zh-CN"),
                new("101010","智华", VoiceGender.Male, "zh-CN"),
                new("101015","智萌", VoiceGender.Male, "zh-CN"),
                new("101016","智甜", VoiceGender.Male, "zh-CN"),
                new("101050","WeJack", VoiceGender.Male, "en-US"),
                new("101051","WeRose", VoiceGender.Female, "en-US"),
                new("101057","智美子", VoiceGender.Male, "jp-JP"),
            };

            _defaultModels =
            [
                //new AudioModel
                //{
                //    Id = "super",
                //    Name = "超自然大模型音色",
                //    Voices = superVoices
                //},
                new AudioModel
                {
                    Id = "big",
                    Name = "大模型音色",
                    Voices = bigVoices
                },
                new AudioModel
                {
                    Id = "basic",
                    Name = "精品音色",
                    Voices = basicVoices
                }
            ];
        }

        return _defaultModels;
    }
}
