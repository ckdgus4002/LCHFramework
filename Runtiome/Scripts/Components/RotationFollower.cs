using UnityEngine;

namespace LCHFramework.Components
{
    [ExecuteAlways]
    public class RotationFollower : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 offset;
        
        
        
        private void Update()
        {
            if (target == null) return;
            
            transform.rotation = target.rotation * Quaternion.Euler(offset);
        }
    }
}