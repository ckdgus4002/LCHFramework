using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Debug = LCHFramework.Utilities.Debug;

namespace LCHFramework.Editor
{
    public class AudioPostprocessor : AssetPostprocessor
    {
        private readonly IEnumerable<BuildTargetGroup> _platformGroups = new[] { BuildTargetGroup.Standalone, BuildTargetGroup.Android, BuildTargetGroup.iOS, };
        
        
        
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
#if !UNITY_2022_2_OR_NEWER
            serializedObject.FindProperty("m_PreloadAudioData").boolValue = preloadAudioData;
#else
            defaultSampleSettings.preloadAudioData = preloadAudioData;
#endif
            serializedObject.ApplyModifiedProperties();
            // defaultSampleSettings.compressionFormat = isNoiseSfx ? AudioCompressionFormat.ADPCM : isShortSfx ? AudioCompressionFormat.PCM : AudioCompressionFormat.Vorbis;
            defaultSampleSettings.compressionFormat = AudioCompressionFormat.Vorbis;
            defaultSampleSettings.quality = 1f;
            defaultSampleSettings.sampleRateSetting = AudioSampleRateSetting.PreserveSampleRate;
            audioImporter.defaultSampleSettings = defaultSampleSettings;
            
            foreach (var platformGroup in _platformGroups) audioImporter.ClearSampleSettingOverride($"{platformGroup}");
            
            Debug.Log($"OnPostprocessAudio: {assetPath}");
        }
    }   
}