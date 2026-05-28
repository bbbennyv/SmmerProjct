using System;
using UnityEngine;

public class UIOption : MonoBehaviour
{
    public Action OnConfirm;
    public Action OnCancel;

    [SerializeField] private GameObject highlightIndicator;

    [SerializeField] private CardUI cardUI;
    public ScriptableCard CardData {get; private set; }

    public void SetHighlighted(bool highlighted)
    {
        highlightIndicator.SetActive(highlighted);
    }

    public void SetCard(ScriptableCard data)
    {
        CardData = data;
        cardUI.SetCardUI(data);
    }
}