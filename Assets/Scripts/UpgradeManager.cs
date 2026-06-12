using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.VirtualTexturing;

public class UpgradeManager : MonoBehaviour
{
   public static UpgradeManager Instance {get; private set; }

   [SerializeField] private List<UpgradePanel> upgradePanels = new();

    private HashSet<int> lockedInPlayers = new HashSet<int>();
    private int lockedInCount = 0;

    private bool resolved = false;

    private void Awake()
   {
        if( Instance != null && Instance != this) {Destroy(gameObject); return; }
        Instance = this;
   }

   public void StartUpgradePhase()
   {
        lockedInPlayers.Clear();
        resolved = false;
        PlayerInputManager.instance.InitialiseUpgradePanels();
   }

   public void PlayerLockedIn(int playerIndex)
   {
        lockedInPlayers.Add(playerIndex);

        //PlayerInputManager.instance.UpgradePanelUI[playerIndex].gameObject.SetActive(false);

        if (lockedInPlayers.Count >= GameManager.Instance.spawnedPlayers.Count)
        {
            ResolveUpgrades();
        }

   }

    public void ResolveUpgrades()
    {
        if (resolved) return;
        resolved = true;

        var panels = PlayerInputManager.instance.UpgradePanelUI;
        int playerCount = GameManager.Instance.spawnedPlayers.Count;

        for (int i = 0; i < playerCount; i++)
        {
            var panel = panels[i];
            if (!panel.IsConfirmed) continue;

            var card = panel.GetSelectedCard();
            if (card == null) continue;

            var deck = panel.OwnerInput.GetComponent<Deck>();
            if (deck == null) continue;

            deck.AddCard(card);
        }

        GameManager.Instance.respawn = true;
    }

    public void PlayerUnLocked(int playerIndex)
    {
        lockedInPlayers.Remove(playerIndex);
    }

    public void ForceResolve()
    {
        if (resolved) return;

        var panels = PlayerInputManager.instance.UpgradePanelUI;
        int playerCount = GameManager.Instance.spawnedPlayers.Count;

        for (int i = 0; i < playerCount; i++)
        {
            if (!panels[i].IsConfirmed)
                panels[i].AssignRandomCard();
        }

        ResolveUpgrades();
    }

}
