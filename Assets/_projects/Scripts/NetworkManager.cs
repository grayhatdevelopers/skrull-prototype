using Playroom;
using UnityEngine;

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
    

}