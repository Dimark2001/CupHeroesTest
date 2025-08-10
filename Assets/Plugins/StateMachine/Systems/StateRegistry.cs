using System;
using System.Collections.Generic;

namespace FinalStateMachine
{
    public class StateRegistry
    {
        private readonly Dictionary<Type, IState> _states = new();

        public void AddStateToRegistry(IState state)
        {
            _states[state.GetType()] = state;
        }

        public IState GetRegisteredStateByType<TState>() where TState : IState
        {
            return _states.GetValueOrDefault(typeof(TState));
        }
    }
}