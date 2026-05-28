using UnityEngine;
using System.Collections.Generic;

public class CardOfferSystem : MonoBehaviour
{
    public static CardOfferSystem Instance {get; private set; }

    [SerializeField] private ClassCardCollection[] allClassCollections;
    [SerializeField] private int offerSize = 3;

   // private ClassCardCollection activeCollection;

    private void Awake()
    {
        if(Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }
   
//    public void Initialise(PlayerClasses playerClass)
//    {
//     foreach(var collection in allClassCollections)
//     {
//         if(collection.Class == playerClass)
//         {
//             activeCollection = ScriptableObject.CreateInstance<ClassCardCollection>();
//             return;
//         }
//     }
//      Debug.LogError($"No CardCollection found for class: {playerClass}");
//    }

public void ValidateCollections()
{
    foreach (PlayerClasses playerClass in System.Enum.GetValues(typeof(PlayerClasses)))
    {
        if(GetCollectionForClass(playerClass) == null)
        Debug.LogError($"No cardCollection foundf for class: {playerClass}");
    }
}



    public List<ScriptableCard> GenerateOffer(PlayerController player)
    {
        ClassCardCollection collection = GetCollectionForClass(player.playerClass);
        if(collection == null)
        {
             Debug.LogError("CardOfferSystem not initialised — call Initialise(playerClass) first.");
            return new List<ScriptableCard>();
        }

        List<ScriptableCard> pool = new();
        pool.AddRange(collection.ClassCards.CardsInCollection);
        pool.AddRange(collection.NeutralCards.CardsInCollection);

        if(pool.Count < offerSize)
        Debug.LogWarning($"Pool only has {pool.Count} cards - offer will be smaller than {offerSize}.");

        List<ScriptableCard> offer = new();
        int count = Mathf.Min(offerSize, pool.Count);

        for(int i = 0; i < count; i++)
        {
            int index = Random.Range(0, pool.Count);
            offer.Add(pool[index]);
            pool.RemoveAt(index);
        }
        return offer;
    }



    private ClassCardCollection GetCollectionForClass(PlayerClasses playerClass)
    {
        foreach (var collection in allClassCollections)
        if(collection.PlayerClass == playerClass) return collection;

        return null;
    }








}


