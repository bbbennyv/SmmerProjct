using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class HandController : MonoBehaviour
{
    #region Fields and Properties

    [SerializeField] private float selectedScale = 1.0f;
    [SerializeField] private float neighbourScale = 0.6f;
    [SerializeField] private float scaleSpeed = 8f;

    [SerializeField] private float cardSpacing = 150f;
    [SerializeField] private float moveSpeed = 8f;

    private int  selectedIndex = 0;
    private Deck deck;

    #endregion


    #region Methods

    private void Awake()
    {
        deck = GetComponent<Deck>();
    }

    private void Update()
    {
        if(deck.HandCards.Count == 0) return;
        UpdateCardVisibility();
        UpdateCardScales();
        UpdateCardPositions();
    }


    public void ScrollLeft(InputAction.CallbackContext action)
    {
        if(!action.started) return;
        selectedIndex = Mathf.Max(0, selectedIndex - 1);
    }

    public void ScrollRight(InputAction.CallbackContext action)
    {
         if(!action.started) return;
        selectedIndex = Mathf.Min(deck.HandCards.Count - 1, selectedIndex + 1);
    }

    public void UseCard(InputAction.CallbackContext action)
    {
        if (!GameManager.Instance.IsGameplay)
            return;


        if(!action.started) return;
        var hand = deck.HandCards;
        if(hand.Count == 0) return;

        Card selected = hand[selectedIndex];
        selected.CardData.Use(gameObject);
        deck.DiscardCard(selected);

        selectedIndex = Mathf.Clamp(selectedIndex, 0, Mathf.Max(0, hand.Count - 1));

        SoundManager.PlaySound(SoundType.CardUse, 1f);
    }

    private void UpdateCardVisibility()
    {
        var hand = deck.HandCards;
        for(int i = 0; i < hand.Count; i++)
        {
            bool isVisible = Mathf.Abs(i - selectedIndex) <= 1;
            hand[i].gameObject.SetActive(isVisible);
        }
    }

    private void UpdateCardScales()
    {
        var hand = deck.HandCards;
        for(int i = 0; i < hand.Count; i++)
        {
            if(!hand[i].gameObject.activeSelf) continue;

            float targetScale = i == selectedIndex ? selectedScale : neighbourScale;
            Transform t = hand[i].transform;
            t.localScale = Vector3.Lerp(t.localScale, Vector3.one * targetScale, Time.deltaTime * scaleSpeed);
        }
    }

    private void UpdateCardPositions()
    {
        var hand = deck.HandCards;
        for(int i = 0; i < hand.Count; i++)
        {
            if(!hand[i].gameObject.activeSelf) continue;

            int offset = i - selectedIndex;
            Vector2 targetPosition = new Vector2(offset * cardSpacing, 0f);

            RectTransform rt  = hand[i].GetComponent<RectTransform>();
            rt.anchoredPosition = Vector2.Lerp(rt.anchoredPosition, targetPosition, Time.deltaTime * moveSpeed);
        }
    }

    #endregion
}
