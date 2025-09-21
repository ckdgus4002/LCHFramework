using UnityEngine;
using UnityEngine.UI;

namespace LCHFramework.Components.UI
{
    [ExecuteAlways]
    public abstract class LayoutSelfController : DrivenRectTransformBehaviour, ILayoutSelfController
    {
        private void Update() => Refresh();
        
        public void SetLayoutHorizontal() => Refresh();
        
        public void SetLayoutVertical() => Refresh();
        
        private void Refresh()
        {
            if (PositionXIsChanged()) SetPositionX();
            if (PositionYIsChanged()) SetPositionY();
            if (PositionZIsChanged()) SetPositionZ();
            if (SizeXIsChanged()) SetSizeX();
            if (SizeYIsChanged()) SetSizeY();
            if (ScaleXIsChanged()) SetScaleX();
            if (ScaleYIsChanged()) SetScaleY();
        }
        
        
        
        protected virtual bool PositionXIsChanged() => false;

        protected virtual void SetPositionX() { }
        
        protected virtual bool PositionYIsChanged() => false;

        protected virtual void SetPositionY() { }
        
        protected virtual bool PositionZIsChanged() => false;

        protected virtual void SetPositionZ() { }
        
        protected virtual bool SizeXIsChanged() => false;
        
        protected virtual void SetSizeX() { }
        
        protected virtual bool SizeYIsChanged() => false;

        protected virtual void SetSizeY() { }
        
        protected virtual bool ScaleXIsChanged() => false;
        
        protected virtual void SetScaleX() { }

        protected virtual bool ScaleYIsChanged() => false;

        protected virtual void SetScaleY() { }
    }
}