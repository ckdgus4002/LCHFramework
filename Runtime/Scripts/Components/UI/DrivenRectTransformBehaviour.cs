using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LCHFramework.Components.UI
{
    [ExecuteAlways]
    public abstract class DrivenRectTransformBehaviour : LCHMonoBehaviour, ILayoutSelfController
    {
        protected DrivenRectTransformTracker Tracker { get; } = new();
        
        
        
#if UNITY_EDITOR
        private void OnValidate() => OnReset();

        private void Reset() => OnReset();
#endif  
        private void Update()
        {
            if (AllIsChanged()) SetAll();
            else if (PositionIsChanged()) SetPosition();
            else if (PositionXIsChanged()) SetPositionX();
            else if (PositionYIsChanged()) SetPositionY();
            else if (SizeIsChanged()) SetSize();
            else if (SizeXIsChanged()) SetSizeX();
            else if (SizeYIsChanged()) SetSizeY();
            else if (ScaleIsChanged()) SetScale();
            else if (ScaleXIsChanged()) SetScaleX();
            else if (ScaleYIsChanged()) SetScaleY();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            
            Tracker.Clear();
            if (GetComponent<UIBehaviour>() != null) LayoutRebuilder.MarkLayoutForRebuild(RectTransform);
        }
        
        
        
        public void SetLayoutHorizontal() => OnReset();
        
        public void SetLayoutVertical() => OnReset();

        protected abstract void OnReset();
        
        
        
        protected virtual bool AllIsChanged() => false;

        protected virtual void SetAll() { }
        
        
        
        protected virtual bool PositionIsChanged() => false;
        
        protected virtual void SetPosition() { }
        
        protected virtual bool PositionXIsChanged() => false;
        
        protected virtual void SetPositionX() { }
        
        protected virtual bool PositionYIsChanged() => false;
        
        protected virtual void SetPositionY() { }
        
        
        
        protected virtual bool SizeIsChanged() => false;
        
        protected virtual void SetSize() { }
        
        protected virtual bool SizeXIsChanged() => false;
        
        protected virtual void SetSizeX() { }
        
        protected virtual bool SizeYIsChanged() => false;
        
        protected virtual void SetSizeY() { }
        
        
        
        protected virtual bool ScaleIsChanged() => false;

        protected virtual void SetScale() { }
        
        protected virtual bool ScaleXIsChanged() => false;
        
        protected virtual void SetScaleX() { }

        protected virtual bool ScaleYIsChanged() => false;

        protected virtual void SetScaleY() { }
    }
}