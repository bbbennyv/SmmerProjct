using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Linq;

public class GameManager : MonoBehaviour
{
    [Header("UI Ref")]
    [SerializeField] private GameObject startUI;
    [SerializeField] private GameObject wonUI;/*
    [SerializeField] private GameObject pauseUI;*/
    [SerializeField] private GameObject upgradeUI;
    [SerializeField] public List<PlayerController> spawnedPlayers;
    [SerializeField] public List<PlayerController> alivePlayers;

    [Header("Timer Config")]
    [SerializeField] private float maxStartTimer = 3;
    [SerializeField] private float maxWinTimer = 3;
    [SerializeField] private float maxUpgradeTimer = 3;
    [SerializeField] private float maxDrawTimer = 3;


    [Header("CountDown UI")]
    [SerializeField] private GameObject countdownUI;
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private CanvasGroup countdownCanvasGroup;

    [SerializeField] private float numberDuration = 1f;
    [SerializeField] private float fightDuration = 0.7f;
    [SerializeField] private float fadeDuration = 1f;

    private State currentState;

    //public GameObject PauseUI => pauseUI;
    //public GameObject GamePlayUI => gameUI;
    public GameObject StartUI => startUI;
    public GameObject WonUI => wonUI;
    public GameObject UpgradeUI => upgradeUI;

    private GameplayState gameplayState = new GameplayState();
    private PauseState pauseState = new PauseState();
    private StartState startState = new StartState();
    private UpgradeState upgradeState = new UpgradeState();
    private WonState wonState = new WonState();

    public static GameManager Instance;

    public bool IsGameplay => currentState is GameplayState || currentState is WonState;
    public bool IsUpgrade => currentState is UpgradeState;
    //public bool IsPaused => currentState is PauseState;
    //public State CurrentState => currentState;

    private float startTimer;
    private float winTimer;
    private float UpgradeTimer;
    private float drawTimerPeriod;

    private bool roundStarting = false;

    public bool respawn = false;

    private Dictionary<int, int> playerWins = new Dictionary<int, int>();
    [SerializeField] private int winsToEnd = 3;
      public bool matchOver = false;
    private void Awake()
    {
        Instance = this;

    }

    private void Start()
    {
        SetState(startState);
        startTimer = maxStartTimer;
        winTimer = maxWinTimer;
        UpgradeTimer = maxUpgradeTimer;
        drawTimerPeriod = maxDrawTimer;

    }



    private void Update()
    {

        if (currentState == startState)
        {
            if (PlayerInputManager.instance.gameStartable)
            {
                startTimer -= Time.deltaTime;

                if (startTimer <= 0 && !roundStarting)
                {
                    roundStarting = true;
                    StartCoroutine(BeginRound());
                }
            }
        }
        else if (currentState == gameplayState)
        {
            if(alivePlayers.Count <= 1)
            {
                drawTimerPeriod -= Time.deltaTime;
            }



            if(alivePlayers.Count == 0 && drawTimerPeriod <= 0)
            {
                SetWinnerText("DRAW");
                SetState(wonState);

                Debug.Log("DRAW");
            }
            else if(alivePlayers.Count == 1 && drawTimerPeriod <= 0)
            {
                SetWinnerText($"{alivePlayers[0].name} WON!!");
                int winnerIndex = spawnedPlayers.IndexOf(alivePlayers[0]);
                AddWinToPlayer(winnerIndex);
                SetState(wonState);

                Debug.Log("WON");
            }


        }
        else if (currentState == wonState)
        {
            winTimer -= Time.deltaTime;

            if (winTimer <= 0)
            {
               matchOver = playerWins.Values.Any(wins => wins >= winsToEnd);
                if(!matchOver)
                {
                SetState(upgradeState);
                Debug.Log("UPGRADE");
                }
                else
                {
                  SetState(startState);
                }
            }
        }
        else if (currentState == upgradeState)
        {
            UpgradeTimer -= Time.deltaTime;
            if (UpgradeTimer <= 0)
            {
                UpgradeManager.Instance.ForceResolve();
            }
        }

    }

    public void PlacePlayersAtSpawnPoints(PlayerController spawnedPlayer,Vector2 point)
    {
        spawnedPlayer.GetComponent<HealthSystem>().Respawn(point);
    }

    public void SetState(State newState)
    {

        if (currentState == newState)
            return;

        currentState = newState;
        if (currentState == wonState)
        {
            winTimer = maxWinTimer;
            drawTimerPeriod = maxDrawTimer;
        }
        if (currentState == upgradeState)
            UpgradeTimer = maxUpgradeTimer;

        currentState.Enter(this);

        Debug.Log($"current state - {currentState.ToString()}");
    }

    private IEnumerator BeginRound()
    {
        countdownUI.SetActive(true);

        countdownCanvasGroup.alpha = 1f;

        string[] countdown = { "3", "2", "1", "FIGHT" };

        for (int i = 0; i < countdown.Length; i++)
        {
            countdownText.text = countdown[i];

            countdownCanvasGroup.alpha = 1f;
            if (countdown[i] != "FIGHT")
            {
                yield return new WaitForSeconds(numberDuration);
            }
            else
            {
                yield return new WaitForSeconds(fightDuration);

                SetState(gameplayState);

                float timer = 0f;

                while (timer < fadeDuration)
                {
                    timer += Time.deltaTime;

                    countdownCanvasGroup.alpha =
                        Mathf.Lerp(1f, 0f, timer / fadeDuration);

                    yield return null;
                }
            }
        }

        countdownUI.SetActive(false);

        foreach(PlayerController player in spawnedPlayers) //spawns each players decks at the start of the round
        {
            player.GetComponent<Deck>()?.DrawHand();
        }

        SetState(gameplayState);

        roundStarting = false;
    }


    public void TogglePause()
    {
        if (currentState is PauseState)
            SetState(gameplayState);
        else if (currentState is GameplayState)
            SetState(pauseState);
    }

    public void GoToGameplay()
    {
        SetState(gameplayState);
    }

    public void GoToStart()
    {
        startTimer = maxStartTimer;
        
       
        if(matchOver)
            {
             for (int i = 0; i < spawnedPlayers.Count; i++)
                {
                    playerWins[i] = 0;
                    spawnedPlayers[i].GetComponent<Deck>().ResetDeck();
                    spawnedPlayers[i].GetComponent<Deck>().Initialise(i);
                    Debug.Log($"Player {i} wins reset to 0");
                }
                    matchOver = false;
            }
        

        SetState(startState);

    }

    public void SetWinnerText(string text)
    {
        var playertext = WonUI.GetComponentInChildren<TextMeshProUGUI>();
        playertext.text = text;
    }


    public void RegisterPlayer(int playerIndex)
    {
        if(!playerWins.ContainsKey(playerIndex))
        {
            playerWins.Add(playerIndex, 0);
        }
    }

    public int GetPlayerScore(int playerIndex)
    {
        if(playerWins.TryGetValue(playerIndex, out int score))
        {
            return score;
        }
        return 0;
    }

    private void AddWinToPlayer(int playerIndex)
    {
        if(playerWins.ContainsKey(playerIndex))
        {
            playerWins[playerIndex]++;
            Debug.Log($"Player {playerIndex} wins: {playerWins[playerIndex]}");
        }

        if(playerWins[playerIndex] >= winsToEnd)
        {
            SetWinnerText($"Player {playerIndex} Wins the Game!");
            SetState(wonState);
            respawn = false;

        }
    }
}
