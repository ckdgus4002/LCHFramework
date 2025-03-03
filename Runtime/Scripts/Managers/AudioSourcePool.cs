using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LCHFramework.Components;
using LCHFramework.Extensions;
using UnityEngine;
using UnityEngine.Pool;

namespace LCHFramework.Managers
{
    public class AudioSourcePool : LCHMonoBehaviour
    {
        private readonly List<AudioSource> audioSources = new();
        
        
        public bool IsPlaying(IEnumerable<AudioSource> isPlayingAudioSources = null) => !(isPlayingAudioSources ?? IsPlayingAudioSources).IsEmpty();

        public IEnumerable<AudioSource> IsPlayingAudioSources => audioSources.Where(t => t.isPlaying);
        
        private ObjectPool<AudioSource> ObjectPool => _audioSourcePool ??= new ObjectPool<AudioSource>(() => 
            {
                var audioSource = new GameObject().AddComponent<AudioSource>(); 
                audioSource.transform.SetParent(transform);
                return audioSource;
            },
            audioSource =>
            {
                audioSources.Add(audioSource);
                audioSource.SetActive(true);
            },
            audioSource =>
            {
                audioSources.Remove(audioSource);
                audioSource.SetActive(false);
            },
            audioSource =>
            {
                audioSources.Remove(audioSource);
            });
        private ObjectPool<AudioSource> _audioSourcePool;
        
        
        
        public AudioPlayResult Play(AudioClip audioClip, float volume, bool loop, Vector3 position, AudioPlayType audioPlayType)
        {
            var isPlayingAudioSources = IsPlayingAudioSources.ToArray();
            var isPlaying = IsPlaying(isPlayingAudioSources);
            var canFadeAudioSourceVolume = name == SoundManager.Bgm;
            if (audioPlayType == AudioPlayType.StoppableAudio && isPlaying && canFadeAudioSourceVolume)
            {
                var audioSource = ObjectPool.Get();
                isPlayingAudioSources.ForEach(t => StartCoroutine(FadeAudioSourceVolumeCor(t, 0, callback: () =>
                {
                    isPlayingAudioSources.ForEach(StopAudioSource);
                    PlayAudioSource(audioSource, audioClip, volume, loop, position, true);
                })));
                return new AudioPlayResult { isFail = false, isSuccess = true, audioClipLength = audioClip.length, audioSource = audioSource };
            }
            else if (audioPlayType == AudioPlayType.StoppableAudio && isPlaying && !canFadeAudioSourceVolume)
            {
                isPlayingAudioSources.ForEach(StopAudioSource);
                return PlayAudioSource(audioClip, volume, loop, position, false);
            }
            else if (audioPlayType == AudioPlayType.SkippableAudio && isPlaying) 
                return AudioPlayResult.fail;
            else
                return PlayAudioSource(audioClip, volume, loop, position, canFadeAudioSourceVolume);
        }

        private AudioPlayResult PlayAudioSource(AudioClip audioClip, float volume, bool loop, Vector3 position, bool canFadeAudioSourceVolume)
            => PlayAudioSource(ObjectPool.Get(), audioClip, volume, loop, position, canFadeAudioSourceVolume);

        private AudioPlayResult PlayAudioSource(AudioSource audioSource, AudioClip audioClip, float volume, bool loop, Vector3 position, bool canFadeAudioSourceVolume)
        {
            audioSource.name = audioClip.name;
            SetAudioSourceTimeScale(audioSource, SoundManager.TimeScale);
            audioSource.clip = audioClip;
            audioSource.loop = loop;
            audioSource.transform.position = position;
            audioSource.Play();
            var calculatedVolume = SoundManager.MasterVolume.Value * SoundManager.LocalVolumes[name].Value * volume;
            if (canFadeAudioSourceVolume)
                StartCoroutine(FadeAudioSourceVolumeCor(audioSource, calculatedVolume, callback: () => ReleaseAudioSource(audioSource, audioClip.length)));
            else
            {
                audioSource.volume = calculatedVolume;
                ReleaseAudioSource(audioSource, audioClip.length);
            }
            
            return new AudioPlayResult { isFail = false, isSuccess = true, audioClipLength = audioClip.length, audioSource = audioSource };
        }
        
        private IEnumerator FadeAudioSourceVolumeCor(AudioSource audioSource, float volume, float duration = SoundManager.FadeDuration, Action callback = null)
        {
            var startVolume = audioSource.volume;
            var startTime = Time.time;
            var endTime = startTime + duration;
            while (Time.time < endTime)
            {
                audioSource.volume = Mathf.Lerp(startVolume, volume, (Time.time - startTime) / (endTime - startTime));
                yield return null;
            }
            audioSource.volume = volume;
            callback?.Invoke();
        }

        public void SetAudioSourcesTimeScale(float timeScale) => audioSources.ForEach(t => SetAudioSourceTimeScale(t, timeScale));

        private void SetAudioSourceTimeScale(AudioSource audioSource, float timeScale) => audioSource.pitch = timeScale;
        
        public void StopAllAudioSources() => audioSources.ForEach(StopAudioSource);

        private void StopAudioSource(AudioSource audioSource)
        {
            audioSource.Stop();
            ReleaseAudioSource(audioSource);
        }
        
        public void ReleaseAudioSources() => audioSources.ForEach(t => ReleaseAudioSource(t));

        private void ReleaseAudioSource(AudioSource audioSource, float delay = 0)
        {
            StartCoroutine(Coroutine());
            IEnumerator Coroutine()
            {
                yield return new WaitForSeconds(delay);
                ObjectPool.Release(audioSource);
            }
        }l

        public void DisposeAudioSourcePool() => ObjectPool.Dispose();
    }
}