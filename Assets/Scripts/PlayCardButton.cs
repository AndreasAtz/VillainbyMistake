using UnityEngine;
using UnityEngine.UI;
using VillainByMistake.Cards;

public class PlayCardButton : MonoBehaviour
{
    [Header("Optional references (can auto-find)")]
    [SerializeField] private CardDisplay cardDisplay;     
    [SerializeField] private HandManager handManager;     
    [SerializeField] private Button button;

    private void Awake()
    {
        // All this ifs are if we didnt define them in inspector so they auto-find everything they need, just in case ifs
        if (button == null)
            button = GetComponent<Button>();

        if (cardDisplay == null)
            cardDisplay = GetComponentInParent<CardDisplay>();

        if (handManager == null)
            handManager = FindFirstObjectByType<HandManager>();

        if (button != null)
            button.onClick.AddListener(OnPlayClicked);
    }

    private void OnDestroy()
    {
        if (button != null)
            button.onClick.RemoveListener(OnPlayClicked);
    }

    private void OnPlayClicked()
    {
        if (cardDisplay == null || cardDisplay.cardData == null)
        {
            Debug.LogWarning("PlayCardButton: No CardDisplay/cardData found.");
            return;
        }

        var player = GameManager.Instance != null ? GameManager.Instance.Player : null;
        if (player == null)
        {
            Debug.LogWarning("PlayCardButton: GameManager.Instance.Player is null.");
            return;
        }

        Card card = cardDisplay.cardData;

        bool played = player.TryPlayCard(card);
        if (!played)
        {
            Debug.Log($"Cannot play card: {card.cardName}");
            return;
        }

        // Remove the card from the hand 
        if (handManager != null)
            handManager.RemoveCardObjectFromHand(cardDisplay.gameObject);
        else
            Destroy(cardDisplay.gameObject);
    }
}
