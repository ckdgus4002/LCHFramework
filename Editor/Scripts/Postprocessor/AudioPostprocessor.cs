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
            if (AssetPostprocessorExceptTable.GlobalExceptAssetPathPrefix.Any(t => t.IsExclude(assetPath))) return;
            if (AssetPostprocessorExceptTable.Instances.Any(t => t.IsExclude(assetPath))) return;
            
            const bool isMobile = true;
            var isBgm = assetPath.Contains("bgm", StringComparison.OrdinalIgnoreCase);
            var isSfx = assetPath.Contains("sfx", StringComparison.OrdinalIgnoreCase);
            var isShortSfx = isSfx && audioClip.length < 3;
            var isLongSfx = isSfx && !isShortSfx;
            var audioImporter = (AudioImporter)assetImporter;
            audioImporter.forceToMono = isMobile;
            var serializedObject = new SerializedObject(audioImporter);
            serializedObject.FindProperty("m_Normalize").boolValue = true;
            serializedObject.ApplyModifiedProperties();
            audioImporter.loadInBackground = false;
            audioImporter.ambisonic = false;
            
            var defaultSampleSettings = audioImporter.defaultSampleSettings;
            // defaultSampleSettings.loadType = isBgm ? AudioClipLoadType.Streaming : isLongSfx ? AudioClipLoadType.CompressedInMemory : AudioClipLoadType.DecompressOnLoad;
            defaultSampleSettings.loadType = AudioClipLoadType.DecompressOnLoad;
            var preloadAudioData = true;
            defaultSampleSettings.preloadAudioData = preloadAudioData;
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