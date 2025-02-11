using UnityEngine;
using UnityHFSM;

public class RuleState : State
{
    private StateMachine gameMachine;
    public RuleState(StateMachine stateMachine)
    {
        gameMachine = stateMachine;
    }
    public override void OnEnter()
    {
        base.OnEnter();

        Debug.Log("New Rule ");
    }
    public override void OnLogic()
    {
        base.OnLogic();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            gameMachine.Trigger(TriggerEvents.ApplyNewRule.ToString());
        }
    }
    
    public override void OnExit()
    {
        base.OnExit();
        Debug.Log("Exiting RuleState");
    }

    
}