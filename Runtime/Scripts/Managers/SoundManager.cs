using System.Collections;
using System.Collections.Generic;
using LCHFramework.Extensions;
using UniRx;
using UnityEngine;

namespace LCHFramework.Managers
{
    public enum AudioPlayType
    {
        /// 동시 재생.
        NestableAudio, 
        /// 재생중이던것을 멈추고 재생.
        StoppableAudio, 
        /// 재생중이지 않으면 재생.
        SkippableAudio, 
    }
    
    public struct SoundPlayResult
    {
        public static SoundPlayResult fail = new() { isFail = true };
        
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
        
        
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void RuntimeInitializeOnLoadMethod() => CreateGameObjectIfInstanceIsNull();
        
        
        
        public readonly Dictionary<string, AudioSourcePool> audioSourcePools = new();
        
        
        protected override bool IsDontDestroyOnLoad => transform.parent == null;
        
        public override bool IsDestroyPrevInstance => false;
        
        
        
        protected override void Awake()
        {
            base.Awake();
            
            foreach (var t in new[] { Bgm, Narration, Sfx }) CreateAudioSourcePool(t);
        }
        
        protected override void OnEnable()
        {
            base.OnEnable();
            
            TimeScale = Time.timeScale;
        }
        
        
        
        protected void CreateAudioSourcePool(string poolName)
        {
            if (audioSourcePools.ContainsKey(poolName)) return;
            
            var go = new GameObject($"{poolName}");
            go.transform.SetParent(transform);
            audioSourcePools.Add(poolName, go.AddComponent<AudioSourcePool>());
            LocalVolumes.Add(poolName, new ReactiveProperty<float> { Value = DefaultVolume });
        }
        
        public virtual SoundPlayResult Play(AudioClip audioClip, string audioSourcePoolName = DefaultAudioSourcePoolName, AudioPlayType audioPlayType = DefaultAudioPlayType, float volume = DefaultVolume, bool loop = DefaultLoop, Vector3? position = null)
        {
            var audioSourcePool = !audioSourcePools.TryGetValue(audioSourcePoolName, out var result) ? audioSourcePools[DefaultAudioSourcePoolName] : result;
            var soundPlayResult = audioSourcePool.Play(audioClip, volume, loop, position ?? transform.position, audioPlayType);
            Debug.Log($"[SoundManager] Play: {(audioClip == null ? "Null" : audioClip.name)}, Result: {soundPlayResult.isSuccess}.");
            return soundPlayResult;
        }
        
        public void StopAll() => audioSourcePools.ForEach(t => t.Value.StopAllAudioSources());
        
        public void ClearAll() => audioSourcePools.ForEach(t => t.Value.ClearAudioSourcePool());
    }
}