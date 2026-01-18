using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VillainByMistake;
using VillainByMistake.Cards;

public class DeckManager : MonoBehaviour
{
    public List<Card> allCards = new List<Card>();

    private int currentIndex = 0;

    void Start()
    {
        //Load all cards from Resources folder (our premade cards are there)
        Card[] cards = Resources.LoadAll<Card>("Cards");

        //Add the loaded cards to the allCards list
        allCards.AddRange(cards);
    } 

    public void DrawCard(HandManager handManager){
        if (allCards.Count == 0)
            return;
        
        Card nextCard = allCards[currentIndex];
        handManager.AddCardToHand(nextCard);
        currentIndex = (currentIndex + 1) % allCards.Count;
        //Logic so that next card is drawn from the top of the deck        
    }
    
}
