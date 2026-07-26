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
        private ObjectPool<AudioSource> audioSourcePool;
        private readonly List<AudioSource> audioSources = new();
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
                audioSources.Add(audioSource);
                if (!audioSourceDisposables.ContainsKey(audioSource))
                {
                    audioSourceDisposables.Add(audioSource, new CompositeDisposable());
                    audioSourceDisposables[audioSource].Add(SoundManager.MasterVolume.Subscribe(masterVolume =>
                        audioSource.volume = GetPlayVolume() * masterVolume * SoundManager.LocalVolumes[name].Value));
                    audioSourceDisposables[audioSource].Add(SoundManager.LocalVolumes[name].Subscribe(localVolume =>
                        audioSource.volume = GetPlayVolume() * SoundManager.MasterVolume.Value * localVolume));

                    float GetPlayVolume()
                    {
                        var split = audioSource.name.Split('|');
                        return split.Length < 2 ? audioSource.volume : Convert.ToSingle(split[1]);
                    }
                }

                SetAudioSourceTimeScale(audioSource, SoundManager.TimeScale);
                audioSource.SetActive(true);

            }, ReleaseAudioSource, ClearAudioSource);
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
                
                isPlayingAudioSources.ForEach((t, i) => StartCoroutine(SoundManager.FadeAudioSourceVolumeCor(t, audioSource.volume, 0f, callback: () =>
                {
                    StartCoroutine(ReleaseAudioSourcePoolCor(audioSource, () => { if (isPlayingAudioSources.Length - 1 <= i)
                    {
                        PlayAudioSource(audioSource, audioClip, volume, loop, position, canFadeAudioSourceVolume);
                    }}));
                })));
                return new SoundPlayResult { isFail = false, isSuccess = true, audioClipLength = audioClip.length, audioSource = audioSource };
            }
            else if (audioPlayType == AudioPlayType.StoppableAudio && !canFadeAudioSourceVolume)
            {
                isPlayingAudioSources.ForEach(StopAudioSource);
                return PlayAudioSource(audioSourcePool.Get(), audioClip, volume, loop, position, canFadeAudioSourceVolume);
            }
            else if (audioPlayType == AudioPlayType.SkippableAudio && !isPlaying) 
                return PlayAudioSource(audioSourcePool.Get(), audioClip, volume, loop, position, canFadeAudioSourceVolume);
            else if (audioPlayType == AudioPlayType.NestableAudio)
                return PlayAudioSource(audioSourcePool.Get(), audioClip, volume, loop, position, canFadeAudioSourceVolume);
            else
                return SoundPlayResult.fail;
        }
        
        private SoundPlayResult PlayAudioSource(AudioSource audioSource, AudioClip audioClip, float volume, bool loop, Vector3 position, bool canFadeAudioSourceVolume)
        {
            audioSource.name = $"{audioClip.name}|{volume:F4}";
            audioSource.clip = audioClip;
            audioSource.loop = loop;
            audioSource.transform.position = position;
            audioSource.Play();
            volume *= SoundManager.MasterVolume.Value * SoundManager.LocalVolumes[name].Value;
            if (canFadeAudioSourceVolume)
                StartCoroutine(SoundManager.FadeAudioSourceVolumeCor(audioSource, 0f, volume, callback: () => StartCoroutine(ReleaseAudioSourcePoolCor(audioSource))));
            else
            {
                audioSource.volume = volume;
                StartCoroutine(ReleaseAudioSourcePoolCor(audioSource));
            }
            
            return new SoundPlayResult { isFail = false, isSuccess = true, audioClipLength = audioClip.length, audioSource = audioSource };
        }
        
        
        
        private IEnumerator ReleaseAudioSourcePoolCor(AudioSource audioSourceOrNull, Action callback = null)
        {
            Func<bool> predicate = () => audioSourceOrNull == null || (!audioSourceOrNull.isPlaying && audioSourceOrNull.timeSamples < 1);
            yield return new WaitUntil(predicate.Invoke);
            
            if (audioSourceOrNull != null) audioSourcePool.Release(audioSourceOrNull);
            else ReleaseAudioSource(audioSourceOrNull);
            callback?.Invoke();
        }
        
        public void ClearAudioSourcePool()
        {
            StopAllCoroutines();
            audioSourcePool.Clear();
            foreach (var audioSource in audioSources) ClearAudioSource(audioSource);
        }
        
        
        
        public void StopAllAudioSources() => audioSources.ForEach(StopAudioSource);
        
        public void StopAudioSource(AudioSource audioSource) => audioSource.Stop();
        
        public void PauseAllAudioSources() => audioSources.ForEach(PauseAudioSource);
        
        public void PauseAudioSource(AudioSource audioSource) => audioSource.Pause();
        
        public void UnPauseAllAudioSources() => audioSources.ForEach(UnPauseAudioSource);
        
        public void UnPauseAudioSource(AudioSource audioSource) => audioSource.UnPause();
        
        private void ReleaseAudioSource(AudioSource audioSource)
        {
            audioSources.Remove(audioSource);
            if (audioSourceDisposables.Remove(audioSource, out var disposables)) disposables.Clear();
            audioSource.SetActive(false);
        }
        
        private void ClearAudioSource(AudioSource audioSource)
        {
            audioSources.Remove(audioSource);
            if (audioSourceDisposables.Remove(audioSource, out var disposables)) disposables.Clear();
            Destroy(audioSource.gameObject);
        }
    }
}