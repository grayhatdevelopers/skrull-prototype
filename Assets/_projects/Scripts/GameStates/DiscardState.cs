
using UnityEngine;
using UnityHFSM;
public class DiscardState : State
{
    private StateMachine gameMachine;

    public DiscardState(StateMachine stateMachine)
    {
        gameMachine = stateMachine;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("Entering Discard State");
    }

    public override void OnLogic()
    {
        base.OnLogic();
        
    }

    public override void OnExit()
    {
        base.OnExit();
        Debug.Log("Exiting Discard State");
    }
    
}
