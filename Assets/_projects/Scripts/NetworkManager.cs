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

    private PlayroomKit prk;
    private List<PlayroomKit.Player> players = new();

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
    }

    private void AddPlayer(PlayroomKit.Player player)
    {
        cardsHolder.SetActive(true);
        gameFlowManager.InitStateMachine();
        
        GameStarted = true;
        players.Add(player);
    }

    public void PlayTurn(object data = null)
    {
        prk.SaveMyTurnData("Temporary data");
        gameFlowManager.playButton.interactable = false;

        NextPlayerTurn();
    }

    private void NextPlayerTurn()
    {
        prk.GetAllTurns((data) =>
        {
            // get the id of the player who's turn is next
            // if it's my turn, enable the play button
            // if it's not my turn, disable the play button
            gameFlowManager.playButton.interactable = false;

            // convert the json data to a list of AllDataHandler using simpleJSON
            var allData = JSON.Parse(data);

            Debug.Log(allData);

            List<TurnData> allDataList = new List<TurnData>();
            for (int i = 0; i < allData.Count; i++)
            {
                allDataList.Add(new TurnData
                {
                    id = allData[i]["id"],
                    player = prk.GetPlayer(allData[i]["player"]["id"]),
                    data = allData[i]["data"]
                });
            }

            foreach (var a in allDataList)
            {
                Debug.Log(a.data);
                Debug.Log(a.player.id);
                Debug.Log(a.player.GetProfile().name);
                Debug.Log(a.id);
            }
            
            // tell other players whose turn it is
            // if it's my turn, enable the play button
            // if it's not my turn, disable the play button
            // I need to use an rpc to tell who just played a turn.
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