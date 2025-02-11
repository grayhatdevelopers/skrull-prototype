using UnityEngine;
using UnityHFSM;

public class CardPlacementState : State
{
    private StateMachine gameMachine;

    public CardPlacementState(StateMachine stateMachine)
    {
        gameMachine = stateMachine;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        Debug.Log("Entering CardPlacementState ");
    }

    public override void OnLogic()
    {
        base.OnLogic();
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            gameMachine.Trigger(TriggerEvents.BiddingStarted.ToString());
        }
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}