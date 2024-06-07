using UnityEngine;

namespace LCHFramework.Utilities
{
    public static class TouchScreenKeyboardUtility
    {
        public static float GetHeight()
        {
            /// TouchScreenKeyboard.area.height는 Android 에서 0을 반환합니다.
            /// https://docs.unity3d.com/ScriptReference/TouchScreenKeyboard-area.html
#if UNITY_ANDROID
            using var unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            using var rect = new AndroidJavaObject("android.graphics.Rect");
            var view = unityClass.GetStatic<AndroidJavaObject>("currentActivity").Get<AndroidJavaObject>("mUnityPlayer").Call<AndroidJavaObject>("getView");
            view.Call("getWindowVisibleDisplayFrame", rect);
            return Screen.height - rect.Call<int>("height");
#else
            return TouchScreenKeyboard.area.height;
#endif
        }
    }
}
