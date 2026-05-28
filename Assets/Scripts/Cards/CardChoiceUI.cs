using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class CardChoiceUI : MonoBehaviour
{
   public static CardChoiceUI Instance {get; private set; }

   [SerializeField] private CardChoiceSlot[] slots;
   [SerializeField] private GameObject panel;
   [SerializeField] private TextMeshProUGUI playerLabel;

   [SerializeField] private float selectedScale = 1.2f;
   [SerializeField] private float unselectedScale = 0.85f;
   [SerializeField] private float scaleSpeed = 8f;

   private Queue<(PlayerController player, List<ScriptableCard> offer, Deck deck)> queue = new();
   private int selectedIndex = 0;

   private void Awake()
    {
        if(Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        panel.SetActive(false);

        
    }
    
    private void Update()
    {
        if(!panel.activeSelf) return;
        UpdateSlotScales();
    }

    public void ShowForPlayer(PlayerController player, List<ScriptableCard> offer, Deck deck)
    {
        queue.Enqueue((player, offer, deck)); 
        if(queue.Count == 1)
        ShowNext();
    }

    private void ShowNext()
    {
        if(queue.Count == 0)
        {
            panel.SetActive(false);
            GameManager.Instance.GoToStart();
            return;
        }

        selectedIndex = 0;
        var (player, offer, _) = queue.Peek();

        panel.SetActive(true);
        playerLabel.text = $"{player.name} - Pic k a card";

        for(int i =0; i < slots.Length; i++)
        {
            if(i < offer.Count)
            slots[i].SetUp(offer[i]);
            else
            slots[i].gameObject.SetActive(false);
        }
    }

      public void ScrollLeft(UnityEngine.InputSystem.InputAction.CallbackContext action)
    {
        if (!action.started || !panel.activeSelf) return;
        selectedIndex = Mathf.Max(0, selectedIndex - 1);
    }

    public void ScrollRight(UnityEngine.InputSystem.InputAction.CallbackContext action)
    {
        if (!action.started || !panel.activeSelf) return;
        selectedIndex = Mathf.Min(slots.Length - 1, selectedIndex + 1);
    }

    public void Confirm(UnityEngine.InputSystem.InputAction.CallbackContext action)
    {
        if (!action.started || !panel.activeSelf) return;

        var (_, offer, deck) = queue.Dequeue();
        deck.AddCard(offer[selectedIndex]);
        ShowNext();
    }

private void UpdateSlotScales()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (!slots[i].gameObject.activeSelf) continue;

            float target = i == selectedIndex ? selectedScale : unselectedScale;
            Transform t = slots[i].transform;
            t.localScale = Vector3.Lerp(t.localScale, Vector3.one * target, Time.deltaTime * scaleSpeed);
        }
    }



}
