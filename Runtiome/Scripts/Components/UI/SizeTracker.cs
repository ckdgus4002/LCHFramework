using LCHFramework.Extensions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LCHFramework.Components.UI
{
    public class SizeTracker : LayoutSelfController
    {
        public RectTransform widthTarget;
        public float leftPadding;
        public  float rightPadding;
        public RectTransform heightTarget;
        public  float topPadding;
        public  float bottomPadding;


        private float _prevWidth = float.MinValue;
        private float _prevHeight = float.MinValue;
        
        
        private LCHMonoBehaviour LCHMonoBehaviour => _lchMonoBehaviour == null ? _lchMonoBehaviour = gameObject.GetOrAddComponent<LCHMonoBehaviour>() : _lchMonoBehaviour;
        private LCHMonoBehaviour _lchMonoBehaviour;
        
        
        
        protected override bool HorizontalIsChanged()
        {
            if (widthTarget == null) return false;
            
            var result = !Mathf.Approximately(_prevWidth, widthTarget.rect.size.x);
            _prevWidth = widthTarget.rect.size.x;
            return result;
        }
        
        public override void SetLayoutHorizontal() => SetLayout();

        protected override bool VerticalIsChanged()
        {
            if (heightTarget == null) return false;
            
            var result = !Mathf.Approximately(_prevHeight, heightTarget.rect.size.y);
            _prevHeight = heightTarget.rect.size.y;
            return result;
        }

        public override void SetLayoutVertical() => SetLayout();

        private void SetLayout()
        {
            tracker.Clear();
            tracker.Add(this, LCHMonoBehaviour.RectTransformOrNull, widthTarget != null && heightTarget != null ? DrivenTransformProperties.SizeDelta
                : widthTarget != null ? DrivenTransformProperties.SizeDeltaX
                : heightTarget != null ? DrivenTransformProperties.SizeDeltaY
                : DrivenTransformProperties.None
            );
            
            if (widthTarget != null) LCHMonoBehaviour.RectTransformOrNull.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, leftPadding + widthTarget.rect.size.x + rightPadding);
            if (heightTarget != null) LCHMonoBehaviour.RectTransformOrNull.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, topPadding + heightTarget.rect.size.y + bottomPadding);
            if (GetComponent<UIBehaviour>() != null) LayoutRebuilder.MarkLayoutForRebuild(LCHMonoBehaviour.RectTransformOrNull);
        }
    }
}