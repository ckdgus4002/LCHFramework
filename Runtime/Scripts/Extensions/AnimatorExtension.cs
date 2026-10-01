using System.Linq;
using UnityEngine;

namespace LCHFramework.Extensions
{
    public static class AnimatorExtension
    {
        public static bool TryGetBehaviour<T>(this Animator animator, out T result) where T : StateMachineBehaviour
            => (result = animator.GetBehaviour<T>()) != null;
        
        public static bool TryGetBehaviours<T>(this Animator animator, out T[] result) where T : StateMachineBehaviour
            => (result = animator.GetBehaviours<T>()).Any();
    }
}