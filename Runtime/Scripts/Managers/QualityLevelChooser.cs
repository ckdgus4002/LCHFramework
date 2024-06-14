using LCHFramework.Extensions;
using UnityEngine;
using Debug = LCHFramework.Utilities.Debug;

namespace LCHFramework.Managers
{
    /// <remarks>
    /// Bootcamp/GameQualitySettings.js/AutoChooseQualityLevel()
    /// </remarks>
    public static class QualitySettingsChooser
    {
        public static int ChooseQualityLevel()
        {
            var shaderLevel = SystemInfo.graphicsShaderLevel;
            int fillrate;
            var vram = SystemInfo.graphicsMemorySize;
            var cpus = SystemInfo.processorCount;
#if !UNITY_5_3_OR_NEWER
            if ((fillrate = SystemInfo.graphicsPixelFillrate) < 0)
#endif
            {
                if (shaderLevel < 10)
                    fillrate = 1000;
                else if (shaderLevel < 20)
                    fillrate = 1300;
                else if (shaderLevel < 30)
                    fillrate = 2000;
                else
                    fillrate = 3000;

                if (6 <= cpus)
                    fillrate *= 3;
                else if (3 <= cpus)
                    fillrate *= 2;

                if (512 <= vram)
                    fillrate *= 2;
                else if (vram <= 128)
                    fillrate /= 2;
            }

            var resx = Screen.width;
            var resy = Screen.height;
            var fillneed = (resx * resy + 400 * 300) * (30.0f / 1000000.0f);
            var levelmult = new[] { 5.0f, 30.0f, 80.0f, 130.0f, 200.0f, 320.0f };

            var level = 0;
            var fantasticIndex = QualitySettings.names.IndexOf(t => t is "Fantastic" or "Ultra");

            while (level < fantasticIndex && fillneed * levelmult[level + 1] < fillrate)
                ++level;
            
            Debug.Log($"{resx}x{resy} need {fillneed} has {fillrate} = {level} level");
            
            return level;
        }
    }
}
