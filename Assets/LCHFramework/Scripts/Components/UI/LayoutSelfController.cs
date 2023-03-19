using LCHFramework.Modules;
using UnityEngine;
using UnityEngine.UI;

namespace LCHFramework.Components.UI
{
    [ExecuteAlways]
    [RequireComponent(typeof(RectTransform))]
    public abstract class LayoutSelfController : LCHMonoBehaviour, ILayoutSelfController
    {
        // field is never assigned warning
        #pragma warning disable 649
        protected DrivenRectTransformTracker tracker;
        #pragma warning restore 649



        private void Update()
        {
            if (PositionXIsChanged()) SetPositionX();
            if (PositionYIsChanged()) SetPositionY();
            if (HorizontalIsChanged()) SetLayoutHorizontal();
            if (VerticalIsChanged()) SetLayoutVertical();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            
            tracker.Clear();
            LayoutRebuilder.MarkLayoutForRebuild(RectTransform);
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