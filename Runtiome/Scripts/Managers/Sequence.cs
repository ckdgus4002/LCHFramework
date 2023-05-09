using LCHFramework.Extensions;
using UnityEngine;

namespace LCHFramework.Managers
{
    public class Sequence : MonoBehaviour
    {
        private int Index => _index ?? (int)(_index = SequenceManager.Sequences.IndexOf(this));
        private int? _index;

        protected SequenceManager SequenceManager => _sequenceManager == null ? _sequenceManager = GetComponentInParent<SequenceManager>() : _sequenceManager;
        private SequenceManager _sequenceManager;
        
        
        
        private void OnValidate() => name = $"{Index} {GetType().Name}";

        private void OnEnable()
        {
            _Show();
        }

        private void OnDisable()
        {
            _Hide();
        }
        
        
        
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

