using LCHFramework.Extensions;
using LCHFramework.Utils;
using UnityEngine;
using UnityEngine.Profiling;

namespace LCHFramework.Components
{
    public class FramePerSecond : MonoBehaviour
    {
	    [SerializeField] private Color textColor;
	    
	    
	    
        private void OnGUI()
        {
	        var rect = new Rect(Vector2.zero, new Vector2(Screen.width, Screen.height));
	        var guiLabel = $"FPS: {(int)Time.deltaTime.Reverse()}"
	                       + $"\nMem: {FileUtil.ToHumanReadableFileSize(Profiler.GetTotalAllocatedMemoryLong(), 2)}"
	                       + $"\n!Mem: {FileUtil.ToHumanReadableFileSize(Profiler.GetTotalUnusedReservedMemoryLong(), 2)}"
	                       ;
	        var guiStyle = new GUIStyle { fontSize = 50, alignment = TextAnchor.UpperLeft, normal = { textColor = textColor } };
	        GUI.Label(rect, guiLabel, guiStyle);
        }
    }
}