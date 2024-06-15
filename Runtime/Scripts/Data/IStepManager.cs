using LCHFramework.Managers;

namespace LCHFramework.Data
{
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