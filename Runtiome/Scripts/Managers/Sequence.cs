using LCHFramework.Extensions;
using UnityEngine;

namespace LCHFramework.Managers
{
    public class Sequence : MonoBehaviour
    {
        public int Index => _index ?? (int)(_index = SequenceManager.Sequences.IndexOf(this));
        private int? _index;

        protected SequenceManager SequenceManager => _sequenceManager == null ? _sequenceManager = GetComponentInParent<SequenceManager>() : _sequenceManager;
        private SequenceManager _sequenceManager;
        
        
        
        protected virtual void OnValidate() => name = $"{Index} {GetType().Name}";

        protected virtual void OnEnable() => _Show();

        protected virtual void OnDisable() => _Hide();
        
        
        
        public void Show() => transform.RadioActiveInSiblings(true);

        protected virtual void _Show()
        {
        }

        public void Hide() => gameObject.SetActive(false);

        protected virtual void _Hide()
        {
        }
    }
}

