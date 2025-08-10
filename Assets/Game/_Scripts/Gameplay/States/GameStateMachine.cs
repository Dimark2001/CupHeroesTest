using FinalStateMachine;
using UnityEngine;
using VContainer;

public class GameStateMachine : MonoBehaviour
{
    private FinalStateMachine.FinalStateMachine _stateMachine;
    private GamePresenter _gamePresenter;
    private ParallaxBackground _par;
    private RewardWindow _rewardWindow;

    [Inject]
    public void Construct(GamePresenter presenter, ParallaxBackground par, RewardWindow rw)
    {
        _gamePresenter = presenter;
        _par = par;
        _rewardWindow = rw;
    }

    public void Start()
    {
        _stateMachine = new BaseFinalStateMachineBuilder()
            .AddStates(
                new RunState(this, _gamePresenter, _par),
                new BattleState(this, _gamePresenter),
                new RewardState(this, _rewardWindow)
            ).Build();

        _stateMachine.SwitchToState<RunState>();
    }

    public void SwitchToState<T>() where T : IState
    {
        _stateMachine.SwitchToState<T>();
    }
}