using UnityEngine;
using UnityHFSM;

public class BiddingState : State
{
    private StateMachine gameMachine;

    public BiddingState(StateMachine stateMachine)
    {
        gameMachine = stateMachine;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("Entering BiddingState");
    }

    public override void OnLogic()
    {
        base.OnLogic();
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}