using LCHFramework.Data;
using UnityEngine;

namespace LCHFramework.Components
{
    [ExecuteAlways]
    [RequireComponent(typeof(BoxCollider))]
    public class BoxColliderSizeController : ColliderSizeController
    {
        [SerializeField] private Vector3Bool only = new(true);
        [SerializeField] private Vector3 offset;


        private Vector3 ColliderSize => Vector3.Scale((Vector3)LCHFramework.Instance.targetScreenResolution + offset, only);
        
        private BoxCollider Collider => _collider == null ? _collider = GetComponent<BoxCollider>() : _collider;
        private BoxCollider _collider;



        private void Update() => Collider.size = ColliderSize;
    }
}