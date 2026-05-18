using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UpgradePanel : MonoBehaviour
{
    public int PlayerIndex;

    public PlayerInput OwnerInput;

    public bool LockedIn;

    public int CurrentSelection;

    public void Initialize(PlayerInput input, int playerIndex)
    {
        OwnerInput = input;
        PlayerIndex = playerIndex;
        this.gameObject.SetActive(true);
    }
    
}
