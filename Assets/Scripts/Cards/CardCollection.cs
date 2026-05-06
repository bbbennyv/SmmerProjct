using UnityEngine;
using System.Collections;
using System.Collections.Generic;


// <Summary>
//A generic collection of card data objects. can be used as a deck or booster for example.
//<Summary>

[CreateAssetMenu(menuName = "Card Collection")]
public class CardCollection : ScriptableObject
{
    [field: SerializeField] public List<ScriptableCard> CardsInCollection {get; private set; }
    
    public void RemoveCardFromCOllection(ScriptableCard card)
    {
        if (CardsInCollection.Contains(card))
        {
            CardsInCollection.Remove(card);
        }
        else
        {
            Debug.LogWarning("cardData is not present in collection - cannot remove");
        }

    }

    public void AddCardToCollection(ScriptableCard card)
    {
        CardsInCollection.Add(card);

    }
    


}

