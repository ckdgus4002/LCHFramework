using LCHFramework.Modules;
using UnityEngine;
using MonoBehaviour = LCHFramework.Modules.MonoBehaviour;

namespace LCHFramework.Components
{
    [ExecuteAlways]
    public class ScaleFollower : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 offset;
        
        
        
        private void Update()
        {
            if (target == null) return;
            
            transform.localScale = target.localScale + offset;
        }
    }
}