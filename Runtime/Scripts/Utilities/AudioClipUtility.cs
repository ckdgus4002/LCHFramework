using UnityEngine;

namespace LCHFramework.Utilities
{
    public static class AudioClipUtility
    {
        public static AudioClip Default
        {
            get
            {
                var outputSampleRate = AudioSettings.outputSampleRate;
                return AudioClip.Create("NewClip", outputSampleRate, 1, outputSampleRate, false);
            }
        }
    }
}