using LCHFramework.Managers;

namespace LCHFramework.Data
{
    public interface IReadOnlyStepManager : IReadOnlyStepManager<Step>
    {
    }
    
    public interface IReadOnlyStepManager<out T> where T : Step
    {
        public event OnValueChangedDelegate<T> OnCurrentStepChanged;
        
        
        public T PrevStepOrNull { get; }
        
        public T LeftStepOrNull { get; }
        
        public T RightStepOrNull { get; }


        public T GetCurrentStep();

        public void SetCurrentStep(Step value);
        
        public T FirstStep { get; }
        
        public T LastStep { get; }
        
        public T[] Steps { get; }
        
        
        
        public void PassCurrentStep();
    }
}