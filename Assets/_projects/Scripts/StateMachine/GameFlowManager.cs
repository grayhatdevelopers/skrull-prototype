using UnityEngine;
using UnityEngine.UI;
using UnityHFSM;

/// <summary>
/// This class manages the game flow using a state machine. It initializes the state machine, adds states and transitions.
/// Use this class to control the game flow and add more states and transitions as needed.
/// </summary>
public class GameFlowManager : MonoBehaviour
{

    // The state machine
    public StateMachine GameFlowMachine { get; private set; }

    // Current state of the game
    public States CurrentState { get; private set; }

    public Button playButton;

    private void Awake()
    {
        playButton.onClick.AddListener(PlaySelectedCards);
    }

    public void InitStateMachine()
    {
        GameFlowMachine = new StateMachine();

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
       
       // Add more states here, for example winning state, battling state etc
    }

    private void AddTransitions()
    {
        //Example: GameFlowMachine.AddTriggerTransition(TriggerEvents.BiddingStarted.ToString(), States.CardPlacement.ToString(),States.Bid.ToString());

        // Add more transitions here, for example from bidding to battle, or from battle to winning state
    }

    private void Update()
    {
        if (!NetworkManager.Instance.GameStarted) return;
        
        GameFlowMachine.OnLogic();
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