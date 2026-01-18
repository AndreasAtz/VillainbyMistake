using UnityEngine;
using UnityEngine.UI;
using VillainByMistake.Cards;

public class PlayCardButton : MonoBehaviour
{
    [Header("Optional references (can auto-find)")]
    [SerializeField] private CardDisplay cardDisplay;     // sitzt auf dem CardPrefab-Root
    [SerializeField] private HandManager handManager;     // Manager in der Scene
    [SerializeField] private Button button;

    private void Awake()
    {
        if (button == null)
            button = GetComponent<Button>();

        // CardDisplay liegt meist am Root der Karte
        if (cardDisplay == null)
            cardDisplay = GetComponentInParent<CardDisplay>();

        // HandManager in der Scene finden
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

        // PlayerController kommt bei euch über GameManager.Instance.Player
        var player = GameManager.Instance != null ? GameManager.Instance.Player : null;
        if (player == null)
        {
            Debug.LogWarning("PlayCardButton: GameManager.Instance.Player is null.");
            return;
        }

        Card card = cardDisplay.cardData;

        // Versuche Karte zu spielen
        bool played = player.TryPlayCard(card);
        if (!played)
        {
            Debug.Log($"Cannot play card: {card.cardName}");
            return;
        }

        // Wenn gespielt: UI-Karte entfernen
        if (handManager != null)
            handManager.RemoveCardObjectFromHand(cardDisplay.gameObject);
        else
            Destroy(cardDisplay.gameObject);
    }
}
