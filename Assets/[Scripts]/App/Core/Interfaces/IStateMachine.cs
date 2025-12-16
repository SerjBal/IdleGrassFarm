using System;

namespace Serjbal.Core
{
    public interface IStateMachine<TState>
    {
        void AddState(TState state);
        void RemoveState(TState state);
        TState GetCurrentState();
        T SwitchToState<T>() where T : TState;
    }
}