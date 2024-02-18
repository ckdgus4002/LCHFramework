using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LCHFramework.Extensions;
using UnityEngine;

namespace LCHFramework.Managers
{
    public class DelaySequence : Sequence
    {
        [SerializeField] private float delay;


        private float _defaultDelay;
        private readonly List<Task> tasks = new();
        
        
        
        private void Awake()
        {
            _defaultDelay = delay;
        }

        private void OnDisable()
        {
            foreach (var t in tasks) t.Dispose();
            tasks.Clear();
        }

        

        public override async void Show()
        {
            base.Show();
            
            foreach (var t in tasks) t.Dispose();
            tasks.Clear();
            
            delay = _defaultDelay;
            await tasks.AddAndReturn(Task.Delay(TimeSpan.FromSeconds(delay))); 

            SequenceManager.PassCurrentSequence();
        }
    }
}
