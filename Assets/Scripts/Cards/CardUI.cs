using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class CardUI : MonoBehaviour
{

 #region Fields and properties 
   private Card _card;

   [Header("prefab Elements")] // references from objects in the card prefab
   [SerializeField] private Image _cardImage;
  

  
    [SerializeField] private TextMeshProUGUI _cardName;
    [SerializeField] private TextMeshProUGUI _cardDescription;

    [Header("Sprite Assets")] //references to the art folder in assets
    [SerializeField] private Sprite _warriorClass;
    [SerializeField] private Sprite _mageClass;


   #endregion


   #region Methods

   private void Awake()
   {
    _card = GetComponent<Card>();
    if(_card!= null)
    SetCardUI();
   }

   private void OnValidate()
   {
    Awake();
   }

    public void SetCardUI()
    {
        if(_card != null && _card.CardData != null)
        {
            // SetCardText();
            // SetCardImage();
            SetCardUI(_card.CardData);
        }
    }

    private void SetCardText()
    {
        _cardName.text = _card.CardData.CardName;
        _cardDescription.text = _card.CardData.CardDescription;
    }

    


    private void SetCardImage()
    {
        _cardImage.sprite = _card.CardData.Image;
    }

    public Image getCardImage()
    {
        return _cardImage;
    }

    public void SetCardUI(ScriptableCard data)
    {
        if(data == null) return;
         _cardName.text = data.CardName;
        _cardDescription.text = data.CardDescription;
         _cardImage.sprite = data.Image;
    }
   #endregion
}
