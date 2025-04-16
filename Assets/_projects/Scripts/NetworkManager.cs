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

    public TMP_Text InfoText;

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

    // This list will be used to store all turns played in the game.
    [SerializeField]
    List<TurnData> allTurns = new();


    // Creates a singleton instance of the NetworkManager, so it can be accessed globally and doesn't get destroyed on scene change
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);


        // Initialize PlayroomKit
        prk = new PlayroomKit();
    }

    private void Start()
    {
        // Configuration for the game room
        InitOptions options = new InitOptions
        {
            skipLobby = false,
            maxPlayersPerRoom = 6,
            turnBased = true,
        };

        // Start the game room
        prk.InsertCoin(options, OnLaunch);
    }

    // This method is called when the game room is launched successfully.
    private void OnLaunch()
    {
        // Handle player joining the game room
        prk.OnPlayerJoin(AddPlayer);

        // RPC registrations for all players
        prk.RpcRegister("NextTurn", HandleNextTurn);
        prk.RpcRegister("UpdatePlayerOrder", HandleUpdatePlayerOrder);
    }

    // This method is called when a player joins the game room.
    // It updates the player list and initializes the game state.
    private void AddPlayer(PlayroomKit.Player player)
    {
        Debug.LogWarning(player.GetProfile().name + " joined the room");
        InfoText.text = prk.MyPlayer().GetProfile().name;

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

    // This method id called by the host whenever a new player joins the game room.
    private void UpdatePlayerOrder()
    {
        if (!prk.IsHost())
            return;

        string orderData = string.Join(",", playerIdsOnHost);
        prk.RpcCall("UpdatePlayerOrder", orderData, PlayroomKit.RpcMode.OTHERS);
    }


    // Called after every turn is played, finds the correct player to play next
    private void HandleNextTurn(string data, string sender)
    {
        UpdateAllTurnsList(() =>
        {
            TurnData latestTurn = allTurns.Last();
            InfoText.text = $"{latestTurn.data} played by {latestTurn.player.GetProfile().name}";

            int totalTurns = allTurns.Count;
            if (players.Count == 0)
            {
                Debug.LogWarning("No players available to determine turn.");
                return;
            }

            int currentTurnIndex = totalTurns % players.Count;
            PlayroomKit.Player currentPlayer = players[currentTurnIndex];

            InfoText.text += $"\n\nIt is now {currentPlayer.GetProfile().name}'s turn.";

            Debug.Log($"{latestTurn.data} played by {latestTurn.player.GetProfile().name}" + $"\n\nIt is now {currentPlayer.GetProfile().name}'s turn.");
            gameFlowManager.playButton.interactable = currentPlayer.id == prk.MyPlayer().id;
        });
    }


    // This is invoke on others by the host, everyone updates their local player order based on the host's order.
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

    // Basic turn data structure to store the player and the cards they played.
    public void PlayTurn(object data = null)
    {
        string turnData = string.Join(", ",
            CardsManager.Instance.selectedCardTypes
            .Select(card => card.GetComponent<CardInput>().cardVisual.cardType.ToString()));

        prk.SaveMyTurnData(turnData);

        gameFlowManager.playButton.interactable = false;
        prk.RpcCall("NextTurn", "", PlayroomKit.RpcMode.ALL);
    }

    // This method updates the list of all turns played in the game.
    private void UpdateAllTurnsList(Action onComplete)
    {
        allTurns.Clear();
        prk.GetAllTurns((allDataList) =>
        {
            allTurns = allDataList;

            Debug.LogWarning("Turn count after update: " + allTurns.Count);
            onComplete?.Invoke();
        });
    }
}

