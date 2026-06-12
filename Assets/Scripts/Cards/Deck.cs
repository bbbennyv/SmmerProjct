using UnityEngine;
using System.Collections;
using System.Collections.Generic;


// <Summary>
//Represents a single card and also governs the discard pile and works in concordance with the hand script
//singleton
//<Summary>

public class Deck : MonoBehaviour
{
   #region Fields and Properties

    public static Deck Instance {get; private set; } //Singleton
    //now we need a areference to what a deck is, a.k.a what cards it contains -> CardCollection


    [SerializeField] private CardCollection _playerDeck;
    [SerializeField] private Card _cardPrefab; // cardprefab, will use this to make copies with different data

    [SerializeField] private  Canvas _cardCanvas;

    private Transform _cardParent;
    [field:SerializeField] private List<Card> _deckPile = new();
    [field:SerializeField] private List<Card> _discardPile = new();

    [field:SerializeField] public List<Card> HandCards { get; private set; } = new();

    [SerializeField] private CardCollection upgradeDeck;
    


    #endregion

    #region Methods

    private void Awake()
    {

    }

    private void Start()
    {
        //InstantiateDeck();
    }

    public void Initialise(int playerIndex)
    {
        Canvas sharedCanvas = GameObject.FindWithTag("CardCanvas").GetComponent<Canvas>();
        _cardParent = PlayerHandUI.All[playerIndex].transform;
        InstantiateDeck();
    }

    private void InstantiateDeck()
    {
        for (int i = 0; i < _playerDeck.CardsInCollection.Count; i++)
        {
        Card card = Instantiate(_cardPrefab, _cardParent.transform); //instantiate the card prefab as child of the card canvas
        card.SetUp(_playerDeck.CardsInCollection[i]);
        _deckPile.Add(card);
        card.gameObject.SetActive(false); //we will later activare the cards when we draw them, for bnow we just want to build the pool
        }
    }


    private void ShuffleDeck()
    {
        for(int i = _deckPile.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            var temp = _deckPile[i];
            _deckPile[i] = _deckPile[j];
            _deckPile[j] = temp;
        }
    }

    public void DrawHand()
    {
        while(_deckPile.Count != 0)
        { 
            ShuffleDeck();
            HandCards.Add(_deckPile[0]);
            _deckPile[0].gameObject.SetActive(true);
            _deckPile.RemoveAt(0);
        }
            _discardPile = _deckPile;
            _discardPile.Clear(); 
    }

    public void DiscardCard(Card card)
    {
        if(HandCards.Contains(card))
        {
            HandCards.Remove(card);
            _discardPile.Add(card);
            card.gameObject.SetActive(false);
        }
    }


    public void AddCard(ScriptableCard cardData)
    {
        Card card = Instantiate(_cardPrefab, _cardParent);
         card.SetUp(cardData);
         card.gameObject.SetActive(false);
         _discardPile.Add(card);

    }

    public void ResetDeck()
    {
        foreach(var card in _deckPile)  Destroy(card.gameObject);
        foreach(var card in _discardPile)  Destroy(card.gameObject);
        foreach(var card in HandCards)  Destroy(card.gameObject);

        _deckPile.Clear();
        _discardPile.Clear();   
        HandCards.Clear();

    //     List<ScriptableCard> fullPool = new List<ScriptableCard>(_originalDeck.CardsInCollection);
    //     fullPool.AddRange(upgradeDeck.CardsInCollection);
        
    //     for(int i = fullPool.Count - 1; i > 0; i--)
    //     {
    //         int j = Random.Range(0, i + 1);
    //         var temp = fullPool[i];
    //         fullPool[i] = fullPool[j];
    //         fullPool[j] = temp;
    //     }

    // int count = Mathf.Min(baseHandSize, fullPool.Count);

    //     for(int i = 0; i < count; i++)
    //     {
    //         Card card = Instantiate(_cardPrefab, _cardParent);
    //         card.SetUp(fullPool[i]);
    //         card.gameObject.SetActive(false);
    //         _deckPile.Add(card);
    //     }
        
    }







    #endregion
}
