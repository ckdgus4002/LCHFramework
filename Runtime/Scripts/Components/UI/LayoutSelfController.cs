using UnityEngine;
using UnityEngine.UI;

namespace LCHFramework.Components.UI
{
    [ExecuteAlways]
    public abstract class LayoutSelfController : DrivenRectTransformBehaviour, ILayoutSelfController
    {
        private void Update()
        {
            if (PositionXIsChanged()) SetPositionX();
            if (PositionYIsChanged()) SetPositionY();
            if (PositionZIsChanged()) SetPositionZ();
            if (HorizontalIsChanged()) SetLayoutHorizontal();
            if (VerticalIsChanged()) SetLayoutVertical();
        }
        
        
        
        protected virtual bool PositionXIsChanged() => false;

        protected virtual void SetPositionX() { }
        
        protected virtual bool PositionYIsChanged() => false;

        protected virtual void SetPositionY() { }
        
        protected virtual bool PositionZIsChanged() => false;

        protected virtual void SetPositionZ() { }
        
        protected virtual bool HorizontalIsChanged() => false;
        
        public virtual void SetLayoutHorizontal() { }

        protected virtual bool VerticalIsChanged() => false;

        public virtual void SetLayoutVertical() { }
    }
}