using LCHFramework.Extensions;
using UnityEngine;
using UnityEngine.Events;

namespace LCHFramework.Components
{
    public class MicrophoneController : MonoBehaviour
    {
        [SerializeField] private bool startOnEnable = true;
        [SerializeField] private bool stopOnDisable = true;
        public string deviceName;
        public bool loop;
        public int lengthSec = 60;
        public int sampleRate;
        public UnityEvent<AudioClip> onStartRecording;
        public UnityEvent<AudioClip> onStopRecording;
        
        
        public AudioClip RecordedAudioClipOrNull { get; private set; }
        
        
        public AudioClip RecordingAudioClipOrNull { get => recordingAudioClip; private set => recordingAudioClip = value; }
        private AudioClip recordingAudioClip;
        
        
        
#if UNITY_EDITOR
        protected virtual void Reset() => sampleRate = AudioSettings.outputSampleRate;
#endif
        protected virtual void OnEnable()
        {
            if (startOnEnable) StartRecording().Forget();
        }
        
        protected virtual void OnDisable()
        {
            if (stopOnDisable) StopRecording();
        }
        
        
        
        public virtual async Awaitable StartRecording(bool force = false)
        {
            if ((RecordingAudioClipOrNull == null || force) && await Application.RequestUserPermissionAsync(UserAuthorization.Microphone))
            {
                RecordingAudioClipOrNull = Microphone.Start(deviceName, loop, lengthSec, sampleRate);
            }
            
            onStartRecording?.Invoke(RecordingAudioClipOrNull);
        }
        
        public virtual void StopRecordingAndCreateAudioClip()
        {
            if (RecordingAudioClipOrNull == null) return;
            
            var position = Microphone.GetPosition(deviceName);
            Microphone.End(deviceName);
            if (position < 1) return;
            
            var data = new float[position * RecordingAudioClipOrNull.channels];
            RecordingAudioClipOrNull.GetData(data, 0);
            
            RecordedAudioClipOrNull = AudioClip.Create(RecordingAudioClipOrNull.name, position, RecordingAudioClipOrNull.channels, sampleRate, false);
            LCHMonoBehaviour.DestroyAndSetNull(ref recordingAudioClip);
            RecordedAudioClipOrNull.SetData(data, 0);
            
            onStopRecording?.Invoke(RecordedAudioClipOrNull);
        }
        
        public void StopRecording()
        {
            if (RecordingAudioClipOrNull == null) return;
            
            Microphone.End(deviceName);
        }
    }
}