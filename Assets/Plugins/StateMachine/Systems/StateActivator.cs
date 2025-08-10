namespace FinalStateMachine
{
    public class StateActivator : IStateActivator
    {
        private IState _currentState;

        public void ActivateState(IState state)
        {
            if (_currentState == state) return;

            DeactivateCurrentState();
            _currentState = state;
            state.EnterState();
        }

        public void DeactivateState(IState state)
        {
            if (_currentState != state) return;

            state.ExitState();
            _currentState = null;
        }

        public void UpdateStates()
        {
            _currentState?.UpdateState();
        }

        private void DeactivateCurrentState()
        {
            if (_currentState == null) return;

            var oldState = _currentState;
            _currentState = null;
            oldState.ExitState();
        }
    }
}