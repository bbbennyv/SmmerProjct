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

    public List<Card> _deckPile = new();
    public List<Card> _discardPile = new();

    [field:SerializeField] public List<Card> HandCards { get; private set; } = new();


    #endregion

    #region Methods

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InstantiateDeck();
    }

    private void InstantiateDeck()
    {
        for (int i = 0; i < _playerDeck.CardsInCollection.Count; i++)
        {
        Card card = Instantiate(_cardPrefab, _cardCanvas.transform); //instantiate the card prefab as child of the card canvas
        card.SetUp(_playerDeck.CardsInCollection[i]);
        _deckPile.Add(card);
        card.gameObject.SetActive(false); //we will later activare the cards when we draw them, for bnow we just want to build the pool
        }
    }


    private void ShuffleDeck()
    {

    }

    public void DrawHand(int amount = 2)
    {
        for (int i  = 0; i < amount; i++)
        {
            if(_deckPile.Count <= 0)
            {
                _discardPile = _deckPile;
                _discardPile.Clear();

            }
            HandCards.Add(_deckPile[0]);
            _deckPile[0].gameObject.SetActive(true);
            _deckPile.RemoveAt(0);
        }
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
    #endregion
}
