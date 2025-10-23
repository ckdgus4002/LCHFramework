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
        [ShowInInspector(nameof(useMinScale))] public float minScale = 1;
        public bool useMaxScale;
        [ShowInInspector(nameof(useMaxScale))] public float maxScale = 1;
        
        
        [NonSerialized] private float _prevScreenAspect;
        [NonSerialized] private float _prevMinScale;
        [NonSerialized] private float _prevMaxScale;
        
        
        
        protected override void OnReset()
        {
            _prevScreenAspect = float.MinValue;
            _prevMinScale = float.MinValue;
            _prevMaxScale = float.MinValue;
        }
        
        
        
        protected override bool ScaleIsChanged()
        {
            var screenAspect = (float)Screen.width / Screen.height;
            var result = !Mathf.Approximately(_prevScreenAspect, screenAspect) || (useMinScale && !Mathf.Approximately(_prevMinScale, minScale)) || (useMaxScale && !Mathf.Approximately(_prevMaxScale, maxScale));
            _prevScreenAspect = screenAspect;
            _prevMinScale = minScale;
            _prevMaxScale = maxScale;
            return result;
        }
        
        protected override void SetScale()
        {
            Tracker.Clear();
            Tracker.Add(this, RectTransform, DrivenTransformProperties.Scale);
            
            var screenAspect = (float)Screen.width / Screen.height;
            var scale = screenAspect / aspectRatio;
            RectTransform.localScale = Vector3Utility.New(useMinScale && useMaxScale ? Mathf.Clamp(scale, minScale, maxScale)
                    : useMinScale && !useMaxScale ? Mathf.Max(scale, minScale)
                    : !useMinScale && useMaxScale ? Mathf.Min(scale, maxScale)
                    : scale);
            
            LayoutRebuilder.ForceRebuildLayoutImmediate(RectTransform);
        }
    }
}