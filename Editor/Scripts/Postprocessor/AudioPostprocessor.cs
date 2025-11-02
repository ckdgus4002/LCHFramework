using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace LCHFramework.Editor
{
    public class AudioPostprocessor : AssetPostprocessor
    {
        private void OnPostprocessAudio(AudioClip audioClip)
        {
            var audioImporter = (AudioImporter)assetImporter;
            if (AudioPostprocessorExceptTable.Instances.Any(t => t.IsExclude(audioImporter.assetPath))) return;
            
            const bool isMobile = true;
            var isBgm = audioImporter.assetPath.Contains("bgm", StringComparison.OrdinalIgnoreCase);
            var isSfx = audioImporter.assetPath.Contains("sfx", StringComparison.OrdinalIgnoreCase);
            var isShortSfx = isSfx && audioClip.length < 3;
            var isLongSfx = isSfx && !isShortSfx;
            audioImporter.forceToMono = isMobile;
            var serializedObject = new SerializedObject(audioImporter);
            serializedObject.FindProperty("m_Normalize").boolValue = true;
            audioImporter.loadInBackground = false;
            audioImporter.ambisonic = false;
            
            var defaultSampleSettings = audioImporter.defaultSampleSettings;
            // defaultSampleSettings.loadType = isBgm ? AudioClipLoadType.Streaming : isLongSfx ? AudioClipLoadType.CompressedInMemory : AudioClipLoadType.DecompressOnLoad;
            defaultSampleSettings.loadType = AudioClipLoadType.DecompressOnLoad;
            var preloadAudioData = true;
            defaultSampleSettings.preloadAudioData = preloadAudioData;
            serializedObject.ApplyModifiedProperties();
            // defaultSampleSettings.compressionFormat = isNoiseSfx ? AudioCompressionFormat.ADPCM : isShortSfx ? AudioCompressionFormat.PCM : AudioCompressionFormat.Vorbis;
            defaultSampleSettings.compressionFormat = AudioCompressionFormat.Vorbis;
            defaultSampleSettings.quality = 1f;
            defaultSampleSettings.sampleRateSetting = AudioSampleRateSetting.PreserveSampleRate;
            audioImporter.defaultSampleSettings = defaultSampleSettings;
            
            foreach (var platformGroup in Application.PlatformGroups) audioImporter.ClearSampleSettingOverride($"{platformGroup}");
            
            Debug.Log($"{nameof(OnPostprocessAudio)}: {assetPath}");
        }
    }   
}