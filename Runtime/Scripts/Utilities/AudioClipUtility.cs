using UnityEngine;

namespace LCHFramework.Utilities
{
    public static class AudioClipUtility
    {
        public static AudioClip Default => _default == null ? _default = AudioClip.Create("NewClip", 1, 1, AudioSettings.outputSampleRate, false) : _default;
        private static AudioClip _default;
    }
}