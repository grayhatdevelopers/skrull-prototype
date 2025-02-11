using UnityEngine;
using UnityHFSM;

public class EliminatePlayerState : State
{
    private StateMachine gameMachine;
    public EliminatePlayerState(StateMachine stateMachine)
    {
        gameMachine = stateMachine;
    }
    public override void OnEnter()
    {
        base.OnEnter();

        Debug.Log("Player Eliminted");
    }
    public override void OnLogic()
    {
        base.OnLogic();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            gameMachine.Trigger(TriggerEvents.RemoveEliminatedPlayer.ToString());
        }
    }
    
    public override void OnExit()
    {
        base.OnExit();
        Debug.Log("Exiting EliminatedPlayerState");
    }


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}