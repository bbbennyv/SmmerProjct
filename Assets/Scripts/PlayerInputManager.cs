using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
public class PlayerInputManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private List<GameObject> readyText = new List<GameObject>(4);

    private HashSet<Gamepad> joinedGamepads = new HashSet<Gamepad>();

    public static PlayerInputManager instance;

    public bool gameStartable = false;

    private void Awake()
    {
        instance = this;
        spawn = 0;
    }

    private void Start()
    {
        foreach (var gamepad in readyText)
        {

            gamepad.gameObject.SetActive(false);

        }

    }

    private int spawn;
    void Update()
    {
        foreach (var gamepad in Gamepad.all)
        {
            if (gamepad.buttonSouth.wasPressedThisFrame && spawn < spawnPoints.Length && !joinedGamepads.Contains(gamepad))
            {
                var player = PlayerInput.Instantiate(playerPrefab, controlScheme: "Gamepad", pairWithDevice: gamepad);
                player.transform.position =  spawnPoints[spawn].position;
                readyText[spawn].SetActive(true);
                player.name = $"Player {spawn + 1}";
                spawn++;
                joinedGamepads.Add(gamepad);

                var controller = player.GetComponent<PlayerController>();

                GameManager.Instance.spawnedPlayers.Add(controller);
                GameManager.Instance.alivePlayers.Add(controller);

                if (spawn > 1)
                {
                    gameStartable = true;
                    Debug.Log("STartable");
                }
            }


        }

        if (GameManager.Instance.respawn)
        {
            GameManager.Instance.alivePlayers.Clear();
            GameManager.Instance.respawn = false;
            spawn = 0;
            foreach (var player in GameManager.Instance.spawnedPlayers)
            {
                GameManager.Instance.PlacePlayersAtSpawnPoints(player, spawnPoints[spawn]);
                GameManager.Instance.alivePlayers.Add(player);
                spawn++;
            }
            GameManager.Instance.GoToGameplay();
        }
    }
}
