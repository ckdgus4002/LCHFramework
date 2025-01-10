using System;
using System.Collections.Generic;
using System.Linq;
using LCHFramework.Extensions;
using UnityEngine;
using UnityEngine.Pool;

namespace LCHFramework.Managers
{
    public class AudioSourceController : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSourcePrefabOrNull;
        
        
        private readonly List<AudioSource> audioSources = new();
        
        
        public string Type => gameObject.name;

        public bool IsPlaying => !IsPlayingAudioSources.IsEmpty();

        public IEnumerable<AudioSource> IsPlayingAudioSources => audioSources.Where(t => t.isPlaying);
        
        private ObjectPool<AudioSource> AudioSourcePool => _audioSourcePool ??= new ObjectPool<AudioSource>(() => 
            {
                var source = audioSourcePrefabOrNull == null ? new GameObject().AddComponent<AudioSource>() : audioSourcePrefabOrNull; 
                source.transform.SetActive(transform);
                return source;
            },
            t => audioSources.Add(t), 
            t => audioSources.Remove(t));
        private ObjectPool<AudioSource> _audioSourcePool;
        
        
        
        public AudioPlayResult Play(AudioClip audioClip, float volume, bool loop, AudioPlayType audioPlayType)
        {
            if (audioPlayType == AudioPlayType.SkippableAudio && IsPlaying) return AudioPlayResult.fail;

            if (audioPlayType == AudioPlayType.FadeableAudio && IsPlaying)
            {
                
            }
            
            switch (audioPlayType)
            {
                case AudioPlayType.NestableAudio:
                case AudioPlayType.FadeableAudio:
                case AudioPlayType.StoppableAudio:
                case AudioPlayType.SkippableAudio:
                {
                    if (audioPlayType == AudioPlayType.FadeableAudio && IsPlaying)
                    {
                        var isPlayingAudioSources = IsPlayingAudioSources;
                    }
                    else if (audioPlayType == AudioPlayType.StoppableAudio && IsPlaying)
                    {
                        var isPlayingAudioSources = IsPlayingAudioSources;
                    }
                    else if (audioPlayType == AudioPlayType.SkippableAudio && IsPlaying)
                    {
                        var isPlayingAudioSources = IsPlayingAudioSources;
                    }
                    var source = AudioSourcePool.Get();
                    source.name = audioClip.name;
                    
                    source.pitch = AudioManager.TimeScale;
                    source.clip = audioClip;
                    var calculatedVolume = AudioManager.MasterVolume.Value * AudioManager.LocalVolumes[Type].Value * volume; 
                    source.volume = calculatedVolume;
                    source.loop = loop;
                    source.Play();
                    Invoke(nameof(ReleaseAudioSource), audioClip.length);
                    
                    return new AudioPlayResult { audioSourceOrNull = source };
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(audioPlayType), audioPlayType, null);
            }
        }

        public void SetTimeScale(float value) => audioSources.ForEach(t => t.pitch = value);
        
        public void UnPauseAll() => audioSources.ForEach(t => t.UnPause());
        
        public void PauseAll() => audioSources.ForEach(t => t.Pause());
        
        public void StopAll() => audioSources.ForEach(t => t.Stop());
        
        public void ReleaseAudioSources(IEnumerable<AudioSource> values) => values.ForEach(ReleaseAudioSource);

        private void ReleaseAudioSource(AudioSource value) => AudioSourcePool.Release(value);
    }
}