using LCHFramework.Modules;
using UnityEngine;

namespace LCHFramework.Components
{
    [ExecuteAlways]
    public class RotationFollower : LCHMonoBehaviour
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