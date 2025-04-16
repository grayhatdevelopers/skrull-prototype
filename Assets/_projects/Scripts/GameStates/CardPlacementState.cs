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

        // Here you can add logic to set up the state, such as initializing variables or UI elements.
    }

    public override void OnLogic()
    {
        base.OnLogic();

        // Here you can add logic that should run every frame while in this state.
    }

    public override void OnExit()
    {
        base.OnExit();
    }

}