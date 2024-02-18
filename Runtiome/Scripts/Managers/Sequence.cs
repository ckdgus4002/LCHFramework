using LCHFramework.Extensions;
using ShowInInspector;
using UnityEngine;

namespace LCHFramework.Managers
{
    public class Sequence : MonoBehaviour
    {
        public int Index => _index ?? (int)(_index = SequenceManager.Sequences.IndexOf(this));
        private int? _index;

        public virtual bool IsShown => gameObject.activeSelf;

        protected SequenceManager SequenceManager => _sequenceManager == null ? _sequenceManager = GetComponentInParent<SequenceManager>() : _sequenceManager;
        private SequenceManager _sequenceManager;
        
        
        
        protected virtual void OnValidate() => name = $"{GetType().Name} ({Index})";
        
        
        
        public virtual void Show()
        {
            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }

        [ShowInInspector]
        public void SetCurrentSequence()
        {
            SequenceManager.CurrentSequence.Value = this;   
        }
    }
}

