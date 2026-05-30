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
        
        
        public AudioClip RecordedClipOrNull { get; private set; }
        
        
        public AudioClip RecordingClipOrNull { get => _recordingClip; private set => _recordingClip = value; }
        private AudioClip _recordingClip;
        
        
        
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
            if ((RecordingClipOrNull == null || force) && await Application.RequestUserPermissionAsync(UserAuthorization.Microphone))
            {
                RecordingClipOrNull = Microphone.Start(deviceName, loop, lengthSec, sampleRate);
            }
            
            onStartRecording?.Invoke(RecordingClipOrNull);
        }
        
        public virtual void StopRecording()
        {
            if (RecordingClipOrNull == null) return;
            
            var position = Microphone.GetPosition(deviceName);
            Microphone.End(deviceName);
            if (position < 1) return;
            
            var data = new float[position * RecordingClipOrNull.channels];
            RecordingClipOrNull.GetData(data, 0);

            RecordedClipOrNull = AudioClip.Create(RecordingClipOrNull.name, position, RecordingClipOrNull.channels, sampleRate, false);
            LCHMonoBehaviour.DestroyAndSetNull(ref _recordingClip);
            RecordedClipOrNull.SetData(data, 0);
            
            onStopRecording?.Invoke(RecordedClipOrNull);
        }
    }
}