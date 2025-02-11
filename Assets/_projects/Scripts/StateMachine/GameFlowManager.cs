using System;
using UnityEngine;
using UnityEngine.UI;
using UnityHFSM;

public class GameFlowManager : MonoBehaviour
{
    public StateMachine GameFlowMachine { get; private set; }

    public States CurrentState { get; private set; }
    
    public Button playButton;

    private void Awake()
    {
        playButton.onClick.AddListener(PlaySelectedCards);
    }

    public void InitStateMachine()
    {
        GameFlowMachine = new StateMachine();
        // Set the current state
        CurrentState = States.CardPlacement;

        AddStatesToMachine();
        AddTransitions();
        // Set the start state
        GameFlowMachine.SetStartState(States.CardPlacement.ToString());
        // Initialize the state machine
        GameFlowMachine.Init();
    }

    private void AddStatesToMachine()
    {
        GameFlowMachine.AddState(States.CardPlacement.ToString(), new CardPlacementState(GameFlowMachine));
        GameFlowMachine.AddState(States.Bid.ToString(), new BiddingState(GameFlowMachine));
    }

    private void AddTransitions()
    {
        GameFlowMachine.AddTriggerTransition(TriggerEvents.BiddingStarted.ToString(), States.CardPlacement.ToString(),
            States.Bid.ToString());
    }

    private void Update()
    {
        GameFlowMachine.OnLogic();

        Debug.Log(GameFlowMachine.GetActiveHierarchyPath());
    }
    
    #region UI Methods
    // Called from the UI, only in one state
    private void PlaySelectedCards()
    {
        Debug.Log("Played Cards");
        NetworkManager.Instance.PlayTurn();
    }
    #endregion
    
}