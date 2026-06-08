using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UpgradePanel : MonoBehaviour
{
    [SerializeField] private List<UIOption> cardOptions = new List<UIOption>();
    public int PlayerIndex;

    public PlayerInput OwnerInput;

    public bool LockedIn;

    public int CurrentSelection;
    
    public UINavigation navigation;


    private void Awake()
    {
        if(navigation == null)
            navigation = GetComponent<UINavigation>();

        Debug.Log($"{navigation} ");

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

        // TODO: OwnerInput.GetComponent<Deck>().AddCard(cardOptions[cardIndex].CardData);
        // TODO: UpgradeManager.Instance.PlayerLockedIn(PlayerIndex);
    }

}
