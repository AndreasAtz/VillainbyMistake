using UnityEngine;
using System.Collections.Generic;
using VillainByMistake.Cards;

public class HandDisplay : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Transform cardContainer;
    [SerializeField] private PlayerController playerController;
    
    private List<GameObject> displayedCards = new List<GameObject>();
    
    private void Start()
    {
        if (playerController == null)
            playerController = GameManager.Instance.Player;
        
        playerController.OnHandUpdated.AddListener(UpdateHandDisplay);
        UpdateHandDisplay();
    }
    
    public void UpdateHandDisplay()
    {
        // Clear old cards
        foreach (GameObject cardObj in displayedCards)
            Destroy(cardObj);
        displayedCards.Clear();
        
        // Create new cards
        foreach (Card card in playerController.Hand)
        {
            GameObject cardObj = Instantiate(cardPrefab, cardContainer);
            CardDisplay display = cardObj.GetComponent<CardDisplay>();
            
            if (display != null)
            {
                display.cardData = card;
                display.UpdateCardDisplay();
                
                // Add the click handler for the cards, we dont use this anymore, but in for next semester
                CardClickHandler clickHandler = cardObj.GetComponent<CardClickHandler>();
                if (clickHandler == null)
                    clickHandler = cardObj.AddComponent<CardClickHandler>();
                
                clickHandler.SetCard(card);
            }
            
            displayedCards.Add(cardObj);
        }
    }
}