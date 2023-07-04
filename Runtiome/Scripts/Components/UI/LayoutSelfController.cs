using UnityEngine;
using UnityEngine.UI;

namespace LCHFramework.Components.UI
{
    [ExecuteAlways]
    [RequireComponent(typeof(RectTransform))]
    public abstract class LayoutSelfController : MonoBehaviour, ILayoutSelfController
    {
        protected DrivenRectTransformTracker tracker;
        
        
        private LCHMonoBehaviour LCHMonoBehaviour => LCHMonoBehaviour.GetOrAddComponent(gameObject);
        
        
        
        private void Update()
        {
            if (PositionXIsChanged()) SetPositionX();
            if (PositionYIsChanged()) SetPositionY();
            if (HorizontalIsChanged()) SetLayoutHorizontal();
            if (VerticalIsChanged()) SetLayoutVertical();
        }

        private void OnDisable()
        {
            tracker.Clear();
            LayoutRebuilder.MarkLayoutForRebuild(LCHMonoBehaviour.RectTransform);
        }



        protected virtual bool PositionXIsChanged() => false;

        protected virtual void SetPositionX()
        {
        }
        
        protected virtual bool PositionYIsChanged() => false;

        protected virtual void SetPositionY()
        {
        }
        
        protected virtual bool HorizontalIsChanged() => false;
        
        public virtual void SetLayoutHorizontal()
        {
        }

        protected virtual bool VerticalIsChanged() => false;

        public virtual void SetLayoutVertical()
        {
        }
    }
}