using System.Linq;
using LCHFramework.Extensions;
using LCHFramework.Utilities;
using UnityEngine;
using UnityEngine.Events;

namespace LCHFramework.Components
{
    public class MicrophoneController : MonoBehaviour
    {
        [SerializeField] private bool startOnEnable = true;
        [SerializeField] private bool stopOnDisable = true;
        public bool loop;
        public int lengthSec = 15;
        public float targetPeak = 1f;
        public UnityEvent<AudioClip> onStartRecording;
        public UnityEvent<AudioClip> onStopRecordingAndCreateAudioClip;
        
        
        public AudioClip RecordingAudioClipOrNull { get ; private set; }
        public AudioClip RecordedAudioClipOrNull { get; private set; }


        protected virtual string DeviceName => Microphone.devices.Length < 1 ? string.Empty : Microphone.devices[0];
        protected virtual int SampleRate => AudioSettings.outputSampleRate;
        
        
        
        protected virtual void OnEnable()
        {
            if (startOnEnable) StartRecording().Forget();
        }
        
        protected virtual void OnDisable()
        {
            if (stopOnDisable) StopRecording();
        }
        
        
        
        public virtual async Awaitable<AudioClip> StartRecording()
        {
            if (await Application.RequestUserPermissionAsync(UserAuthorization.Microphone))
            {
                if (RecordingAudioClipOrNull != null) Destroy(RecordingAudioClipOrNull);
                
                RecordingAudioClipOrNull = Microphone.Start(DeviceName, loop, lengthSec, SampleRate);
                await AwaitableUtility.WaitUntil(() => 0 < Microphone.GetPosition(DeviceName));
            }
            
            onStartRecording?.Invoke(RecordingAudioClipOrNull);
            return RecordingAudioClipOrNull;
        }
        
        public virtual AudioClip StopRecordingAndCreateAudioClip()
        {
            if (RecordingAudioClipOrNull == null) return null;
            
            var position = Microphone.GetPosition(DeviceName);
            Microphone.End(DeviceName);
            if (position < 1) return null;
            
            var samples = new float[position * RecordingAudioClipOrNull.channels];
            RecordingAudioClipOrNull.GetData(samples, 0);
            Amplify(samples);
            
            RecordedAudioClipOrNull = AudioClip.Create(RecordingAudioClipOrNull.name, position, RecordingAudioClipOrNull.channels, RecordingAudioClipOrNull.frequency, false);
            RecordedAudioClipOrNull.SetData(samples, 0);
            
            onStopRecordingAndCreateAudioClip?.Invoke(RecordedAudioClipOrNull);
            return RecordedAudioClipOrNull;
        }
        
        private void Amplify(float[] data)
        {
            var peak = data.Aggregate(0f, (current, sample) => Mathf.Max(current, Mathf.Abs(sample)));
            if (peak < 0.0001f) return; // 거의 무음이면 그대로

            var scale = peak / targetPeak;
            for (var i = 0; i < data.Length; i++) data[i] = Mathf.Clamp(data[i] * scale, -1f, 1f);
        }
        
        public void StopRecording()
        {
            if (RecordingAudioClipOrNull == null) return;
            
            Microphone.End(DeviceName);
        }
    }
}