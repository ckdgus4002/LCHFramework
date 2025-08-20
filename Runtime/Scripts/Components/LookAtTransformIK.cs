using UnityEngine;

namespace LCHFramework.Components
{
    [RequireComponent(typeof(Animator))]
    public class LookAtTransformIK : MonoBehaviour
    {
        [SerializeField] private Transform target;
        
        
        private Animator Animator => _animator == null ? _animator = GetComponent<Animator>() : _animator;
        private Animator _animator;
        
        
        
        private void OnAnimatorIK(int layerIndex)
        {
            Animator.SetLookAtPosition(target.position);
            Animator.SetLookAtWeight(1);
        }
    }
}