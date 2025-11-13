using System;
using LCHFramework.Attributes;
using LCHFramework.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace LCHFramework.Components.UI
{
    public class ScaleControllerByScreenAspect : DrivenRectTransformBehaviour
    {
        public float aspectRatio = 1;
        public bool useMinScale;
        [ShowInInspector(nameof(useMinScale))] public Vector3 minScale = Vector3.one;
        public bool useMaxScale;
        [ShowInInspector(nameof(useMaxScale))] public Vector3 maxScale = Vector3.one;
        
        
        [NonSerialized] private float _prevScreenAspectRatio;
        [NonSerialized] private Vector3 _prevMinScale;
        [NonSerialized] private Vector3 _prevMaxScale;
        
        
        
        protected override void OnReset()
        {
            _prevScreenAspectRatio = float.MinValue;
            _prevMinScale = Vector3Utility.New(float.MinValue);
            _prevMaxScale = Vector3Utility.New(float.MinValue);
        }
        
        
        
        protected override bool ScaleIsChanged()
        {
            var screenAspectRatio = Screen.AspectRatio;
            var result = !Mathf.Approximately(_prevScreenAspectRatio, screenAspectRatio) || (useMinScale && _prevMinScale != minScale) || (useMaxScale && _prevMaxScale != maxScale);
            _prevScreenAspectRatio = screenAspectRatio;
            _prevMinScale = minScale;
            _prevMaxScale = maxScale;
            return result;
        }
        
        protected override void SetScale()
        {
            Tracker.Clear();
            Tracker.Add(this, RectTransform, DrivenTransformProperties.Scale);
            
            var scale = Screen.AspectRatio / aspectRatio;
            RectTransform.localScale = useMinScale switch
            {
                false when !useMaxScale => new Vector3(scale, scale, scale),
                false when useMaxScale => new Vector3(Mathf.Min(scale, maxScale.x), Mathf.Min(scale, maxScale.y), Mathf.Min(scale, maxScale.z)),
                true when !useMaxScale => new Vector3(Mathf.Max(scale, minScale.x), Mathf.Max(scale, minScale.y), Mathf.Max(scale, minScale.z)),
                _ => new Vector3(Mathf.Clamp(scale, minScale.x, maxScale.x), Mathf.Clamp(scale, minScale.y, maxScale.y), Mathf.Clamp(scale, minScale.z, maxScale.z))
            };
            
            LayoutRebuilder.ForceRebuildLayoutImmediate(RectTransform);
        }
    }
}