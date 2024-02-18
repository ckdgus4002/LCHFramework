using System.Collections.Generic;
using System.Linq;
using LCHFramework.Extensions;
using ShowInInspector;
using UnityEngine;

namespace LCHFramework.Managers
{
    public class SequenceManager : MonoBehaviour
    {
        [SerializeField] private bool playOnAwake;
        [SerializeField] private bool playOnEnable;
        [SerializeField] private bool loop;
        [SerializeField] private Sequence firstSequence;
        
        
        public ReactiveProperty<Sequence> CurrentSequence => _currentSequence ??= new ReactiveProperty<Sequence>(null, OnCurrentSequenceChanged);
        private ReactiveProperty<Sequence> _currentSequence;
        
        public IReadOnlyList<Sequence> Sequences => _sequences.IsEmpty() ? _sequences = GetComponentsInChildren<Sequence>(true).ToArray() : _sequences;
        private IReadOnlyList<Sequence> _sequences;
        
        
        
        protected virtual void Awake()
        {
            if (playOnAwake) CurrentSequence.Value = firstSequence;
        }
        
        protected virtual void OnEnable()
        {
            if (playOnEnable) CurrentSequence.Value = firstSequence;
        }



        protected virtual void OnCurrentSequenceChanged(Sequence prevSequence, Sequence currentSequence)
        {
            foreach (var t in Sequences.Where(t => t.IsShown)) t.Hide();
            currentSequence.Show();
        }

        [ShowInInspector]
        public void ReturnCurrentSequence() => CurrentSequence.Value
            = !loop && CurrentSequence.Value.Index == 0 ? Sequences[0]
            : loop && CurrentSequence.Value.Index == 0 ? Sequences[^1]
            : Sequences[CurrentSequence.Value.Index - 1];
        
        [ShowInInspector]
        public void PassCurrentSequence() => CurrentSequence.Value
            = CurrentSequence.Value.Index < Sequences.Count - 1 ? Sequences[CurrentSequence.Value.Index + 1] 
            : !loop ? Sequences[^1] 
            : Sequences[0];
    }   
}