using LCHFramework.Managers;

namespace LCHFramework.Data
{
    public delegate void OnCurrentStepIndexChangedDelegate(int prev, int current);
    
    public interface ICurrentStepIndexChanged : IStepIndexManager
    {
        public OnCurrentStepIndexChangedDelegate OnCurrentStepIndexChanged { get; set; }
    }
    
    public interface IStepIndexManager
    {
        public int PrevStepIndex { get; }

        public int CurrentStepIndex { get; }
    }
    
    public interface IStepManager<T> where T : Step
    {
        public T PrevStepOrNull { get; }
        
        public T LeftStepOrNull { get; }
        
        public T RightStepOrNull { get; }


        public T CurrentStep { get; set; }
        
        public T FirstStep { get; }
        
        public T LastStep { get; }
        
        public T[] Steps { get; }
    }
}