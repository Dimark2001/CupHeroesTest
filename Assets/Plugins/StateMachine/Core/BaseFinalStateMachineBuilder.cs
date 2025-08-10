namespace FinalStateMachine
{
    public class BaseFinalStateMachineBuilder
    {
        private readonly StateRegistry _stateRegistry = new();
        private StateActivator _stateActivator;
        private FinalStateMachine _finalStateMachine;

        public BaseFinalStateMachineBuilder AddStates(params IState[] states)
        {
            foreach (var state in states)
            {
                _stateRegistry.AddStateToRegistry(state);
            }

            return this;
        }

        public FinalStateMachine Build()
        {
            _stateActivator = new StateActivator();
            _finalStateMachine = new FinalStateMachine(_stateRegistry, _stateActivator);

            return _finalStateMachine;
        }
    }
}