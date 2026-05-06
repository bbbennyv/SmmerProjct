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
                spawn++;
                joinedGamepads.Add(gamepad);
                if(spawn > 1)
                {
                    gameStartable = true;
                    Debug.Log("STartable");
                }
            }
        } 
    }
}
