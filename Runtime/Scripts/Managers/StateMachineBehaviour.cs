using System.Collections.Generic;
using UnityEngine;

namespace LCHFramework.Managers
{
    public class StateMachineBehaviour : MonoBehaviour
    {
        public virtual List<IState> States { get; } = new();
        
        
        public virtual IState CurrentState
        {
            get
            {
                if (_currentState == null) CurrentState = StartState;
                    
                return _currentState;
            }
            set
            {
                if (_currentState == (value ??= StartState)) return;
                
                _currentState?.OnExit();
                _currentState = value;
                _currentState.OnEnter();
            }
        }
        private IState _currentState;
        
        public virtual IState StartState => States[0];
        
        
        
        protected virtual void Update() => CurrentState.OnUpdate();
    }
}