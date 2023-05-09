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


        private float? _defaultPositionX;
        private float? _defaultPositionY;
        private float _prevPositionX;
        private float _prevPositionY;


        private bool IsInitialized => _defaultPositionX != null && _defaultPositionY != null;
        
        private float TargetPositionX => xTarget.position.x;
        
        private float TargetPositionY => yTarget.position.y;
        
        private LCHMonoBehaviour LCHMonoBehaviour => LCHMonoBehaviour.GetOrAddComponent(gameObject);
        
        
        
        private readonly Coroutine[] _onEnable = new Coroutine[2];
        private void OnEnable()
        {
            if (!IsInitialized)
            {
                if (_defaultPositionX == null) _onEnable[0] = LCHMonoBehaviour.RestartCoroutine(_onEnable[0], Coroutine(0));
                if (_defaultPositionY == null) _onEnable[1] = LCHMonoBehaviour.RestartCoroutine(_onEnable[1], Coroutine(1));
                IEnumerator Coroutine(int index)
                {
                    switch (index)
                    {
                        case 0:
                            yield return new WaitWhile(() => xTarget == null);
                            _defaultPositionX = xTarget.position.x;
                            break;
                        case 1:
                            yield return new WaitWhile(() => yTarget == null);
                            _defaultPositionY = yTarget.position.y;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(index), index, null);
                    }
                }
            }
            
        }
        protected override bool PositionXIsChanged()
        {
            if (xTarget == null) return false;
            
            var result = !Mathf.Approximately(_prevPositionX, xTarget.position.x);
            _prevPositionX = xTarget.position.x;
            return result;
        }

        protected override void SetPositionX() => SetPosition();

        protected override bool PositionYIsChanged()
        {
            if (yTarget == null) return false;

            var result = !Mathf.Approximately(_prevPositionY, yTarget.position.y);
            _prevPositionY = yTarget.position.y;
            return result;
        }

        protected override void SetPositionY() => SetPosition();

        private void SetPosition()
        {
            tracker.Clear();
            tracker.Add(this, LCHMonoBehaviour.RectTransformOrNull, xTarget != null && yTarget != null ? DrivenTransformProperties.AnchoredPosition 
                : xTarget != null ? DrivenTransformProperties.AnchoredPositionX
                : yTarget != null ? DrivenTransformProperties.AnchoredPositionY
                : DrivenTransformProperties.None
                );
            
            if (xTarget != null) onSetPositionX?.Invoke(TargetPositionX, _defaultPositionX == null ? 0 : TargetPositionX - (float)_defaultPositionX);
            if (yTarget != null) onSetPositionY?.Invoke(TargetPositionY, _defaultPositionY == null ? 0 : TargetPositionY - (float)_defaultPositionY);
            if (GetComponent<UIBehaviour>() != null) LayoutRebuilder.MarkLayoutForRebuild(LCHMonoBehaviour.RectTransformOrNull);
        }
    }
}