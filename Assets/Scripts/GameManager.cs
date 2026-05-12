using NUnit.Framework;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("UI Ref")]
    [SerializeField] private GameObject startUI;
    [SerializeField] private GameObject wonUI;/*
    [SerializeField] private GameObject pauseUI;*/
    [SerializeField] private GameObject upgradeUI;
    [SerializeField] public List<PlayerController> spawnedPlayers;
    [SerializeField] public List<PlayerController> alivePlayers;

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

    //public bool IsPaused => currentState is PauseState;
    //public State CurrentState => currentState;

    private float startTimer = 3;
    private float winTimer = 3;
    private float UpgradeTimer = 3;
    public bool respawn = false;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SetState(startState);
    }



    private void Update()
    {
        //.Log($"CURRENTSTATE - { currentState}");

        if (currentState == startState)
        {
            if (PlayerInputManager.instance.gameStartable)
            {
                startTimer -= Time.deltaTime;

                if (startTimer <= 0)
                {
                    SetState(gameplayState);

                    Debug.Log("GAMEPLAY");
                }
            }
        }
        else if (currentState == gameplayState)
        {
            if (alivePlayers.Count <= 0)
            {
                SetWinnerText("DRAW");
                SetState(wonState);

                Debug.Log("DRAW");
            }

            else if (alivePlayers.Count == 1)
            {
                SetWinnerText($"{alivePlayers[0].name} WON!!");
                SetState(wonState);

                Debug.Log("WON");
            }
        }
        else if (currentState == wonState)
        {
            winTimer -= Time.deltaTime;

            if (winTimer <= 0)
            {
                SetState(upgradeState);
                Debug.Log("UPGRADE");
            }
        }
        else if (currentState == upgradeState)
        {
            UpgradeTimer -= Time.deltaTime;
            if (UpgradeTimer <= 0)
            {
                respawn = true;
                Debug.Log("RESPAWN");
            }
        }

    }

    public void PlacePlayersAtSpawnPoints(PlayerController spawnedPlayer,Transform point)
    {
        spawnedPlayer.GetComponent<HealthSystem>().Respawn(point);
    }

    public void SetState(State newState)
    {

        if (currentState == newState)
            return;

        currentState = newState;

        if (currentState == wonState)
            winTimer = 3f;

        if (currentState == upgradeState)
            UpgradeTimer = 3f;

        currentState.Enter(this);
        Debug.Log($"current state - {currentState.ToString()}");
    }


    //if need be these are here
  /*
    public void GoToPause()
    {
        SetState(pauseState);
    }

  */

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

    public void SetWinnerText(string text)
    {
        var playertext = WonUI.GetComponentInChildren<TextMeshProUGUI>();
        playertext.text = text;
    }
}
