namespace LCHFramework.Managers
{
    public interface IState
    {
        public void OnEnter();
        
        public void OnUpdate();
        
        public void OnExit();
    }
}