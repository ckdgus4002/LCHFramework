using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LCHFramework.Components.UI
{
    public class PositionTracker : LayoutSelfController
    {
        public RectTransform xTarget;
        [SerializeField] private UnityEvent<float, float> onSetPositionX; 
        public RectTransform yTarget;
        [SerializeField] private UnityEvent<float, float> onSetPositionY;


        private float _prevTargetPositionX;
        private float _prevTargetPositionY;


        public float? DefaultTargetPositionX { get; private set; }
        public float? DefaultTargetPositionY { get; private set; }
        
        
        
        private readonly Coroutine[] _onEnable = new Coroutine[2];
        protected override void OnEnable()
        {
            base.OnEnable();
            
            if (DefaultTargetPositionX == null) _onEnable[0] = RestartCoroutine(this, _onEnable[0], Coroutine(0));
            if (DefaultTargetPositionY == null) _onEnable[1] = RestartCoroutine(this, _onEnable[1], Coroutine(1));
            IEnumerator Coroutine(int index)
            {
                switch (index)
                {
                    case 0:
                        yield return new WaitWhile(() => xTarget == null);
                        DefaultTargetPositionX = xTarget.position.x;
                        break;
                    case 1:
                        yield return new WaitWhile(() => yTarget == null);
                        DefaultTargetPositionY = yTarget.position.y;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(index), index, null);
                }
            }
        }
        
        protected override bool PositionXIsChanged()
        {
            if (xTarget == null) return false;
            
            var result = !Mathf.Approximately(_prevTargetPositionX, xTarget.position.x);
            _prevTargetPositionX = xTarget.position.x;
            return result;
        }

        protected override void SetPositionX() => SetPosition();

        protected override bool PositionYIsChanged()
        {
            if (yTarget == null) return false;

            var result = !Mathf.Approximately(_prevTargetPositionY, yTarget.position.y);
            _prevTargetPositionY = yTarget.position.y;
            return result;
        }

        protected override void SetPositionY() => SetPosition();

        private void SetPosition()
        {
            Tracker.Clear();
            Tracker.Add(this, RectTransformOrNull, xTarget != null && yTarget != null ? DrivenTransformProperties.AnchoredPosition 
                : xTarget != null ? DrivenTransformProperties.AnchoredPositionX
                : yTarget != null ? DrivenTransformProperties.AnchoredPositionY
                : DrivenTransformProperties.None
                );
            
            if (xTarget != null) onSetPositionX?.Invoke(xTarget.position.x, DefaultTargetPositionX == null ? 0 : xTarget.position.x - (float)DefaultTargetPositionX);
            if (yTarget != null) onSetPositionY?.Invoke(yTarget.position.y, DefaultTargetPositionY == null ? 0 : yTarget.position.y - (float)DefaultTargetPositionY);
            if (GetComponent<UIBehaviour>() != null) LayoutRebuilder.MarkLayoutForRebuild(RectTransformOrNull);
        }
    }
}