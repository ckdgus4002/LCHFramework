using UnityEngine;

namespace LCHFramework.Components
{
    public class LookAtTransform : LCHMonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 offset;
        [SerializeField] private bool onlyY;
        [SerializeField] private bool useLocal;
        
        
        
        private void LateUpdate()
        {
            if (target == null) return;

            var direction = target.position - transform.position;
            if (!onlyY && !useLocal)
            {
                transform.rotation = Quaternion.LookRotation(direction, target.up) * Quaternion.Euler(offset);
            }
            else if (!onlyY && useLocal)
            {
                if (direction.normalized.sqrMagnitude == 0) return;

                transform.localRotation = Quaternion.LookRotation(Quaternion.Inverse(transform.parent.rotation) * direction.normalized) * Quaternion.Euler(offset);
            }
            else if (onlyY && !useLocal)
            {
                if (direction is { x: 0, z: 0 }) return;
                
                direction.y = 0;
                transform.rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(offset);
            }
            else
            {
                if (direction.normalized.sqrMagnitude == 0) return;

                direction = (Quaternion.Inverse(transform.parent.rotation) * direction.normalized).normalized;
                if (direction is { x: 0, z: 0 })
                    transform.localRotation = Quaternion.Euler(offset);
                else
                {
                    direction.y = 0;
                    transform.localRotation = Quaternion.LookRotation(direction) * Quaternion.Euler(offset);
                }
            }
        }
    }
}