using System.Collections.Generic;
using UnityEngine;
using UnityHFSM;

public class CardPlacementState : State
{
    private StateMachine gameMachine;
    private Stack<GameObject> placedCards = new Stack<GameObject>();
    
    
#region 
  
    // Tracking # of cards placed by player
    private Dictionary<int, List<GameObject>> playerCards = new Dictionary<int, List<GameObject>>();

    public void PlaceCard(int playerId, GameObject card)
    {
        if (!playerCards.ContainsKey(playerId))
            playerCards[playerId] = new List<GameObject>();
        
        playerCards[playerId].Add(card);
        Debug.Log("Placed card");
    }
    
#endregion    

#region




  
    

#endregion

    public CardPlacementState(StateMachine stateMachine)
    {
        gameMachine = stateMachine;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        Debug.Log("Entering CardPlacementState ");
    }
    
    
    public override void OnLogic()
    {
        base.OnLogic();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            gameMachine.Trigger(TriggerEvents.BiddingStarted.ToString());
        }
    }

    

    public void PlaceCard(GameObject card)
    {
        placedCards.Push(card);
        Debug.Log($"Card {card.name} placed. Stack count: {placedCards.Count} ");

    }
    
    
    
        
    public override void OnExit()
    {
        base.OnExit();
        Debug.Log("Exiting CardPlacementState");
    }
    
    
}

