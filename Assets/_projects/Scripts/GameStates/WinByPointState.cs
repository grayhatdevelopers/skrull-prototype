using UnityEngine;
using UnityHFSM;

public class WinByPointState : State
{
    private StateMachine gameMachine;
    public WinByPointState(StateMachine stateMachine)
    {
        gameMachine = stateMachine;
    }
    public override void OnEnter()
    {
        base.OnEnter();

        Debug.Log("Won By Point ");
    }
    public override void OnLogic()
    {
        base.OnLogic();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            gameMachine.Trigger(TriggerEvents.TwoPointsAwarded.ToString());
        }
    }
    
    public override void OnExit()
    {
        base.OnExit();
        Debug.Log("Exiting WinByPointState");
    }


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
