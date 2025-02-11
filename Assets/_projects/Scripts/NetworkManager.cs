using System.Collections.Generic;
using System.Linq;
using Playroom;
using UnityEngine;
using UnityEngine.Serialization;

public class NetworkManager : MonoBehaviour
{
    [Header("External")]
    [SerializeField]
    private GameObject cardsHolder;
    [SerializeField]
    private GameFlowManager gameFlowManager;

    public static NetworkManager Instance { get; private set; }
    private PlayroomKit prk;

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
            skipLobby = true,
        };

        prk.InsertCoin(options, OnLaunch);
    }

    private void OnLaunch()
    {
        cardsHolder.SetActive(true);
        gameFlowManager.InitStateMachine();
    }

    public void PlayTurn(object data = null)
    {
        prk.SaveMyTurnData(CardsManager.Instance.selectedCardTypes);
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
            

        });
    }
}