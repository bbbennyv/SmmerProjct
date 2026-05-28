using UnityEngine;
using UnityEngine.UI;
using System;

public class CardChoiceSlot : MonoBehaviour
{
    [SerializeField] private CardUI cardUI;
    
    public void SetUp(ScriptableCard data)
    {
        gameObject.SetActive(true);
        cardUI.SetCardUI(data);
    }
}
