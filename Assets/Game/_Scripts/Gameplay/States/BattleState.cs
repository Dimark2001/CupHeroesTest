using System;
using Cysharp.Threading.Tasks;
using FinalStateMachine;
using R3;

public class BattleState : IState, IDisposable
{
    private readonly GameStateMachine _gsm;
    private readonly CompositeDisposable _disposables = new();
    private readonly GamePresenter _gamePresenter;

    public BattleState(GameStateMachine gsm, GamePresenter gamePresenter)
    {
        _gsm = gsm;
        _gamePresenter = gamePresenter;
    }

    public async void EnterState()
    {
        await UniTask.WaitForSeconds(1f);
        await _gamePresenter.SetupBattleSystem();
        _gsm.SwitchToState<RewardState>();
    }

    public void UpdateState()
    {
    }

    public void ExitState()
    {
        _disposables.Clear();
    }

    public void Dispose() => _disposables.Dispose();
}