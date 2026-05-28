using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class UpgradeManager : MonoBehaviour
{
   public static UpgradeManager Instance {get; private set; }

   [SerializeField] private List<UpgradePanel> upgradePanels = new();

   private int lockedInCount = 0;

   private void Awake()
   {
    if( Instance != null && Instance != this) {Destroy(gameObject); return; }
    Instance = this;
   }

   public void StartUpgradePhase()
   {
    lockedInCount = 0;

    // List<PlayerController> players = GameManager.Instance.spawnedPlayers;

    // for(int i = 0; i < players.Count; i++)
    // {
    //     PlayerInput input = players[i].GetComponent<PlayerInput>();
    //     upgradePanels[i].gameObject.SetActive(true);
    //     upgradePanels[i].LockedIn = false;
    //     upgradePanels[i].Initialize(input, i);
    // }
    PlayerInputManager.instance.InitialiseUpgradePanels();
   }

   public void PlayerLockedIn(int playerIndex)
   {
    lockedInCount++;
    
    PlayerInputManager.instance.UpgradePanelUI[playerIndex].gameObject.SetActive(false);
    
    if(lockedInCount >= GameManager.Instance.spawnedPlayers.Count)
    {
        GameManager.Instance.respawn = true;
    } 

   }
}
