using UnityEngine;

namespace LCHFramework.Components
{
    [RequireComponent(typeof(Animator))]
    public class LookAtCameraIK : LCHMonoBehaviour
    {
        private Animator Animator => _animator == null ? _animator = GetComponent<Animator>() : _animator;
        private Animator _animator;
        
        
        
        private void OnAnimatorIK(int layerIndex)
        {
            Animator.SetLookAtPosition(Camera.main.transform.position);
            Animator.SetLookAtWeight(1);
        }
    }
}