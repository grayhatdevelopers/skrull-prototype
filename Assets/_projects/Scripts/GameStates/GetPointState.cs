using UnityEngine;
using UnityHFSM;
public class GetPointState : State
{
    private StateMachine gameMachine;

    public GetPointState(StateMachine stateMachine)
    {
        gameMachine = stateMachine;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("Entering Get Point State");
    }

    public override void OnLogic()
    {
        base.OnLogic();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            gameMachine.Trigger(TriggerEvents.GainPoint.ToString());
        }
        
    }

    public override void OnExit()
    {
        base.OnExit();
        Debug.Log("Exiting GetPoint State");
    }
    
}