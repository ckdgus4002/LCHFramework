using System;
using UnityEditor;
using UnityEngine;

namespace LCHFramework.Editor
{
    public static class CaptureScreenshot
    {
        [MenuItem(LCHFramework.MenuItemRootPath + "/Capture Screenshot")]
        private static void Capture() => ScreenCapture.CaptureScreenshot($"{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.png");
    }
}