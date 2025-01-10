using System;
using System.Collections;
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
        
        
        public AudioSourceControllerType type;
        private readonly List<AudioSource> audioSources = new();
                
        
        public bool IsPlaying() => IsPlaying(IsPlayingAudioSources);
        
        public bool IsPlaying(IEnumerable<AudioSource> isPlayingAudioSources) => !audioSources.IsEmpty();

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
            var isPlayingAudioSources = IsPlayingAudioSources.ToArray();
            var isPlaying = IsPlaying(isPlayingAudioSources);
            
            if (audioPlayType == AudioPlayType.FadeableAudio && isPlaying)
            {
                var audioSource = AudioSourcePool.Get();
                isPlayingAudioSources.ForEach(t => StartCoroutine(FadeAudioSourceVolumeCor(t, 0, () =>
                {
                    isPlayingAudioSources.ForEach(StopAudioSource);
                    ReleaseAudioSources(isPlayingAudioSources);
                    PlayAudioSource(audioSource, audioClip, volume, loop, audioPlayType);
                })));
                return new AudioPlayResult { audioSource = audioSource, isSuccess = true, audioClipLength = audioClip.length };
            }
            else if (audioPlayType == AudioPlayType.StoppableAudio && isPlaying)
            {
                isPlayingAudioSources.ForEach(StopAudioSource);
                ReleaseAudioSources(isPlayingAudioSources);
            }
            else if (audioPlayType == AudioPlayType.SkippableAudio && isPlaying) 
                return AudioPlayResult.fail;
            
            return PlayAudioSource(audioClip, volume, loop, audioPlayType);
        }

        private IEnumerator FadeAudioSourceVolumeCor(AudioSource audioSource, float volume, Action callback = null)
        {
            var startVolume = audioSource.volume;
            var startTime = Time.time;
            var endTime = startTime + AudioManager.FadeDuration;
            while (Time.time < endTime)
            {
                audioSource.volume = Mathf.Lerp(startVolume, volume, (Time.time - startTime) / (endTime - startTime));
                yield return null;
            }
            audioSource.volume = volume;
            callback?.Invoke();
        }

        private AudioPlayResult PlayAudioSource(AudioClip audioClip, float volume, bool loop, AudioPlayType audioPlayType)
            => PlayAudioSource(AudioSourcePool.Get(), audioClip, volume, loop, audioPlayType);

        private AudioPlayResult PlayAudioSource(AudioSource audioSource, AudioClip audioClip, float volume, bool loop, AudioPlayType audioPlayType)
        {
            audioSource.name = audioClip.name;
            SetAudioSourceTimeScale(audioSource, AudioManager.TimeScale);
            audioSource.clip = audioClip;
            audioSource.loop = loop;
            audioSource.Play();
            var calculatedVolume = AudioManager.MasterVolume.Value * AudioManager.LocalVolumes[type].Value * volume;
            if (audioPlayType == AudioPlayType.FadeableAudio) StartCoroutine(FadeAudioSourceVolumeCor(audioSource, calculatedVolume));
            else audioSource.volume = calculatedVolume;
            
            return new AudioPlayResult { audioSource = audioSource, isSuccess = true, audioClipLength = audioClip.length };
        }

        public void SetAudioSourcesTimeScale(float timeScale) => audioSources.ForEach(t => SetAudioSourceTimeScale(t, timeScale));

        private void SetAudioSourceTimeScale(AudioSource audioSource, float timeScale) => audioSource.pitch = timeScale;
        
        public void UnPauseAudioSources() => audioSources.ForEach(t => t.UnPause());
        
        public void PauseAllAudioSources() => audioSources.ForEach(t => t.Pause());

        public void StopAllAudioSources() => audioSources.ForEach(StopAudioSource);

        private void StopAudioSource(AudioSource audioSource)
        {
            audioSource.Stop();
            ReleaseAudioSource(audioSource);
        }
        
        public void ReleaseAudioSources(IEnumerable<AudioSource> values) => values.ForEach(ReleaseAudioSource);

        private void ReleaseAudioSource(AudioSource value) => AudioSourcePool.Release(value);
    }
}