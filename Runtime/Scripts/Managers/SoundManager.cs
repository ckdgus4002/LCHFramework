using System.Collections.Generic;
using LCHFramework.Extensions;
using UniRx;
using UnityEngine;

namespace LCHFramework.Managers
{
    public enum AudioPlayType
    {
        NestableAudio,
        StoppableAudio,
        SkippableAudio,
    }
    
    public struct AudioPlayResult
    {
        public static AudioPlayResult fail = new() { isFail = true };
        
        public bool isFail;
        public bool isSuccess;
        public float audioClipLength;
        public AudioSource audioSource;
    }
    
    public class SoundManager : MonoSingleton<SoundManager>
    {
        public const string Bgm = "Bgm";
        public const string Narration = "Narration";
        public const string Sfx = "Sfx";
        public const string DefaultAudioSourcePoolName = Sfx;
        public const AudioPlayType DefaultAudioPlayType = AudioPlayType.NestableAudio;
        public const float DefaultVolume = 1f;
        public const bool DefaultLoop = false;
        public const float FadeDuration = 1f;
        
        
        
        public static readonly ReactiveProperty<float> MasterVolume = new() { Value = DefaultVolume };
        public static readonly Dictionary<string, ReactiveProperty<float>> LocalVolumes = new();
        
        
        public static float TimeScale
        {
            get => _timeScale;
            set
            {
                Instance.audioSourcePools.ForEach(t => t.Value.SetAudioSourcesTimeScale(value));
                _timeScale = value;
            }
        }
        private static float _timeScale;
        
        
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void RuntimeInitializeOnLoadMethod() => CreateGameObjectIfInstanceIsNull();
        
        
        
        protected readonly Dictionary<string, AudioSourcePool> audioSourcePools = new();
        
        
        protected override bool IsDontDestroyOnLoad => true;
        
        
        
        protected override void Awake()
        {
            base.Awake();
            
            TimeScale = Time.timeScale;
            foreach (var t in new[] { Bgm, Narration, Sfx }) CreateAudioSourcePool(t);
        }
        
        
        
        protected void CreateAudioSourcePool(string poolName)
        {
            if (audioSourcePools.ContainsKey(poolName)) return;
            
            var go = new GameObject($"{poolName}");
            go.transform.SetParent(transform);
            audioSourcePools.Add(poolName, go.AddComponent<AudioSourcePool>());
            LocalVolumes.Add(poolName, new ReactiveProperty<float> { Value = DefaultVolume });
        }
        
        public AudioPlayResult Play(AudioClip audioClip, string audioSourcePoolName = DefaultAudioSourcePoolName, AudioPlayType audioPlayType = DefaultAudioPlayType, float volume = DefaultVolume, bool loop = DefaultLoop, Vector3? position = null)
        {
            // ReleaseAll();
            var audioSourcePool = !audioSourcePools.TryGetValue(audioSourcePoolName, out var result) ? audioSourcePools[DefaultAudioSourcePoolName] : result;
            return audioSourcePool.Play(audioClip, volume, loop, position ?? transform.position, audioPlayType);
        }
        
        public void StopAll() => audioSourcePools.ForEach(t => t.Value.StopAllAudioSources());
        
        public void DisposeAllAudioSourcePool() => audioSourcePools.ForEach(t => t.Value.DisposeAudioSourcePool());
        
        private void ReleaseAll() => audioSourcePools.ForEach(t => t.Value.ReleaseAudioSources());
    }
}