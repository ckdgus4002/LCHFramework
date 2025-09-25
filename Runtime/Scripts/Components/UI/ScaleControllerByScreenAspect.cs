using System;
using LCHFramework.Utilities;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LCHFramework.Components.UI
{
    public class ScaleControllerByScreenAspect : DrivenRectTransformBehaviour
    {
        [SerializeField] private float minScale = 1;
        
        
        [NonSerialized] private float _prevScreenAspect;
        [NonSerialized] private float _prevMinScale;
        
        
        
        protected override void OnReset()
        {
            _prevScreenAspect = float.MinValue;
            _prevMinScale = float.MinValue;
        }
        
        
        
        protected override bool ScaleIsChanged()
        {
            var screenAspect = (float)Screen.width / Screen.height;
            var result = !Mathf.Approximately(_prevScreenAspect, screenAspect) || !Mathf.Approximately(_prevMinScale, minScale);
            _prevScreenAspect = screenAspect;
            _prevMinScale = minScale;
            return result;
        }
        
        protected override void SetScale()
        {
            Tracker.Clear();
            Tracker.Add(this, RectTransform, DrivenTransformProperties.Scale);
            
            var screenAspect = (float)Screen.width / Screen.height;
            var rectTransformAspect = RectTransform.rect.width / RectTransform.rect.height;
            RectTransform.localScale = Vector3Utility.New(Mathf.Max(screenAspect / rectTransformAspect, minScale));
            
            if (GetComponent<UIBehaviour>() != null) LayoutRebuilder.MarkLayoutForRebuild(RectTransform);
        }
    }
}