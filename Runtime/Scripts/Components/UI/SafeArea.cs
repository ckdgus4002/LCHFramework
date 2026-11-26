using UnityEngine;
#if !UNITY_6000_6_OR_NEWER
using System;
using LCHFramework.Attributes;
using LCHFramework.Data;
using LCHFramework.Utilities;
using UnityEngine.UI;
#endif

namespace LCHFramework.Components.UI
{
#if UNITY_6000_6_OR_NEWER
    public class SafeArea : UnityEngine.UI.SafeArea
    {
        public Vector4 Padding
        {
            get
            {
                var result = new Vector4();
                var rectTransform = (RectTransform)transform;
                var width = LCHMonoBehaviour.GetWidth(transform);
                result.x = rectTransform.anchorMin.x * width; // left
                result.y = rectTransform.anchorMax.x * width; // right
                var height = LCHMonoBehaviour.GetHeight(transform);
                result.z = rectTransform.anchorMin.x * height; // top
                result.w = rectTransform.anchorMax.x * height; // bottom
                return result;
            }
        }
        
        
        
        protected override void Awake()
        {
            base.Awake();
            
            ReferenceOrientation = !LCHFramework.Instance.isPreferredLandscapeOrientation ? UnityEngine.ScreenOrientation.Portrait : UnityEngine.ScreenOrientation.LandscapeLeft;
        }
    }
#else
    public class SafeArea : DrivenRectTransformBehaviour
    {
        public SizeMode sizeModeX = SizeMode.SafeArea;
        [ShowInInspector(nameof(sizeModeX), ComparisonOperator.Equals, SizeMode.SafeArea)] public bool alwaysXZero = true;
        public SizeMode sizeModeY = SizeMode.SafeArea;
        [ShowInInspector(nameof(sizeModeY), ComparisonOperator.Equals, SizeMode.SafeArea)] public bool alwaysYZero = true;
        
        
        [NonSerialized] private Vector2 _prevPosition;
        [NonSerialized] private Vector2 _prevSize;
        
        
        
        protected override void OnReset()
        {
            _prevPosition = new Vector2(float.MinValue, float.MinValue);
            _prevSize = new Vector2(float.MinValue, float.MinValue);
        }
        
        
        
        protected override bool AllIsChanged()
        {
            if (RootCanvasOrNull == null || (RootCanvasOrNull.renderMode == RenderMode.WorldSpace && RootCanvasOrNull.worldCamera == null)) return false;
            
            var result = _prevPosition != UnityEngine.Screen.safeArea.position || _prevSize != UnityEngine.Screen.safeArea.size;
            _prevPosition = UnityEngine.Screen.safeArea.position;
            _prevSize = UnityEngine.Screen.safeArea.size;
            return result;
        }
        
        protected override void SetAll()
        {
            tracker.Clear();
            tracker.Add(this, RectTransform, DrivenTransformProperties.All);

            RectTransform.anchorMin = Vector2Utility.Half;
            RectTransform.anchorMax = Vector2Utility.Half;
            RectTransform.pivot = Vector2Utility.Half;
            RectTransform.rotation = Quaternion.identity;
            RectTransform.localScale = Vector3.one;
            var scaleFactor = CanvasUtility.GetScaleFactor(RootCanvasOrNull);
            if (scaleFactor == 0) return;
            
            var insetAverage = new Vector2(UnityEngine.Screen.width - UnityEngine.Screen.safeArea.width, UnityEngine.Screen.height - UnityEngine.Screen.safeArea.height) * 0.5f;
            var position = new Vector2(sizeModeX == SizeMode.SafeArea ? (UnityEngine.Screen.safeArea.position.x - insetAverage.x) / scaleFactor : 0, sizeModeY == SizeMode.SafeArea ? (UnityEngine.Screen.safeArea.position.y - insetAverage.y) / scaleFactor : 0);
            RectTransform.localPosition = new Vector2(
                float.IsInfinity(position.x) || (sizeModeX == SizeMode.SafeArea && alwaysXZero) ? 0 : position.x,
                float.IsInfinity(position.y) || (sizeModeY == SizeMode.SafeArea && alwaysYZero) ? 0 : position.y);
            var size = new Vector2(
                (sizeModeX == SizeMode.SafeArea ? UnityEngine.Screen.safeArea.size.x - (!alwaysXZero ? 0 : Mathf.Abs(position.x) * 2) : UnityEngine.Screen.width) / scaleFactor,
                (sizeModeY == SizeMode.SafeArea ? UnityEngine.Screen.safeArea.size.y - (!alwaysYZero ? 0 : Mathf.Abs(position.y) * 2) : UnityEngine.Screen.height) / scaleFactor);
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, float.IsInfinity(size.x) ? Width : size.x);
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, float.IsInfinity(size.y) ? Height : size.y);
            
            LayoutRebuilder.ForceRebuildLayoutImmediate(RectTransform);
        }
        
        
        
        public enum SizeMode
        {
            SafeArea = 1,
            Screen,
        }
    }
#endif
}
