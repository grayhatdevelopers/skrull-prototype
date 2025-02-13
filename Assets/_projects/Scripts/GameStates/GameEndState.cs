using UnityEngine;
using UnityHFSM;
public class GameEndState:State
{
    private StateMachine gameMachine;

    public GameEndState(StateMachine stateMachine)
    {
        gameMachine = stateMachine;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("Game End");
    }

    public override void OnLogic()
    {
        base.OnLogic();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            gameMachine.Trigger(TriggerEvents.EndsGame.ToString());
        }        
    }

    public override void OnExit()
    {
        base.OnExit();
        Debug.Log("Game End for ELiminated Player");
    }
    
}
    

