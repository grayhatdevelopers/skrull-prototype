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
        prk.SaveMyTurnData(
            CardsManager.Instance.selectedCardTypes[0]
                .GetComponent<CardInput>()
                .cardVisual.cardType.ToString()
        );

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