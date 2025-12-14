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
        public static bool OverlapsAtWorld(Transform transform, Transform other) => OverlapsAtWorld(GetWorldCorners(transform), GetWorldCorners(other));
        
        public static bool OverlapsAtWorld(Vector3[] corners, Vector3[] other) => OverlapsAtWorld(GetWorldRect(corners), GetWorldRect(other));
        
        public static bool OverlapsAtWorld(Rect worldRect, Rect other) => worldRect.Overlaps(other);
        
        private static Rect GetWorldRect(Vector3[] corners) => new(corners[0].x, corners[0].y, corners[2].x - corners[0].x, corners[2].y - corners[0].y);
        
        private static Vector3[] GetWorldCorners(Transform target)
        {
            var result = new Vector3[4];
            if (target is RectTransform targetRt)
                targetRt.GetWorldCorners(result);
            else
            {
                var bounds = target.TryGetComponent<Collider2D>(out var result0) ? result0.bounds
                    : target.TryGetComponent<Renderer>(out var result1) ? result1.bounds
                    : throw new OutOfRangeException("bounds");
                var position = target.position;
                result[0] = position + bounds.min;
                result[1] = position + new Vector3(bounds.min.x, bounds.max.y, 0);
                result[2] = position + bounds.max;
                result[3] = position + new Vector3(bounds.max.x, bounds.min.y, 0);
            }
            return result;
        }
        
        
        
        [Header("DragAndDrop")]
        public InteractionAreas[] interactionAreas = Array.Empty<InteractionAreas>();
        
        
        public int DefaultSortingOrder { get; private set; }
        
        
        public event Action<PointerEventData> onBeginDrag;
        public event Action<PointerEventData> onDrag;
        public event Action<PointerEventData, int> onEndDrag; // 몇 번째 Rect.
        
        
        public Vector3 BeginPosition { get; private set; }
        public Vector3 BeginMousePosition { get; private set; }
        public bool IsDragging { get; set; }
        
        
        public virtual int SortingOrder
        {
            get
            {
                return CanvasOrNull != null ? CanvasOrNull.sortingOrder
                    : RendererOrNull != null ? RendererOrNull.sortingOrder
                    : RootCanvasOrNull != null ? RootCanvasOrNull.sortingOrder
                    : -1;
            }
            set
            {
                if (CanvasOrNull != null) CanvasOrNull.sortingOrder = value;
                else if (RendererOrNull != null) RendererOrNull.sortingOrder = value;
            }
        }
        
        public virtual Canvas CanvasOrNull => GetComponent<Canvas>();
        
        public virtual Renderer RendererOrNull => GetComponent<Renderer>();
        
        
        
        protected override void Awake()
        {
            base.Awake();
            
            DefaultSortingOrder = SortingOrder;
        }
        
        
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            IsDragging = true;
            BeginPosition = transform.position;
            BeginMousePosition = GetMousePosition(eventData);
            SortingOrder = GetDragSortingOrder();
            
            onBeginDrag?.Invoke(eventData);
        }
        
        public virtual int GetDragSortingOrder() => DefaultSortingOrder + 1;
        
        public void OnDrag(PointerEventData eventData)
        {
            transform.position = GetDragPosition(eventData);
            
            onDrag?.Invoke(eventData);
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
            : RootCanvasOrNull.worldCamera.ScreenToWorldPoint(eventData.position);
        
        private int GetOverlapsInteractionAreaIndex()
        {
            var cornersRect = GetWorldRect(GetWorldCorners(GetOverlapsTarget()));
            foreach (var ia in interactionAreas.OrderByDescending(item => item.overlapsPriority))
                if (ia.areas.Any(iaa => iaa.gameObject.activeSelf && OverlapsAtWorld(cornersRect, GetWorldRect(GetWorldCorners(iaa)))))
                    return interactionAreas.IndexOf(ia);
            
            return -1;
        }
        
        protected virtual Transform GetOverlapsTarget() => transform;
    }
}