namespace FinalStateMachine
{
    public interface IState
    {
        void EnterState();

        void ExitState();

        void UpdateState();
    }
}