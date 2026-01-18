using UnityEngine;
using UnityEngine.EventSystems;
using VillainByMistake.Cards;
// We dont use this Script at all, was a first idea to play cards by clicking on them, might use next semester

public class CardClickHandler : MonoBehaviour, IPointerClickHandler
{
    private Card card;
    private PlayerController playerController;
    
    private void Start()
    {
        playerController = GameManager.Instance.Player;
    }
    
    public void SetCard(Card cardToSet)
    {
        card = cardToSet;
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (card == null || playerController == null) return;
        
        // Try to play the card
        bool played = playerController.TryPlayCard(card);
        
        if (played)
        {
            // Card was successfully played
            // Remove the card from hand visually
        }
        else
        {
            Debug.Log($"Cannot play card: {card.cardName}");
            // Add visual feedback here 
        }
    }
}
