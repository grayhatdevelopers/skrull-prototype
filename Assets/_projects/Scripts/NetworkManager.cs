using System;
using System.Collections.Generic;
using System.Linq;
using Playroom;
using SimpleJSON;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager Instance { get; private set; }

    [Header("temp")]
    public TMP_Text me;

    [Header("External")]
    [SerializeField]
    private GameObject cardsHolder;
    [SerializeField]
    private GameFlowManager gameFlowManager;

    public bool GameStarted { get; private set; }
    public bool IsHost { get; private set; }

    private PlayroomKit prk;
    // This list will be overwritten on each client based on the host’s ordering.
    private List<PlayroomKit.Player> players = new();
    private List<string> playerIdsOnHost = new();

    #region BiddingStateDeclaration

    [Header("UI References")]
    public TMP_Text highestBidText;
    public TMP_Text currentTurnText;
    
    private int highestBid = 0;
    private string highestBidderString = "";
    private PlayroomKit.Player highestBidder = null;
    private int passCount = 0;
    private int currentTurnIndex = 0;
    
    
    #endregion    
    

    [SerializeField]
    List<TurnData> allTurns = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        prk = new PlayroomKit();
    }

    private void Start()
    {
        InitOptions options = new InitOptions
        {
            maxPlayersPerRoom = 6,
            turnBased = true,
            skipLobby = true,
        };

        prk.InsertCoin(options, OnLaunch);
    }

    private void OnLaunch()
    {
        prk.OnPlayerJoin(AddPlayer);
        prk.RpcRegister("NextTurn", HandleNextTurn);
        prk.RpcRegister("UpdatePlayerOrder", HandleUpdatePlayerOrder);
        prk.RpcRegister("UpdateBid", HandleUpdateBid);
    }

    private void HandleNextTurn(string data, string sender)
    {
        UpdateAllTurnsList(() =>
        {
            TurnData latestTurn = allTurns.Last();

            me.text = ($"{latestTurn.data} played by {latestTurn.player.GetProfile().name}");

            int totalTurns = allTurns.Count;
            if (players.Count == 0)
            {
                Debug.LogWarning("No players available to determine turn.");
                return;
            }

            int currentTurnIndex = totalTurns % players.Count;
            PlayroomKit.Player currentPlayer = players[currentTurnIndex];

            me.text += $"\n\nIt is now {currentPlayer.GetProfile().name}'s turn.";
            gameFlowManager.playButton.interactable = (currentPlayer.id == prk.MyPlayer().id);
        });
    }
    
    # region BiddingStateFunctions

    public void PlayerBid(int bidAmount)
    {
        highestBid = bidAmount;
        highestBidder = players[currentTurnIndex];
      
        
        
        highestBidText.text = $"Highest Bid : {highestBid} by {highestBidder.GetProfile().name}";
        
        passCount = 0;
        
        prk.RpcCall("UpdateBid", $"{highestBid},{highestBidder.id}", PlayroomKit.RpcMode.ALL);
        
        
        NextTurn();
    }

    public void PlayerPass()
    {
        passCount++;
        if (passCount >= players.Count - 1)
        {
            Debug.LogWarning("All players have passed. Move to reveal State");
            //Reveal state func to be called in here
        }
        else
        {
            NextTurn();
        }
    }

    private void NextTurn()
    {
        currentTurnIndex = (currentTurnIndex + 1) % players.Count;
        PlayroomKit.Player currentPlayer = players[currentTurnIndex];
        currentTurnText.text = $" its {currentPlayer.GetProfile().name}'s turn. ";
        gameFlowManager.playButton.interactable = (currentPlayer.id == prk.MyPlayer().id);
    }
    
    private void HandleUpdateBid(string data, string sender)
    {
        string[] bidData = data.Split(',');
        highestBid = int.Parse(bidData[0]);

        string bidderId = bidData[1];
        highestBidder = players.FirstOrDefault(p => p.id == bidderId);

        highestBidText.text = $"Highest Bid: {highestBid} by {highestBidder.GetProfile().name}";

        Debug.Log($"New highest bid: {highestBid} by {highestBidder.GetProfile().name}");
    }


    public int GetMaxBidAmount()
    {
        return allTurns.Count == 0 ? 2 : allTurns.Count;
    }
    
    
    #endregion
    

    private void AddPlayer(PlayroomKit.Player player)
    {
        Debug.LogWarning(player.GetProfile().name + " joined the room");
        me.text = prk.MyPlayer().GetProfile().name;

        IsHost = prk.IsHost();
        cardsHolder.SetActive(true);
        gameFlowManager.playButton.interactable = prk.IsHost();
        gameFlowManager.InitStateMachine();
        GameStarted = true;

        if (prk.IsHost())
        {
            players.Add(player);
            playerIdsOnHost.Add(player.id);
            UpdatePlayerOrder();
        }
    }


    private void UpdatePlayerOrder()
    {
        if (!prk.IsHost())
            return;

        string orderData = string.Join(",", playerIdsOnHost);
        prk.RpcCall("UpdatePlayerOrder", orderData, PlayroomKit.RpcMode.OTHERS);
    }

    private void HandleUpdatePlayerOrder(string data, string sender)
    {
        // Parse the comma-separated list of player IDs.
        string[] idOrder = data.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        players.Clear();
        foreach (string id in idOrder)
        {
            var player = prk.GetPlayer(id);
            if (player != null)
            {
                players.Add(player);
            }
        }

        Debug.LogWarning("Updated local player order from host: " + data + "\n\n\n");
    }


    public void PlayTurn(object data = null)
    {
        // Get the selected card types from the CardsManager.
        List<CardVisual.CardType> selectedCardsTypes = CardsManager.Instance.GetSelectedCardTypes();
       
        // Convert the selected card types to a comma-separated string.
        string selectedCards = string.Join(",", selectedCardsTypes);

        Debug.Log("selected Cards" + selectedCards);
        
        // Save the selected cards to the turn data.
        prk.SaveMyTurnData(selectedCards);

        gameFlowManager.playButton.interactable = false;
        prk.RpcCall("NextTurn", "", PlayroomKit.RpcMode.ALL);
    }

    private void UpdateAllTurnsList(Action onComplete)
    {
        allTurns.Clear();
        prk.GetAllTurns((data) =>
        {
            JSONNode allData = JSON.Parse(data);
            for (int i = 0; i < allData.Count; i++)
            {
                TurnData turnData = new TurnData
                {
                    id = allData[i]["id"],
                    player = prk.GetPlayer(allData[i]["player"]["id"]),
                    data = allData[i]["data"]
                };
                allTurns.Add(turnData);
            }

            Debug.LogWarning("Turn count after update: " + allTurns.Count);
            onComplete?.Invoke();
        });
    }
}

[System.Serializable]
public class TurnData
{
    public string id;
    public PlayroomKit.Player player;
    public string data;
}