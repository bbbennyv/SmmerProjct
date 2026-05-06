using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// <Summary>
//Defines what a card is and can be, will connect all data and behaviours 
//<Summary>

[RequireComponent(typeof(CardUI))] // will automatically attach the card ui script to every object that is a card
[RequireComponent(typeof(CardMovement))] // will handle everything to do wirth percieved card movement
public class Card : MonoBehaviour
{
   #region Fields and properties 
    [field: SerializeField] public ScriptableCard CardData { get; private set; }

   #endregion


   #region Methods

    public void SetUp(ScriptableCard data)
    {
        CardData = data;
        GetComponent<CardUI>().SetCardUI();
    }


   #endregion
}
