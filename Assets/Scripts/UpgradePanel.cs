using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UpgradePanel : MonoBehaviour
{
    [SerializeField] private List<UIOption> cardOptions = new List<UIOption>();
    [SerializeField] private float upgradeInputDelay = 0.5f;
    public int PlayerIndex;

    public PlayerInput OwnerInput;

    public bool LockedIn;

    public int CurrentSelection;
    
    public UINavigation navigation;

    private bool canAcceptInput;
    private bool waitingForRelease;
    private void Awake()
    {
        if(navigation == null)
            navigation = GetComponent<UINavigation>();
    }

    public void Initialize(PlayerInput input, int playerIndex)
    {

        OwnerInput = input;

        PlayerIndex = playerIndex;

        PlayerController player = input.GetComponent<PlayerController>();
        List<ScriptableCard> offer = CardOfferSystem.Instance.GenerateOffer(player);

        for (int i = 0; i < cardOptions.Count; i++)
        {
            cardOptions[i].gameObject.SetActive(true);
            if(i < offer.Count)
            {
               cardOptions[i].SetCard(offer[i]);
                int index = i; 
                cardOptions[i].OnConfirm = () => LockIn(index);
            }
            else
            {
                cardOptions[i].gameObject.SetActive(false);
            }
        }


        navigation.InitializeWithPlayerInput(input);
        navigation.Initialize(cardOptions);

        StartCoroutine(EnableInputRoutine());

    }

    private IEnumerator EnableInputRoutine()
    {
        canAcceptInput = false;
        waitingForRelease = true;

        yield return new WaitForSeconds(upgradeInputDelay);

        canAcceptInput = true;
    }

    public bool CanAcceptInput()
    {
        if (!canAcceptInput)
            return false;

        if (!waitingForRelease)
            return true;

        Gamepad gamepad = OwnerInput.devices[0] as Gamepad;

        if (gamepad == null)
        {
            waitingForRelease = false;
            return true;
        }

        bool anyButtonHeld =
            gamepad.buttonSouth.isPressed ||
            gamepad.buttonNorth.isPressed ||
            gamepad.buttonEast.isPressed ||
            gamepad.buttonWest.isPressed ||
            gamepad.dpad.up.isPressed ||
            gamepad.dpad.down.isPressed ||
            gamepad.dpad.left.isPressed ||
            gamepad.dpad.right.isPressed;

        if (!anyButtonHeld)
        {
            waitingForRelease = false;
            return true;
        }

        return false;
    }

    private void LockIn(int cardIndex)
    {
        if (LockedIn) return;
        LockedIn = true;

        PlayerController player = OwnerInput.GetComponent<PlayerController>();
        Deck deck = OwnerInput.GetComponent<Deck>();
        deck.AddCard(cardOptions[cardIndex].CardData);

        UpgradeManager.Instance.PlayerLockedIn(PlayerIndex);

        for (int i = 0; i < cardOptions.Count; i++)
        {
             cardOptions[i].gameObject.SetActive(false);
        }

    }

}
