using FinalStateMachine;
using UnityEngine;

public class SampleStateMachine : MonoBehaviour
{
    private FinalStateMachine.FinalStateMachine _stateMachine;
    
    private void Start()
    {
        _stateMachine = new BaseFinalStateMachineBuilder().AddStates(new State1()).Build();
        _stateMachine.SwitchToState<State1>();
    }
    
    public void Update()
    {
        _stateMachine.UpdateStates();
    }
}

public class State1 : IState
{
    public void EnterState() { }
    public void ExitState() { }
    public void UpdateState() { }
}