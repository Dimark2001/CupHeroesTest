using FinalStateMachine;

public class RewardState : IState
{
    private readonly GameStateMachine _stateMachine;
    private readonly RewardWindow _rewardWindow;

    public RewardState(GameStateMachine gsm, RewardWindow rw)
    {
        _stateMachine = gsm;
        _rewardWindow = rw;
    }

    public void EnterState()
    {
        _rewardWindow.Show(true);
        _rewardWindow.OnHide += () => _stateMachine.SwitchToState<RunState>();
    }

    public void ExitState()
    {
    }

    public void UpdateState()
    {
    }
}