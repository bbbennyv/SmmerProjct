using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// <Summary>
//Holds all data for each individual  card
//<Summary>

[CreateAssetMenu(menuName = "CardData")] // lets you crate A New CARDdATA OBJECT 
public class ScriptableCard : ScriptableObject
{
    [field: SerializeField] public string CardName {get; private set;}
    [field: SerializeField, TextArea] public string CardDescription {get; private set;} //TextArea makes an input field in the inspector to write lonnger text
    [field: SerializeField] public int PlayCost {get; private set;}
    [field: SerializeField] public Sprite Image {get; private set;}
    [field: SerializeField] public CardEffect Effect {get; private set;}
   

    

    public enum CardElement
    {
        Basic, 
        Ice,
        Fire,
        Lightning
    }

     public enum CardEffectType
    {
        Melee, 
        Range,
        Magic
    }

     public enum CardRarity
    {
        Basic, 
        Common, 
        Rare,
        Legendary
    }


    public void Use(GameObject user)
    {
        Effect?.Execute(user);
    }

}
