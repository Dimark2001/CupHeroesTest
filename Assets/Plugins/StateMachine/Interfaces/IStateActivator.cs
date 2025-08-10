namespace FinalStateMachine
{
    public interface IStateActivator
    {
        void ActivateState(IState state);
        void DeactivateState(IState state);
        void UpdateStates();
    }
}