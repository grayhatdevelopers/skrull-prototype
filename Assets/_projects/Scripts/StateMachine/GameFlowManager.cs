using System;
using UnityEngine;
using UnityHFSM;

public class GameFlowManager : MonoBehaviour
{
    public StateMachine GameFlowMachine { get; private set; }

    public States CurrentState { get; private set; }
    
    private void Start()
    {
        // Create the state machine
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
        GameFlowMachine.AddState(States.Reveal.ToString(), new RevealState(GameFlowMachine));
        GameFlowMachine.AddState(States.GetPoint.ToString(), new GetPointState(GameFlowMachine));
        GameFlowMachine.AddState(States.WinByPoints.ToString(), new WinByPointState(GameFlowMachine));
        GameFlowMachine.AddState(States.Discard.ToString() , new DiscardState(GameFlowMachine));
        
        GameFlowMachine.AddState(States.EliminatePlayer.ToString(),new EliminatePlayerState(GameFlowMachine));
        GameFlowMachine.AddState( States.Rule.ToString(), new RuleState(GameFlowMachine));
        GameFlowMachine.AddState(States.GameEnd.ToString(), new GameEndState(GameFlowMachine));
        
    }

    private void AddTransitions()
    {
        GameFlowMachine.AddTriggerTransition(TriggerEvents.BiddingStarted.ToString(), States.CardPlacement.ToString(),States.Bid.ToString());
        GameFlowMachine.AddTriggerTransition(TriggerEvents.CardSRevealing.ToString(), States.Bid.ToString(),States.Reveal.ToString());
        GameFlowMachine.AddTriggerTransition(TriggerEvents.GainPoint.ToString(), States.Reveal.ToString(),States.GetPoint.ToString());
        GameFlowMachine.AddTriggerTransition(TriggerEvents.PlacingCards.ToString(), States.GetPoint.ToString(),States.CardPlacement.ToString()); 
        GameFlowMachine.AddTriggerTransition(TriggerEvents.TwoPointsAwarded.ToString(), States.GetPoint.ToString(),  States.WinByPoints.ToString());
        GameFlowMachine.AddTriggerTransition(TriggerEvents.RemoveEliminatedPlayer.ToString(), States.Reveal.ToString(), States.Discard.ToString());
        GameFlowMachine.AddTriggerTransition(TriggerEvents.EndsGame.ToString() , States.Discard.ToString(), States.EliminatePlayer.ToString());
        GameFlowMachine.AddTriggerTransition(TriggerEvents.PlacingCards.ToString() , States.Discard.ToString(), States.CardPlacement.ToString());
        GameFlowMachine.AddTriggerTransition(TriggerEvents.ApplyNewRule.ToString() , States.Discard.ToString(), States.Rule.ToString());
        GameFlowMachine.AddTriggerTransition(TriggerEvents.PlacingCards.ToString() , States.Rule.ToString(), States.CardPlacement.ToString());
        
        GameFlowMachine.AddTriggerTransition(TriggerEvents.EndsGame.ToString() , States.WinByPoints.ToString(), States.GameEnd.ToString());
        GameFlowMachine.AddTriggerTransition(TriggerEvents.EndsGame.ToString() , States.EliminatePlayer.ToString(), States.GameEnd.ToString());
        
        
        
    }

    private void Update()
    {
        GameFlowMachine.OnLogic();

        Debug.Log(GameFlowMachine.GetActiveHierarchyPath());
    }
}