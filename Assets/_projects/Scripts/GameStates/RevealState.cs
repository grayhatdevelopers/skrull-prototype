using System.Collections.Generic;
using UnityEngine;
using UnityHFSM;
public class RevealState : State
{
    private StateMachine gameMachine;
    public RevealState(StateMachine stateMachine)
    {
        gameMachine = stateMachine;
    }
    public override void OnEnter()
    {
        base.OnEnter();

        Debug.Log("Entering RevealState ");
    }
    public override void OnLogic()
    {
        base.OnLogic();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            gameMachine.Trigger(TriggerEvents.CardSRevealing.ToString());
        }
    }
    
    public override void OnExit()
    {
        base.OnExit();
        Debug.Log("Exiting RevealState");
    }


    
}
