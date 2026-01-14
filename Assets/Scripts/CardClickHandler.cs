using UnityEngine;
using UnityEngine.EventSystems;
using VillainByMistake.Cards;

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
            // It will be removed from hand by PlayerController
        }
        else
        {
            Debug.Log($"Cannot play card: {card.cardName}");
            // You could add visual feedback here (shake, color change, etc.)
        }
    }
}