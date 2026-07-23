using System.Collections;
using System.Linq;
using LCHFramework.Managers.StepManager;
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
            yield return base.Start();
            
            MessageBroker.Default.Receive<OnCurrentStepChangedMessage>().Subscribe(_ => OnCurrentStepIndexChanged()).AddTo(this);
            
            Refresh();
        }
        
        
        
        private void OnCurrentStepIndexChanged()
            => Refresh();
        
        public void Refresh()
        {
            var currentStepIndex = StepManager.Instance.CurrentStep.Index;
            var isShow = steps.Any(t => t.Index == currentStepIndex);
            
            if (isShow) Show(); else Hide();
            
            var prevStepIndex = StepManager.Instance.PrevStepOrNull == null ? -1 : StepManager.Instance.PrevStepOrNull.Index;
            (isShow ? onShow : onHide)?.Invoke(prevStepIndex, currentStepIndex);
        }
        
        private void Show() => gameObject.SetActive(true);
        
        private void Hide() => gameObject.SetActive(false);
    }
}