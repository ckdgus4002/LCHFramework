using System;
using System.Collections;
using System.Linq;
using LCHFramework.Managers;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

namespace LCHFramework.Components
{
    public class OnlyStep : LCHMonoBehaviour
    {
        [SerializeField] private Step[] steps;
        [SerializeField] private UnityEvent<int, int> onShow;
        [SerializeField] private UnityEvent<int, int> onHide;
        
        
        
        protected override IEnumerator Start()
        {
            yield return StartCoroutine(base.Start());

            MessageBroker.Default.Receive<CurrentStepChangedMessage>().Subscribe(message =>
            {
                var prevStepIndex = message.prevStepOrNull == null ? -1 : message.prevStepOrNull.Index;
                OnCurrentStepIndexChanged(prevStepIndex, message.currentStep.Index);
                
            }).AddTo(gameObject);
        }
        
        
        
        private void OnCurrentStepIndexChanged(int prevStepIndex, int currentStepIndex)
            => OnCurrentStepIndexChanged(() => prevStepIndex, () => currentStepIndex);
        
        private void OnCurrentStepIndexChanged(Func<int> prevStepIndex, Func<int> currentStepIndex)
        {
            var isShow = steps.Any(t => t.Index == currentStepIndex.Invoke());
            
            if (isShow) Show(); else Hide();
            
            (isShow ? onShow : onHide)?.Invoke(prevStepIndex.Invoke(), currentStepIndex.Invoke());
        }

        private void Show() => gameObject.SetActive(true);
        
        private void Hide() => gameObject.SetActive(false);
    }
}