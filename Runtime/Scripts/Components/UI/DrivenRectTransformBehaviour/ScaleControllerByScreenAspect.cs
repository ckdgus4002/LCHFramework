using System;
using LCHFramework.Utilities;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LCHFramework.Components.UI
{
    public class ScaleControllerByScreenAspect : LayoutSelfController
    {
        [SerializeField] private float minAspect;
        [SerializeField] private float maxAspect;
        
        
        [NonSerialized] private float screenAspect;
        [NonSerialized] private float _prevScreenAspect;
        
        
        
        private void Reset() => minAspect = maxAspect = (float)Screen.width / Screen.height;
        
        
        
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

            // origin: 1.7778 // 1920*1080, 960*540
            // scren : 2.3703 // 2560*1080, 3413*1440
            var rectTransformAspect = RectTransformOrNull.rect.width / RectTransformOrNull.rect.height;
            RectTransformOrNull.localScale = Vector3Utility.New(Mathf.Clamp(minAspect, maxAspect, screenAspect / rectTransformAspect));
            
            if (GetComponent<UIBehaviour>() != null) LayoutRebuilder.MarkLayoutForRebuild(RectTransformOrNull);
        }
    }
}