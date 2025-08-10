using System;
using Cysharp.Threading.Tasks;
using FinalStateMachine;

public class RunState : IState, IDisposable
{
    private readonly GameStateMachine _stateMachine;
    private readonly GamePresenter _gamePresenter;
    private ParallaxBackground _par;
 
    public RunState(GameStateMachine gsm, GamePresenter gamePresenter, ParallaxBackground par)
    {
        _stateMachine = gsm;
        _gamePresenter = gamePresenter;
        _par = par;
    }
    
    public async void EnterState()
    {
        _par.Play();
        _gamePresenter.StartHeroMovement();
        await UniTask.WaitForSeconds(3f);
        _stateMachine.SwitchToState<BattleState>();
    }

    public void UpdateState()
    {
    }

    public void ExitState()
    {
        _par.Stop();
        _gamePresenter.StopHeroMovement();
    }

    public void Dispose()
    {
        
    }
}