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
        if(navigation != null)
            navigation = GetComponent<UINavigation>();

        Debug.Log($"{navigation} ");

    }

    public void Initialize(PlayerInput input, int playerIndex)
    {
        OwnerInput = input;
        PlayerIndex = playerIndex;

        for (int i = 0; i < cardOptions.Count; i++)
        {
            int index = i; 
            cardOptions[i].OnConfirm = () => LockIn(index);
        }

        navigation.InitializeWithPlayerInput(input);
        navigation.Initialize(cardOptions);
    }

    private void LockIn(int cardIndex)
    {
        if (LockedIn) return;
        LockedIn = true;

        

        // TODO: OwnerInput.GetComponent<Deck>().AddCard(cardOptions[cardIndex].CardData);
        // TODO: UpgradeManager.Instance.PlayerLockedIn(PlayerIndex);
    }

}
