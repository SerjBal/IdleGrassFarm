using System;
using System.Collections.Generic;

namespace Serjbal.Core
{

    public abstract class StateMachineBase<T> : IStateMachine<T>, INotifyStateChanged where T : IState
    {
        private readonly Dictionary<Type, T> _states = new Dictionary<Type, T>();
        private T _currentState;

        public event Action<IState> OnEnterState;
        public event Action<IState> OnExitState;
        public event Action<IState, IState> OnStateChanged;

        public void AddState(T state)
        {
            var stateType = state.GetType();
            if (!_states.ContainsKey(stateType))
            {
                _states.Add(stateType, state);
            }
        }

        public T GetCurrentState()
        {
            return _currentState;
        }

        public void RemoveState(T state)
        {
            var stateType = state.GetType();
            if (!_states.ContainsKey(stateType))
            {
                _states.Remove(stateType);
            }
        }

        public T1 SwitchToState<T1>() where T1 : T
        {
            if (_currentState?.GetType() == typeof(T1))
                return (T1)_currentState;
            var t = typeof(T1);
            if (_states.TryGetValue(t, out var newState))
            {
                var state = _currentState as IState;
                state?.Exit();
                OnExitState?.Invoke(state);

                _currentState = newState;
                OnEnterState?.Invoke(_currentState as IState);

                return (T1)_currentState;
            }

            throw new KeyNotFoundException($"State of type {typeof(T1)} not found in state machine");
        }
    }
}