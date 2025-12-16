using System;

namespace Serjbal.Core
{
     public interface INotifyStateChanged
    {
        event Action<IState> OnEnterState;
        event Action<IState> OnExitState;
        event Action<IState, IState> OnStateChanged;
    }
}