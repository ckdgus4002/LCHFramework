using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LCHFramework.Extensions;
using UniRx;
using UnityEngine;
using UnityEngine.Pool;

namespace LCHFramework.Managers
{
    public class AudioSourcePool : MonoBehaviour
    {
        private readonly List<AudioSource> audioSources = new();
        private ObjectPool<AudioSource> audioSourcePool;
        private readonly Dictionary<AudioSource, CompositeDisposable> audioSourceDisposables = new();
        
        
        public bool IsPlaying(IEnumerable<AudioSource> isPlayingAudioSources = null) => !(isPlayingAudioSources ?? PlayingAudioSources).IsEmpty();
        
        public IEnumerable<AudioSource> PlayingAudioSources => audioSources.Where(t => t != null && t.isPlaying);
        
        
        
        private void Awake()
        {
            audioSourcePool = new ObjectPool<AudioSource>(() => 
            {
                var audioSource = new GameObject().AddComponent<AudioSource>(); 
                audioSource.transform.SetParent(transform);
                return audioSource;
                
            }, audioSource =>
            {
                if (!audioSources.Contains(audioSource)) 
                    audioSources.Add(audioSource);
                if (!audioSourceDisposables.ContainsKey(audioSource))
                {
                    audioSourceDisposables.Add(audioSource, new CompositeDisposable());
                    audioSourceDisposables[audioSource].Add(SoundManager.MasterVolume.Subscribe(masterVolume => audioSource.volume *= masterVolume * SoundManager.LocalVolumes[name].Value));
                    audioSourceDisposables[audioSource].Add(SoundManager.LocalVolumes[name].Subscribe(localVolume => audioSource.volume *= SoundManager.MasterVolume.Value * localVolume));
                }
                SetAudioSourceTimeScale(audioSource, SoundManager.TimeScale);
                audioSource.SetActive(true);
                
            }, audioSource =>
            {
                audioSources.Remove(audioSource);
                audioSourceDisposables[audioSource].Dispose();
                if (audioSource == null) return;
                audioSource.SetActive(false);
                
            }, audioSource =>
            {
                audioSources.Remove(audioSource);
                if (audioSource == null) return;
                Destroy(audioSource.gameObject);
            });
        }
        
        
        
        public void SetAudioSourcesTimeScale(float timeScale) => audioSources.ForEach(t => SetAudioSourceTimeScale(t, timeScale));
        
        private void SetAudioSourceTimeScale(AudioSource audioSource, float timeScale) { if (audioSource != null) audioSource.pitch = timeScale; }

        public SoundPlayResult Play(AudioClip audioClip, float volume, bool loop, Vector3 position, AudioPlayType audioPlayType)
        {
            if (audioClip == null) return SoundPlayResult.fail;
            
            var isPlayingAudioSources = PlayingAudioSources.ToArray();
            var isPlaying = IsPlaying(isPlayingAudioSources);
            var canFadeAudioSourceVolume = name == SoundManager.Bgm;
            if (audioPlayType == AudioPlayType.StoppableAudio && canFadeAudioSourceVolume)
            {
                var audioSource = audioSourcePool.Get();
                if (isPlayingAudioSources.IsEmpty()) return PlayAudioSource(audioSource, audioClip, volume, loop, position, canFadeAudioSourceVolume);

                isPlayingAudioSources.ForEach((t, i) => StartCoroutine(FadeAudioSourceVolumeCor(t, 0, callback: i < isPlayingAudioSources.Length - 1 ? null : () => 
                {
                    isPlayingAudioSources.ForEach(StopAudioSource);
                    PlayAudioSource(audioSource, audioClip, volume, loop, position, canFadeAudioSourceVolume);
                })));
                return new SoundPlayResult { isFail = false, isSuccess = true, audioClipLength = audioClip.length, audioSource = audioSource };
            }
            else if (audioPlayType == AudioPlayType.StoppableAudio && !canFadeAudioSourceVolume)
            {
                isPlayingAudioSources.ForEach(StopAudioSource);
                return PlayAudioSource(audioClip, volume, loop, position, canFadeAudioSourceVolume);
            }
            else if (audioPlayType == AudioPlayType.SkippableAudio && !isPlaying) 
                return PlayAudioSource(audioClip, volume, loop, position, canFadeAudioSourceVolume);
            else if (audioPlayType == AudioPlayType.NestableAudio)
                return PlayAudioSource(audioClip, volume, loop, position, canFadeAudioSourceVolume);
            else
                return SoundPlayResult.fail;
        }

        private SoundPlayResult PlayAudioSource(AudioClip audioClip, float volume, bool loop, Vector3 position, bool canFadeAudioSourceVolume)
            => PlayAudioSource(audioSourcePool.Get(), audioClip, volume, loop, position, canFadeAudioSourceVolume);

        private SoundPlayResult PlayAudioSource(AudioSource audioSource, AudioClip audioClip, float volume, bool loop, Vector3 position, bool canFadeAudioSourceVolume)
        {
            audioSource.name = audioClip.name;
            audioSource.clip = audioClip;
            audioSource.loop = loop;
            audioSource.transform.position = position;
            audioSource.Play();
            volume *= SoundManager.MasterVolume.Value * SoundManager.LocalVolumes[name].Value;
            if (canFadeAudioSourceVolume)
                StartCoroutine(FadeAudioSourceVolumeCor(audioSource, volume, callback: () => StartCoroutine(ReleaseAudioSourceCor(audioSource, ReleaseAudioSourcePredicate))));
            else
            {
                audioSource.volume = volume;
                StartCoroutine(ReleaseAudioSourceCor(audioSource, ReleaseAudioSourcePredicate));
            }
            
            return new SoundPlayResult { isFail = false, isSuccess = true, audioClipLength = audioClip.length, audioSource = audioSource };
        }

        private IEnumerator FadeAudioSourceVolumeCor(AudioSource audioSource, float volume, float duration = SoundManager.FadeDuration, Action callback = null)
        {
            var startVolume = audioSource.volume;
            var startTime = Time.time;
            var endTime = startTime + duration;
            while (audioSource != null && Time.time < endTime)
            {
                audioSource.volume = Mathf.Lerp(startVolume, volume, (Time.time - startTime) / (endTime - startTime));
                yield return null;
            }
            if (audioSource != null) audioSource.volume = volume;
            callback?.Invoke();
        }
        
        private IEnumerator ReleaseAudioSourceCor(AudioSource audioSource, Func<AudioSource, bool> predicate)
        {
            yield return new WaitUntil(() => predicate.Invoke(audioSource));
            audioSourcePool.Release(audioSource);
        }

        private bool ReleaseAudioSourcePredicate(AudioSource audioSource) => audioSource == null || !audioSource.isPlaying && audioSource.timeSamples < 1;
        
        
        
        public void StopAllAudioSources() => audioSources.ForEach(StopAudioSource);
        
        private void StopAudioSource(AudioSource audioSource) => audioSource.Stop();

        public void ClearAudioSourcePool() => audioSourcePool.Clear();
    }
}