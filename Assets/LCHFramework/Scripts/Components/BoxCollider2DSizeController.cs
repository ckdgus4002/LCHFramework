using UnityEngine;

namespace LCHFramework.Components
{
    [ExecuteAlways]
    [RequireComponent(typeof(BoxCollider2D))]
    public class BoxCollider2DSizeController : ColliderSizeController
    {
        [SerializeField] private BooleanVector2 only = new BooleanVector2(true);
        [SerializeField] private Vector2 offset;
        
        
        private Vector2 ColliderSize => Vector2.Scale(CanvasSize + offset, only);
        
        private BoxCollider2D Collider => _collider == null ? _collider = GetComponent<BoxCollider2D>() : _collider;
        private BoxCollider2D _collider;
        
        
        
        private void Update() => Collider.size = ColliderSize;
    }
}