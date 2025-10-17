using UnityEngine;

namespace LCHFramework.Utilities
{
    public static class TouchScreenKeyboardUtility
    {
        /// <remarks>
        /// TouchScreenKeyboard.area.height는 Android 에서 0을 반환합니다.
        /// https://docs.unity3d.com/ScriptReference/TouchScreenKeyboard-area.html
        /// </remarks>
        public static float GetHeight()
        {
#if UNITY_ANDROID
            using var window = Application.CurrentActivity.Call<AndroidJavaObject>("getWindow");
            using var decorView = window.Call<AndroidJavaObject>("getDecorView");
            using var rect = new AndroidJavaObject("android.graphics.Rect");
            decorView.Call("getWindowVisibleDisplayFrame", rect);
            return Screen.height - rect.Call<int>("height");
#else
            return TouchScreenKeyboard.area.height;
#endif
        }
    }
}
