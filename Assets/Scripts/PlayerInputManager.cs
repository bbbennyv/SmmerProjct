using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
public class PlayerInputManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject[] MapPrefab;
    [SerializeField] private List<Vector2> spawnPoints;
    [SerializeField] private List<GameObject> readyText = new List<GameObject>(4);

    private HashSet<Gamepad> joinedGamepads = new HashSet<Gamepad>();
    private GameObject spawnedMap;

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

        SpawnRandomMap();

    }

    private int spawn;
    void Update()
    {
        foreach (var gamepad in Gamepad.all)
        {
            if (gamepad.buttonSouth.wasPressedThisFrame && spawn < spawnPoints.Count && !joinedGamepads.Contains(gamepad))
            {
                var player = PlayerInput.Instantiate(playerPrefab, controlScheme: "Gamepad", pairWithDevice: gamepad);
                player.transform.position =  spawnPoints[spawn];
                readyText[spawn].SetActive(true);
                player.name = $"Player {spawn + 1}";
              //  player.transform.position = spawnPoints[spawn].position;
                
                player.GetComponent<Deck>().Initialise(spawn);

                joinedGamepads.Add(gamepad);

                var controller = player.GetComponent<PlayerController>();

                var controllerText = controller.gameObject.GetComponentInChildren<TextMeshProUGUI>();
                controllerText.text = $"Player {spawn + 1}";
                readyText.Add(controllerText.gameObject);

                GameManager.Instance.spawnedPlayers.Add(controller);
                GameManager.Instance.alivePlayers.Add(controller);
                spawn++;

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

            SpawnRandomMap();


            spawn = 0;
            spawnPoints = spawnPoints.OrderBy(x => Random.value).ToList();
            
            foreach (var player in GameManager.Instance.spawnedPlayers)
            {
                var playerController = player.GetComponent<PlayerController>();
                playerController.isDashing = false;
                GameManager.Instance.PlacePlayersAtSpawnPoints(player, spawnPoints[spawn]);
                GameManager.Instance.alivePlayers.Add(player);
                spawn++;

            }
            GameManager.Instance.GoToStart();
        }
    }


    private void SpawnRandomMap()
    {
        if (spawnedMap != null)
        {
            DestroyImmediate(spawnedMap);
        }

        spawnPoints.Clear();

        int randomIndex = Random.Range(0, MapPrefab.Length);
        
        spawnedMap = Instantiate(MapPrefab[randomIndex]);
        
        Transform spawnPointsParent = spawnedMap.transform
            .GetComponentsInChildren<Transform>(true)
            .FirstOrDefault(t => t.name == "SpawnPoints");

        if (spawnPointsParent == null)
        {
            Debug.LogError("No SpawnPoints object found.");
            return;
        }

        foreach (Transform child in spawnPointsParent)
        {
            spawnPoints.Add(child.position);
        }

        Debug.Log($"Spawned map: {spawnedMap.name}");
    }

}
