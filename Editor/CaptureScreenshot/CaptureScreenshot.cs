using System;
using UnityEditor;
using UnityEngine;

namespace LCHFramework.Editor.CaptureScreenshot
{
    public static class CaptureScreenshot
    {
        [MenuItem("DevTool/LCHFramework/Capture Screenshot")]
        private static void Capture() => ScreenCapture.CaptureScreenshot($"{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.png");
    }
}