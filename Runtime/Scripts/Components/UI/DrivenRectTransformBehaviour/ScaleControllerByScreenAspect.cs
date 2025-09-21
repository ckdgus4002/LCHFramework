using System;
using LCHFramework.Utilities;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LCHFramework.Components.UI
{
    public class ScaleControllerByScreenAspect : LayoutSelfController
    {
        [SerializeField] private float minScale = 1;
        
        
        [NonSerialized] private float screenAspect;
        [NonSerialized] private float _prevScreenAspect;
        
        
        
        private void OnValidate() => _prevScreenAspect = 0; 
        
        
        
        protected override bool ScaleXIsChanged() => ScaleIsChanged();
        
        protected override void SetScaleX() => SetScale();
        
        protected override bool ScaleYIsChanged() => ScaleIsChanged();
        
        protected override void SetScaleY() => SetScale();
        
        
        
        private bool ScaleIsChanged()
        {
            var result = !Mathf.Approximately(_prevScreenAspect, screenAspect = (float)Screen.width / Screen.height);
            _prevScreenAspect = screenAspect;
            return result;
        }
        
        private void SetScale()
        {
            Tracker.Clear();
            Tracker.Add(this, RectTransformOrNull, DrivenTransformProperties.Scale);
            
            var rectTransformAspect = RectTransformOrNull.rect.width / RectTransformOrNull.rect.height;
            RectTransformOrNull.localScale = Vector3Utility.New(Mathf.Max(screenAspect / rectTransformAspect, minScale));
            
            if (GetComponent<UIBehaviour>() != null) LayoutRebuilder.MarkLayoutForRebuild(RectTransformOrNull);
        }
    }
}