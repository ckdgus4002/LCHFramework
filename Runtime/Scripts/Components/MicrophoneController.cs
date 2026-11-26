using System;
using System.Linq;
using LCHFramework.Extensions;
using LCHFramework.Utilities;
using UnityEngine;
using UnityEngine.Events;

namespace LCHFramework.Components
{
    public class MicrophoneController : MonoBehaviour
    {
        private const float TargetPeak = 1f;
        protected const UserAuthorization UserAuthorizationMicrophone = UserAuthorization.Microphone;
        
        
        
        [SerializeField] private bool startOnEnable;
        [SerializeField] private bool stopOnDisable = true;
        public bool loop;
        public int lengthSec = 15;
        public UnityEvent<AudioClip> onStartRecording;
        public UnityEvent<AudioClip> onStopRecordingAndCreateAudioClip;
        
        
        private AudioClip recordingAudioClipOrNull;
        private string recordingDeviceName = string.Empty;
        
        
        public AudioClip RecordedAudioClipOrNull { get; private set; }
        
        
        
        protected virtual int SampleRate => AudioSettings.outputSampleRate;
        
        public AudioClip RecordingAudioClipOrNull => recordingAudioClipOrNull; 
        
        
        
        protected virtual void OnEnable()
        {
            if (startOnEnable) StartRecording().Forget();
        }
        
        protected virtual void OnDisable()
        {
            if (stopOnDisable) StopRecording();
        }
        
        
        
        public virtual async Awaitable<AudioClip> StartRecording(Func<Awaitable<Application.RequestUserPermissionResult>> onRequestPermissionDeniedAndDontAskAgainOrNull = null)
        {
            var requestUserPermissionResult = await Application.RequestUserPermissionAsync(UserAuthorizationMicrophone);
            if (onRequestPermissionDeniedAndDontAskAgainOrNull != null && requestUserPermissionResult == Application.RequestUserPermissionResult.DeniedAndDontAskAgain) requestUserPermissionResult = await onRequestPermissionDeniedAndDontAskAgainOrNull.Invoke();
            
            if (Application.RequestUserPermissionResult.Granted <= requestUserPermissionResult)
            {
                if (recordingAudioClipOrNull != null) Destroy(recordingAudioClipOrNull);
                recordingAudioClipOrNull = Microphone.Start(recordingDeviceName = GetRecordingDeviceName(), loop, lengthSec, SampleRate);
                if (recordingAudioClipOrNull != null) await AwaitableUtility.WaitUntil(() => 0 < Microphone.GetPosition(recordingDeviceName));
            }
            
            onStartRecording?.Invoke(recordingAudioClipOrNull);
            return recordingAudioClipOrNull;
        }
        
        protected virtual string GetRecordingDeviceName() => UnityEngine.Application.platform == RuntimePlatform.Android && Microphone.devices.TryFirstOrDefault(t => t.Contains("Bluetooth", StringComparison.OrdinalIgnoreCase), out var bluetooth) ? bluetooth
            : string.Empty;
        
        public virtual AudioClip StopRecordingAndCreateAudioClip()
        {
            if (recordingAudioClipOrNull == null) return null;
            
            var position = Microphone.GetPosition(recordingDeviceName);
            Microphone.End(recordingDeviceName);
            if (position < 1) return null;
            
            var samples = new float[position * recordingAudioClipOrNull.channels];
            recordingAudioClipOrNull.GetData(samples, 0);
            Amplify(samples);
            
            RecordedAudioClipOrNull = AudioClip.Create(recordingAudioClipOrNull.name, position, recordingAudioClipOrNull.channels, recordingAudioClipOrNull.frequency, false);
            ObjectUtility.DestroyAndSetNull(ref recordingAudioClipOrNull);
            RecordedAudioClipOrNull.SetData(samples, 0);
            
            onStopRecordingAndCreateAudioClip?.Invoke(RecordedAudioClipOrNull);
            return RecordedAudioClipOrNull;
        }
        
        private void Amplify(float[] data)
        {
            var peak = data.Aggregate(0f, (current, sample) => Mathf.Max(current, Mathf.Abs(sample)));
            if (peak < 0.0001f) return; // 거의 무음이면 그대로

            var scale = TargetPeak / peak;
            for (var i = 0; i < data.Length; i++) data[i] *= scale;
        }
        
        public void StopRecording()
        {
            if (recordingAudioClipOrNull == null) return;
            
            Microphone.End(recordingDeviceName);
        }
    }
}