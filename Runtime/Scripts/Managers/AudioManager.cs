using System;
using System.Collections.Generic;
using System.Linq;
using LCHFramework.Data;
using LCHFramework.Extensions;
using UnityEngine;

namespace LCHFramework.Managers
{
    public enum AudioPlayType
    {
        NestableAudio,
        FadeableAudio,
        StoppableAudio,
        SkippableAudio,
    }

    public class AudioSourceControllerType
    {
        public static readonly AudioSourceControllerType Bgm = new() { value = nameof(Bgm) };
        public static readonly AudioSourceControllerType Narration = new() { value = nameof(Narration) };
        public static readonly AudioSourceControllerType Sfx = new() { value = nameof(Sfx) };

        public string value;
    }
    
    public struct AudioPlayResult
    {
        public static AudioPlayResult fail = new();

        public AudioSource audioSource;
        public bool isSuccess;
        public float audioClipLength;
        
        public bool isFail => !isSuccess;
    }
    
    public class AudioManager : MonoSingleton<AudioManager>
    {
        public const AudioPlayType DefaultAudioPlayType = AudioPlayType.NestableAudio;
        public const float DefaultVolume = 1f;
        public const bool DefaultLoop = false;
        public const float FadeDuration = 1f;
        
        public static readonly AudioSourceControllerType DefaultAudioSourceControllerType = AudioSourceControllerType.Sfx;
        public static readonly ReactiveProperty<float> MasterVolume = new() { Value = DefaultVolume };
        public static readonly Dictionary<AudioSourceControllerType, ReactiveProperty<float>> LocalVolumes = new();
        
        
        public static float TimeScale
        {
            get => _timeScale;
            set
            {
                Instance.controllers.Values.ForEach(t => t.SetAudioSourcesTimeScale(value));
                _timeScale = value;
            }
        }
        private static float _timeScale = -1;
        
        
        
        private readonly Dictionary<AudioSourceControllerType, AudioSourceController> controllers = new();
        
        
        protected override bool IsDontDestroyOnLoad => true;
        
        
        
        protected override void Awake()
        {
            base.Awake();

            foreach (var t in new[] { AudioSourceControllerType.Bgm, AudioSourceControllerType.Narration, AudioSourceControllerType.Sfx }) CreateAudioController(t);
        }

        protected override void Start()
        {
            base.Start();
            
            if (_timeScale < 0) _timeScale = Time.timeScale;
        }
        
        
        
        protected void CreateAudioController(AudioSourceControllerType type)
        {
            if (controllers.ContainsKey(type)) return;

            if (controllers.Keys.Any(t => t.value == type.value)) return;
            
            var go = new GameObject($"{type.value}");
            go.transform.SetParent(transform);
            LocalVolumes.Add(type, new ReactiveProperty<float> { Value = DefaultVolume });
            var audioSourceController = go.AddComponent<AudioSourceController>();
            controllers.Add(audioSourceController.type = type, audioSourceController);
        }
        
        public AudioPlayResult Play(AudioClip audioClip, AudioPlayType audioPlayType = DefaultAudioPlayType, float volume = DefaultVolume, bool loop = DefaultLoop)
            => _Play(audioClip, controllers[DefaultAudioSourceControllerType], audioPlayType, volume, loop);
        
        public AudioPlayResult Play(AudioClip audioClip, AudioSourceControllerType audioSourceControllerType, AudioPlayType audioPlayType = DefaultAudioPlayType, float volume = DefaultVolume, bool loop = DefaultLoop)
            => _Play(audioClip, controllers[audioSourceControllerType], audioPlayType, volume, loop);
        
        public AudioPlayResult Play(AudioClip audioClip, string audioClipFilePath, AudioPlayType audioPlayType = DefaultAudioPlayType, float volume = DefaultVolume, bool loop = DefaultLoop)
            => _Play(audioClip, controllers.Values.FirstOrDefault(t => audioClipFilePath.Contains(t.type.value, StringComparison.OrdinalIgnoreCase)), audioPlayType, volume, loop);

        private AudioPlayResult _Play(AudioClip audioClip, AudioSourceController audioSourceController, AudioPlayType audioPlayType, float volume, bool loop)
        {
            ReleaseAudioSources();
            return audioSourceController.Play(audioClip, volume, loop, audioPlayType);
        }

        public void UnPauseAll() => controllers.Values.ForEach(t => t.UnPauseAudioSources());
        
        public void PauseAll() => controllers.Values.ForEach(t => t.PauseAllAudioSources());
        
        public void StopAll() => controllers.Values.ForEach(t => t.StopAllAudioSources());

        public void ReleaseAudioSources() => controllers.Values.ForEach(t => t.ReleaseAudioSources(t.IsPlayingAudioSources));
    }
}
