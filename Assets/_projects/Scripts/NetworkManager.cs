using System;
using System.Collections.Generic;
using System.Linq;
using Playroom;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Serialization;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager Instance { get; private set; }

    [Header("External")]
    [SerializeField]
    private GameObject cardsHolder;
    [SerializeField]
    private GameFlowManager gameFlowManager;

    public bool GameStarted { get; private set; }
    public bool IsHost { get; private set; }

    private PlayroomKit prk;
    private List<PlayroomKit.Player> players = new();


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
    }

    private void HandleNextTurn(string data, string sender)
    {
        UpdateAllTurnsList();

        TurnData latestTurn = allTurns.Last();
        Debug.Log($"{latestTurn.data} played by {latestTurn.player.GetProfile().name}");
        
        if (latestTurn.player.id == prk.MyPlayer().id)
        {
            gameFlowManager.playButton.interactable = false;
        }
        else
        {
            gameFlowManager.playButton.interactable = true;
        }
    }

    private void AddPlayer(PlayroomKit.Player player)
    {
        Debug.LogWarning(player.GetProfile().name + " joined the room");

        IsHost = prk.IsHost();
        cardsHolder.SetActive(true);

        gameFlowManager.playButton.interactable = prk.IsHost();

        gameFlowManager.InitStateMachine();
        GameStarted = true;
        players.Add(player);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            for (var i = 0; i < players.Count; i++)
            {
                var player = players[i];
                Debug.LogWarning($"Player at index {i}: {player.GetProfile().name}\n\n");
            }
        }
    }

    public void PlayTurn(object data = null)
    {
        prk.SaveMyTurnData(CardsManager.Instance.selectedCardTypes[0].GetComponent<CardInput>().cardVisual.cardType
            .ToString());

        gameFlowManager.playButton.interactable = false;

        prk.RpcCall("NextTurn", "", PlayroomKit.RpcMode.ALL);
    }

    private void UpdateAllTurnsList()
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