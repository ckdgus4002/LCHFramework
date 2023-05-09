using System;
using System.Linq;
using LCHFramework.Extensions;
using LCHFramework.Utils;
using UnityEngine;

namespace LCHFramework.Managers
{
    public class SequenceManager : MonoBehaviour
    {
        [SerializeField] private bool playOnEnable = true;
        [SerializeField] private Sequence firstSequence;
        
        
        public event Action<Sequence, Sequence> OnCurrentSequenceChanged; // prev, current.
        
        
        
        public Sequence PreviousSequenceOrNull { get; private set; }
        public Sequence RightSequenceOrNull { get; private set; }
         
         
        public Sequence CurrentSequence
        {
            get => _currentSequence == null ? CurrentSequence = Sequences[0] : _currentSequence;
            set
            {
                if (_currentSequence != value)
                {
                    PreviousSequenceOrNull = _currentSequence;
                    _currentSequence = value;
                    var _currentSequenceIndex = Sequences.IndexOf(value);
                    RightSequenceOrNull = 0 <= _currentSequenceIndex && _currentSequenceIndex < Sequences.Length - 1 ? Sequences[_currentSequenceIndex + 1] : null;
                    _currentSequence.RadioActiveInSiblings(true);
                    OnCurrentSequenceChanged?.Invoke(PreviousSequenceOrNull, _currentSequence);
                }
                
            }
        }
        private Sequence _currentSequence;
         
        public Sequence LastSequence => _lastSequence == null ? _lastSequence = Sequences.Last() : _lastSequence;
        private Sequence _lastSequence;

        public Sequence[] Sequences => ApplicationUtil.IsEditor || _sequences.IsEmpty() ? _sequences = GetComponentsInChildren<Sequence>(true) : _sequences;
        private Sequence[] _sequences;
        
        
        
        private void OnEnable()
        {
            if (playOnEnable && CurrentSequence != firstSequence) CurrentSequence = firstSequence;
        }

        private void Update()
        {
            if (!CurrentSequence.gameObject.activeInHierarchy && RightSequenceOrNull != null) CurrentSequence = RightSequenceOrNull;
        }
    }   
}