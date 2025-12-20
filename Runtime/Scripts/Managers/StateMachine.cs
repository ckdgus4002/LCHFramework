using System.Collections.Generic;
using UnityEngine;

namespace LCHFramework.Managers
{
    public interface IState
    {
        public void OnEnter();
        
        public void OnUpdate();
        
        public void OnExit();
    }
    
    public class StateMachine : MonoBehaviour
    {
        [SerializeField] private bool playOnAwake;
        
        
        private IState _currentState;
        
        
        public bool IsPlayed { get; private set; }
        public virtual List<IState> States { get; } = new();
        
        
        public IState CurrentState
        {
            get
            {
                if (_currentState == null) SetState(StartState);
                    
                return _currentState;    
            }
        }
        
        public virtual void SetState(IState value)
        {
            if (_currentState == (value ??= StartState)) return;

            IsPlayed = true;
            if (_currentState != null) _currentState.OnExit();
            _currentState = value;
            value.OnEnter();
        }
        
        public virtual IState StartState => States[0];
        
        
        
        protected virtual void Awake()
        {
            if (playOnAwake) SetState(StartState);
        }
        
        protected virtual void Update()
        {
            if (IsPlayed) CurrentState.OnUpdate();
        }
        
        protected void OnDestroy()
        {
            if (IsPlayed) CurrentState.OnExit();
        }
    }
}