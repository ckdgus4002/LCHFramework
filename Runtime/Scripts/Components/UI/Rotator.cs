using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LCHFramework.Data;
using LCHFramework.Extensions;
using LCHFramework.Utilities;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Debug = LCHFramework.Utilities.Debug;

namespace LCHFramework.Components.UI
{
    public class Rotator : LCHMonoBehaviour, IDragHandler, IEndDragHandler
    {
        public Transform target;
        public Image rotateButton;
        
        [Header("Values")]
        [SerializeField] private Vector3 speed = new(-200f, -200f, 1);
        [SerializeField] private Vector3Bool horizontalSwipeAxis = Vector3Bool.OnlyY;
        [SerializeField] private Vector3Bool verticalSwipeAxis = Vector3Bool.OnlyZ;
        [SerializeField] private Vector3Bool rotateAxis = Vector3Bool.OnlyZ;
        public UnityEvent onBeginDrag;
        public UnityEvent onDrag;
        public UnityEvent onEndDragStart;
        public UnityEvent onEndDragComplete;
        
        [Header("Snap")]
        [SerializeField] [Range(-1, 360)] private float snap = 90;
        [SerializeField] private float snapDuration = .5f;
        
        [Header("!UseSnap")]
        [SerializeField] [Range(0, 1)] private float smoothRatio = .9f;


        public Vector3 PrevVelocity { get; private set; }
        public Vector3 Velocity { get; private set; }
        public Vector3 TotalVelocity { get; private set; }

        private float CanvasHeight => 1080f;

        public bool Interactable
        {
            set
            {
                foreach (var item in Graphics)
                {
                    item.raycastTarget = value;
                }
            }
        }

        public Gesture.Type GestureType
        {
            get => _gesture.value;
            set
            {
                rotateButton.SetActive(rotateAxis != Vector3Bool.False && !GestureTypeUtils.IsRotate(value));
                _gesture.value = value;
            }
        }
        private readonly Gesture _gesture = new();

        private Gesture.Type GetGestureType(int touchCount, Vector2 pointerPositionDelta, bool onlySameAxis)
            => (!onlySameAxis || GestureTypeUtils.IsHorizontalSwipe(GestureType)) && touchCount == 1 && Mathf.Abs(pointerPositionDelta.y) <= Mathf.Abs(pointerPositionDelta.x) && 0 < pointerPositionDelta.x ? Gesture.Type.LeftToRightSwipe
                : (!onlySameAxis || GestureTypeUtils.IsHorizontalSwipe(GestureType)) && touchCount == 1 && Mathf.Abs(pointerPositionDelta.y) <= Mathf.Abs(pointerPositionDelta.x) && pointerPositionDelta.x < 0 ? Gesture.Type.RightToLeftSwipe
                : (!onlySameAxis || GestureTypeUtils.IsVerticalSwipe(GestureType)) && touchCount == 1 && Mathf.Abs(pointerPositionDelta.x) <= Mathf.Abs(pointerPositionDelta.y) && pointerPositionDelta.y < 0 ? Gesture.Type.UpToDownSwipe
                : (!onlySameAxis || GestureTypeUtils.IsVerticalSwipe(GestureType)) && touchCount == 1 && Mathf.Abs(pointerPositionDelta.x) <= Mathf.Abs(pointerPositionDelta.y) && 0 < pointerPositionDelta.y ? Gesture.Type.DownToUpSwipe
                : (!onlySameAxis || GestureTypeUtils.IsRotate(GestureType)) && touchCount == 2 && 0 <= pointerPositionDelta.y ? Gesture.Type.ClockwiseRotate
                : (!onlySameAxis || GestureTypeUtils.IsRotate(GestureType)) && touchCount == 2 && pointerPositionDelta.y <= 0 ? Gesture.Type.CounterclockwiseRotate 
                : GestureType;
        
        private bool UseSnap => -1 < snap;
        
        private bool UseSmooth => float.Epsilon <= smoothRatio;

        private Canvas Canvas => _canvas == null ? _canvas = GetComponentInChildren<Canvas>() : _canvas;
        private Canvas _canvas;

        private IEnumerable<Graphic> Graphics => _graphics.IsEmpty() ? _graphics = GetComponentsInChildren<Graphic>(true) : _graphics;
        private Graphic[] _graphics;
        
        
        
        protected override void Awake()
        {
            base.Awake();
            
            UnityEventUtility.AddPersistentListener(onEndDragComplete, Initialize);
            
            if (Canvas.worldCamera == null) Canvas.worldCamera = Camera.main;
            
            rotateButton.SetActive(rotateAxis != Vector3Bool.False);
        }
        
        protected override void OnEnable()
        {
            base.OnEnable();
            
            Initialize();
        }

        // private void LateUpdate()
        // {
        //     if (Input.touchCount != 2) return;
        //     
        //     if (_onEndDrag != null) Initialize();
        //     
        //     if (Input.touches.Any(touch => touch.phase == TouchPhase.Moved))
        //         OnDrag(Input.touches.Where(touch => collider.GetComponent<Image>().Raycast(touch.position, Canvas.worldCamera)).ToArray());
        //     else if (Input.touches.All(touch => touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled))
        //         OnEndDrag();
        // }
        
        
        
        // UnityEvent event.
        private Coroutine _setType;
        public void SetType(string type)
        {
            _setType = RestartCoroutine(this, _setType, Coroutine());
            IEnumerator Coroutine()
            {
                yield return new WaitWhile(() => GestureType != Gesture.Type.None);

                target.rotation = Quaternion.identity;
                switch (type)
                {
                    case "1":
                        snap = 90;
                        break;
                    case "2":
                        snap = -1;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(type), type, null);
                }
            }
        }
        
        
        
        public void OnDrag(PointerEventData eventData)
        {
            var buttonIsClicked = eventData.pointerCurrentRaycast.gameObject == rotateButton.gameObject;
            if (Input.touchCount < 2)
            {
                if (!buttonIsClicked && !GestureTypeUtils.IsRotate(GestureType))
                {
                    var pointerPositionDelta = GetPointerPositionDelta(1, new[] { eventData.position }, new[] { eventData.delta });
                    _OnDrag(1,
                        () => 1 <= pointerPositionDelta.magnitude,
                        InViewPort(eventData.position),
                        pointerPositionDelta);
                }
                else
                {
                    var pointerPositionDelta = GetPointerPositionDelta(2, new[] { eventData.position }, new[] { eventData.delta });
                    _OnDrag(2,
                        () => GestureType == Gesture.Type.None || .1 <= Quaternion.Angle(Quaternion.Euler(pointerPositionDelta), Quaternion.identity),
                        InViewPort(eventData.position),
                        pointerPositionDelta);
                }    
            }
        }

        // public void OnDrag(params Touch[] touches)
        // {
        //     var pointerPositionDelta = GetPointerPositionDelta(2, touches.Select(item => item.position).ToArray(), touches.Select(item => item.deltaPosition).ToArray()); 
        //     _OnDrag(2, () => .1f <= pointerPositionDelta.y, () => InViewPort(touches.Select(item => (Vector3)item.position).ToArray()), pointerPositionDelta);
        // }
        
        private Vector2 GetPointerPositionDelta(int touchCount, Vector2[] positions, Vector2[] positionsDelta)
        {
            var result = Vector3.zero;
            if (touchCount < 2)
            {
                result = positionsDelta[0];
            }
            else if (touchCount == 2)
            {
                for (var i = 0; i < positions.Length; i++)
                {
                    var prevPosition = transform.InverseTransformPoint(Camera.main.ScreenToWorldPoint(positions[i] - positionsDelta[i]));
                    var currPosition = transform.InverseTransformPoint(Camera.main.ScreenToWorldPoint(positions[i]));
                    var prevRotation = Quaternion.LookRotation(new Vector3(prevPosition.x, 0, prevPosition.y));
                    var currRotation = Quaternion.LookRotation(new Vector3(currPosition.x, 0, currPosition.y));
                    result += currRotation.eulerAngles - prevRotation.eulerAngles;
                }
            }
            return result;
        }

        private bool InViewPort(params Vector3[] screenPositions) => screenPositions.All(item =>
        {
            var viewportPosition = Camera.main.ScreenToViewportPoint(item);
            return 0 <= viewportPosition.x && viewportPosition.x <= 1
                && 0 <= viewportPosition.y && viewportPosition.y <= 1;
        });

        private void _OnDrag(int touchCount, Func<bool> overDragThreshold, bool inViewport, Vector2 pointerPositionDelta)
        {
            if (overDragThreshold == null || !overDragThreshold.Invoke()) return;

            if (!inViewport) return;

            if (_onEndDrag != null) Initialize();

            var getGestureType = GetGestureType(touchCount, pointerPositionDelta, GestureType != Gesture.Type.None);
            GestureType = getGestureType;
            Velocity = GestureTypeUtils.IsHorizontalSwipe(GestureType) ? pointerPositionDelta.x * Vector3.Scale(horizontalSwipeAxis, speed / CanvasHeight)
                : GestureTypeUtils.IsVerticalSwipe(GestureType) ? pointerPositionDelta.y * Vector3.Scale(verticalSwipeAxis, speed / CanvasHeight)
                : GestureTypeUtils.IsRotate(GestureType) ? pointerPositionDelta.y * Vector3.Scale(rotateAxis, speed) 
                : Vector3.zero
                ;
            if (TotalVelocity == Vector3.zero && Velocity != Vector3.zero) onBeginDrag?.Invoke();
            TotalVelocity += Velocity;
            if (Velocity != Vector3.zero) onDrag?.Invoke();
            RotateTarget(Velocity);

            PrevVelocity = Velocity;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (Input.touchCount < 2) _OnEndDrag();
        }

        // public void OnEndDrag()
        // {
        //     if (2 <= Input.touchCount) _OnEndDrag();
        // }

        private Coroutine _onEndDrag;
        private void _OnEndDrag()
        {
            onEndDragStart?.Invoke();
            _onEndDrag = RestartCoroutine(this, _onEndDrag, Coroutine());
            IEnumerator Coroutine()
            {
                if (!UseSnap)
                {
                    bool canSmooth;
                    do
                    {
                        var angle = Quaternion.Angle(Quaternion.Euler(Velocity), Quaternion.identity);
                        canSmooth = UseSmooth && 1 <= angle;
                        if (canSmooth)
                        {
                            var rotate = Quaternion.Slerp(Quaternion.Euler(Velocity), Quaternion.identity, (1 - smoothRatio) * (Time.deltaTime/0.0167f)).eulerAngles;
                            RotateTarget(Velocity = rotate);
                            for (var i = 0; i < Mathf.Max(1, UnityEngine.Application.targetFrameRate / 60f); i++) yield return null;
                        }
                    } while (canSmooth);
                }
                else
                {
                    var snappedEulerAnglesDelta = RoundIfNonZero(TotalVelocity, snap) - TotalVelocity;
#if DOTween
                    target.DORotate(snappedEulerAnglesDelta, snapDuration, RotateMode.WorldAxisAdd);
                    yield return new WaitForSeconds(snapDuration);
#else
                    Debug.LogError("NotImplementedException. Please use DOTween.");
                    yield return new WaitForSeconds(snapDuration);
                    target.Rotate(snappedEulerAnglesDelta, Space.World);
#endif
                }

                onEndDragComplete?.Invoke();
            }
        }
        
        private Vector3 RoundIfNonZero(Vector3 v, float dist)
        {
            v.x = Mathf.Approximately(v.x, 0) ? v.x : v.x.Round(dist);
            v.y = Mathf.Approximately(v.y, 0) ? v.y : v.y.Round(dist);
            v.z = Mathf.Approximately(v.z, 0) ? v.z : v.z.Round(dist);
            return v;
        }
        
        private Vector3 CeilingIfNonZero(Vector3 v, float dist)
        {
            v.x = Mathf.Approximately(v.x, 0) ? v.x : v.x.Ceiling(dist);
            v.y = Mathf.Approximately(v.y, 0) ? v.y : v.y.Ceiling(dist);
            v.z = Mathf.Approximately(v.z, 0) ? v.z : v.z.Ceiling(dist);
            return v;
        }
        
        private Vector3 TruncateIfNonZero(Vector3 v, float dist)
        {
            v.x = Mathf.Approximately(v.x, 0) ? v.x : v.x.Truncate(dist);
            v.y = Mathf.Approximately(v.y, 0) ? v.y : v.y.Truncate(dist);
            v.z = Mathf.Approximately(v.z, 0) ? v.z : v.z.Truncate(dist);
            return v;
        }

        private void Initialize()
        {
            GestureType = Gesture.Type.None;
            PrevVelocity = Vector3.zero;
            Velocity = Vector3.zero;
            TotalVelocity = Vector3.zero;
            
            if (_onEndDrag != null) StopCoroutine(_onEndDrag);
            _onEndDrag = null;
        }

        private void RotateTarget(Vector3 eulers)
        {
            if (target != null) target.Rotate(eulers, Space.World);
        }
        
        
        
        public class Gesture
        {
            public Type value = Type.None;


        
            public enum Type
            {
                None = -1,
                LeftToRightSwipe,
                RightToLeftSwipe,
                UpToDownSwipe,
                DownToUpSwipe,
                ClockwiseRotate,
                CounterclockwiseRotate,    
            }
        }
        
        private class GestureTypeUtils
        {
            public static bool IsHorizontalSwipe(Gesture.Type gestureType) => Gesture.Type.LeftToRightSwipe <= gestureType && gestureType <= Gesture.Type.RightToLeftSwipe;
            
            public static bool IsVerticalSwipe(Gesture.Type gestureType) => Gesture.Type.UpToDownSwipe <= gestureType && gestureType <= Gesture.Type.DownToUpSwipe;
            
            public static bool IsRotate(Gesture.Type gestureType) => Gesture.Type.ClockwiseRotate <= gestureType && gestureType <= Gesture.Type.CounterclockwiseRotate;
        }
    }
}