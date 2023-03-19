using LCHFramework.Modules;
using UnityEngine;

namespace LCHFramework.Components
{
    [ExecuteAlways]
    public class PositionFollower : LCHMonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 offset;
        
        
        
        private void Update()
        {
            if (target == null) return;
            
            transform.position = target.position + offset;
        }
    }   
}