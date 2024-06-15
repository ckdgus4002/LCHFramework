using LCHFramework.Components;
using LCHFramework.Data;
using LCHFramework.Extensions;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace LCHFramework.Managers
{
    public class Step : LCHMonoBehaviour
    {
        public int Index
        {
            get
            {
#if UNITY_EDITOR
                if (!EditorApplication.isPlaying) return GetIndex();
#endif
                return _index < 0 ? _index = GetIndex() : _index;

                int GetIndex() => StepManager.Steps.IndexOf(this);
            }
        }
        private int _index = -1;
        
        public virtual bool IsShown => gameObject.activeSelf;

        protected IReadOnlyStepManager StepManager => _stepManager ??= GetComponentInParent<IReadOnlyStepManager>();
        private IReadOnlyStepManager _stepManager;
        
        
        
        protected virtual void OnValidate() => name = $"{GetType().Name} ({Index})";
        
        
        
        public virtual void Show() => gameObject.SetActive(true);

        public virtual void Hide() => gameObject.SetActive(false);
    }
}

