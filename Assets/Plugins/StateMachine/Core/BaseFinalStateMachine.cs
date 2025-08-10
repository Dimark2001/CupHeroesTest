namespace FinalStateMachine
{
    public abstract class BaseFinalStateMachine
    {
        protected readonly IStateActivator StateActivator;
        protected readonly StateRegistry StateRegistry;
        private readonly ILogger _logger;

        protected BaseFinalStateMachine(StateRegistry stateRegistry, StateActivator stateActivator,
            ILogger logger = null)
        {
            StateRegistry = stateRegistry;
            StateActivator = stateActivator;
            _logger = logger ?? new NullLogger();
        }

        protected void SwitchToStateBase<TState>() where TState : IState
        {
            var state = GetStateFromRegistryBase<TState>();
            if (state == null) return;
            StateActivator.ActivateState(state);
        }

        protected IState GetStateFromRegistryBase<TState>() where TState : IState
        {
            var state = StateRegistry.GetRegisteredStateByType<TState>();
            if (state == null)
            {
                _logger.LogError($"State of type {typeof(TState)} not found.");
                return null;
            }

            return state;
        }

        public void UpdateStates()
        {
            StateActivator.UpdateStates();
        }

        public class NullLogger : ILogger
        {
            public void Log(string message)
            {
            }

            public void LogWarning(string message)
            {
            }

            public void LogError(string message)
            {
            }
        }
    }
}