using System;
using System.Linq;
using LCHFramework.Data;
using LCHFramework.Extensions;
using UnityEngine;
using UnityEngine.EventSystems;

namespace LCHFramework.Components
{
    [Serializable]
    public class InteractionAreas
    {
        public int overlapsPriority;
        public Transform[] areas = Array.Empty<Transform>();
        
        public Vector3 Center => areas.Aggregate(Vector3.zero, (aggregate, area) => aggregate + area.position) / areas.Length;
    }
    
    public class DragAndDrop : LCHMonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public static Vector3[] GetCornerAtWorld(Transform target)
        {
            var result = new Vector3[4];
            if (target is RectTransform targetRectTransform)
            {
                targetRectTransform.GetWorldCorners(result);
            }
            else
            {
                var bounds = target.TryGetComponent<Collider2D>(out var result0) ? result0.bounds
                        : target.TryGetComponent<Renderer>(out var result1) ? result1.bounds
                        : throw new OutOfRangeException("bounds")
                    ;
                var targetPosition = target.position;
                result[0] = targetPosition + bounds.min;
                result[1] = targetPosition + new Vector3(bounds.min.x, bounds.max.y, 0);
                result[2] = targetPosition + bounds.max;
                result[3] = targetPosition + new Vector3(bounds.max.x, bounds.min.y, 0);
            }
            return result;
        }

        public static bool OverlapsAtWorld(Vector3[] corner, Vector3[] other) => OverlapsAtWorld(
            new Rect(corner[0].x, corner[0].y, corner[2].x - corner[0].x, corner[2].y - corner[0].y)
            , new Rect(other[0].x, other[0].y, other[2].x - other[0].x, other[2].y - other[0].y)
        );

        public static bool OverlapsAtWorld(Rect rect, Rect other) => rect.Overlaps(other);
        
        
        
        [Header("DragAndDrop")]
        public InteractionAreas[] interactionAreas = Array.Empty<InteractionAreas>();
        
        
        public int DefaultSortingOrder { get; private set; }
        
        
        public event Action onBeginDrag;
        public event Action onDrag;
        public event Action<PointerEventData, int> onEndDrag; // 몇 번째 Rect.
        
        
        public Vector3 BeginPosition { get; private set; }
        public Vector3 BeginMousePosition { get; private set; }
        public virtual bool IsDragging { get; set; }
        
        
        public virtual int SortingOrder
        {
            get
            {
                return CanvasOrNull != null ? CanvasOrNull.sortingOrder
                    : RendererOrNull != null ? RendererOrNull.sortingOrder
                    : RootCanvasOrNull != null ? RootCanvasOrNull.sortingOrder
                    : -1
                    ;
            }
            set
            {
                if (CanvasOrNull != null) CanvasOrNull.sortingOrder = value;
                else if (RendererOrNull != null) RendererOrNull.sortingOrder = value;
            }
        }

        public virtual Canvas CanvasOrNull => GetComponent<Canvas>();

        public virtual Renderer RendererOrNull => GetComponent<Renderer>();
        
        
        
        protected override void  Start()
        {
            base.Start();
            
            DefaultSortingOrder = SortingOrder;
        }
        
        
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            IsDragging = true;
            BeginPosition = transform.position;
            BeginMousePosition = GetMousePosition(eventData);
            SortingOrder = GetDragSortingOrder();

            onBeginDrag?.Invoke();
        }

        public virtual int GetDragSortingOrder() => DefaultSortingOrder + 1;
        
        public void OnDrag(PointerEventData eventData)
        {
            transform.position = GetDragPosition(eventData);
            
            onDrag?.Invoke();
        }

        public virtual Vector3 GetDragPosition(PointerEventData eventData) => GetMousePosition(eventData) + (BeginPosition - BeginMousePosition);

        public void OnEndDrag(PointerEventData eventData)
        {
            IsDragging = false;
            SortingOrder = DefaultSortingOrder;
            
            onEndDrag?.Invoke(eventData, GetOverlapsInteractionAreaIndex());
        }
        
        public virtual Vector3 GetMousePosition(PointerEventData eventData)
            => RootCanvasOrNull == null || RootCanvasOrNull.renderMode == RenderMode.ScreenSpaceOverlay ? Input.mousePosition
            : RootCanvasOrNull.worldCamera.ScreenToWorldPoint(eventData.position)
            ;

        private int GetOverlapsInteractionAreaIndex()
        {
            var corner = GetCornerAtWorld(GetOverlapsTarget());
            foreach (var interactionArea in interactionAreas.OrderByDescending(item => item.overlapsPriority))
                if (interactionArea.areas.Any(interactionAreaArea => interactionAreaArea.gameObject.activeSelf
                                                                     && OverlapsAtWorld(corner, GetCornerAtWorld(interactionAreaArea))))
                    return interactionAreas.IndexOf(interactionArea);

            return -1;
        }

        protected virtual Transform GetOverlapsTarget() => transform;
    }
}