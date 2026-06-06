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
        public int lengthSec = 15;
        public UnityEvent<AudioClip> onStartRecording;
        public UnityEvent<AudioClip> onStopRecordingAndCreateAudioClip;
        
        
        public AudioClip RecordingAudioClipOrNull { get ; private set; }
        public AudioClip RecordedAudioClipOrNull { get; private set; }
        
        
        protected virtual int SampleRate => AudioSettings.outputSampleRate;
        
        
        
        protected virtual void OnEnable()
        {
            if (startOnEnable) StartRecording().Forget();
        }
        
        protected virtual void OnDisable()
        {
            if (stopOnDisable) StopRecording();
        }
        
        
        
        public virtual async Awaitable<AudioClip> StartRecording(bool force = false)
        {
            if ((RecordingAudioClipOrNull == null || force) && await Application.RequestUserPermissionAsync(UserAuthorization.Microphone))
            {
                if (RecordingAudioClipOrNull != null) Destroy(RecordingAudioClipOrNull);
                RecordingAudioClipOrNull = Microphone.Start(deviceName, loop, lengthSec, SampleRate);
            }
            
            onStartRecording?.Invoke(RecordingAudioClipOrNull);
            return RecordingAudioClipOrNull;
        }
        
        public virtual AudioClip StopRecordingAndCreateAudioClip()
        {
            if (RecordingAudioClipOrNull == null) return null;
            
            var position = Microphone.GetPosition(deviceName);
            Microphone.End(deviceName);
            if (position < 1) return null;
            
            var data = new float[position * RecordingAudioClipOrNull.channels];
            RecordingAudioClipOrNull.GetData(data, 0);
            
            RecordedAudioClipOrNull = AudioClip.Create(RecordingAudioClipOrNull.name, position, RecordingAudioClipOrNull.channels, SampleRate, false);
            RecordedAudioClipOrNull.SetData(data, 0);
            
            onStopRecordingAndCreateAudioClip?.Invoke(RecordedAudioClipOrNull);
            return RecordedAudioClipOrNull;
        }
        
        public void StopRecording()
        {
            if (RecordingAudioClipOrNull == null) return;
            
            Microphone.End(deviceName);
        }
    }
}