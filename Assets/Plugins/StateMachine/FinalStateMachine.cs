namespace FinalStateMachine
{
    public class FinalStateMachine : BaseFinalStateMachine
    {
        public FinalStateMachine(StateRegistry stateRegistry, StateActivator stateActivator, ILogger logger = null)
             : base(stateRegistry, stateActivator, logger) { }

        public void SwitchToState<TState>() where TState : IState
        {
            SwitchToStateBase<TState>();
        }
    }
}