using System.Collections.Generic;
using LCHFramework.Extensions;
using UnityEngine;
using UnityEngine.Events;

namespace LCHFramework.Managers
{
    public class SequenceManager : MonoBehaviour
    {
        [SerializeField] private bool playOnAwake;
        [SerializeField] private bool playOnEnable;
        [SerializeField] private Sequence firstSequence;
        
        
        public UnityEvent<Sequence, Sequence> onCurrentSequenceChanged; // prev, current.


        public ReactiveProperties<Sequence> Sequence => _sequence ??= new ReactiveProperties<Sequence>(null, Sequences, (prevSequenceOrNull, currentSequence) =>
        {
            currentSequence.RadioActiveInSiblings(true);
            onCurrentSequenceChanged?.Invoke(prevSequenceOrNull, currentSequence);
        });
        private ReactiveProperties<Sequence> _sequence;
        
        public IEnumerable<Sequence> Sequences => _sequences.IsEmpty() ? _sequences = GetComponentsInChildren<Sequence>(true) : _sequences;
        private IEnumerable<Sequence> _sequences;
        
        
        
        protected virtual void Awake()
        {
            if (playOnAwake) Sequence.CurrentValue = firstSequence;
        }
        
        protected virtual void OnEnable()
        {
            if (playOnEnable) Sequence.CurrentValue = firstSequence;
        }

        protected virtual void Update()
        {
            if (!Sequence.CurrentValue.gameObject.activeInHierarchy && Sequence.RightValueOrNull != null) Sequence.CurrentValue = Sequence.RightValueOrNull;
        }
    }   
}