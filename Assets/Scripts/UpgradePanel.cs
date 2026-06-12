using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UpgradePanel : MonoBehaviour
{
    [SerializeField] private List<UIOption> cardOptions = new List<UIOption>();
    public int PlayerIndex;

    private List<ScriptableCard> currentOffer = new List<ScriptableCard>();
    public PlayerInput OwnerInput;

    public int CurrentSelection;
    
    public UINavigation navigation;


    public bool IsConfirmed { get; private set; }
    public int SelectedCardIndex { get; private set; }

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
        currentOffer = CardOfferSystem.Instance.GenerateOffer(player);

        for (int i = 0; i < cardOptions.Count; i++)
        {
            cardOptions[i].gameObject.SetActive(true);
            if(i < currentOffer.Count)
            {
               cardOptions[i].SetCard(currentOffer[i]);
                int index = i; 
                cardOptions[i].OnConfirm = () => LockIn(index);
                cardOptions[i].OnCancel = Cancel;
                cardOptions[i].GetComponent<CardUI>().getCardImage().canvasRenderer.SetAlpha(1f);

            }
            else
            {
                cardOptions[i].GetComponent<CardUI>().getCardImage().canvasRenderer.SetAlpha(1f);
            }
        }


        navigation.Initialize(cardOptions);
        navigation.InitializeWithPlayerInput(input);


    }

    private void LockIn(int cardIndex)
    {

        SelectedCardIndex = cardIndex;
        IsConfirmed = true;

        UpgradeManager.Instance.PlayerLockedIn(PlayerIndex);

        for (int i = 0; i < cardOptions.Count; i++)
        {
            cardOptions[i].GetComponent<CardUI>().getCardImage().canvasRenderer.SetAlpha(0.5f);
        }


    }

    private void Cancel()
    {
        if (!IsConfirmed) return;
        IsConfirmed = false;
        navigation.Reset();
        UpgradeManager.Instance.PlayerUnLocked(PlayerIndex);

        for (int i = 0; i < cardOptions.Count; i++)
        {
            cardOptions[i].GetComponent<CardUI>().getCardImage().canvasRenderer.SetAlpha(1f);
        }

    }

    public ScriptableCard GetSelectedCard()
    {
        return cardOptions[SelectedCardIndex].CardData;
    }

    public void AssignRandomCard()
    {
        int randomIndex = Random.Range(0, currentOffer.Count);
        SelectedCardIndex = randomIndex;
        IsConfirmed = true;

        for (int i = 0; i < cardOptions.Count; i++)
        {
            cardOptions[i].SetHighlighted(i == SelectedCardIndex);
        }
    }

}
