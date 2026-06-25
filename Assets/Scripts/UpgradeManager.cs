using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.VirtualTexturing;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance {get; private set; }

    [SerializeField] private List<UpgradePanel> upgradePanels = new();
    [SerializeField] private float maxlockedInCount = 3;

    private HashSet<int> lockedInPlayers = new HashSet<int>();

    private float lockedInCount;

    private bool resolved = false;
    private bool allLockedIn = false;
    private void Awake()
    {
        if( Instance != null && Instance != this) {Destroy(gameObject); return; }
        Instance = this;
    }
    public void StartUpgradePhase()
    {
        lockedInPlayers.Clear();
        resolved = false;
        allLockedIn = false;
        PlayerInputManager.instance.InitialiseUpgradePanels();
    }

   public void PlayerLockedIn(int playerIndex)
   {
        lockedInPlayers.Add(playerIndex);

        if (lockedInPlayers.Count >= GameManager.Instance.spawnedPlayers.Count)
        {
            allLockedIn = true;
            lockedInCount = maxlockedInCount;
        }

   }


    private void Update()
    {
        if (resolved) return;

        if (allLockedIn)
        {
            lockedInCount -= Time.deltaTime;
            if (lockedInCount <= 0f)
            {
                ResolveUpgrades();
            }
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
        allLockedIn = false;

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
