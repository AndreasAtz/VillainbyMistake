using UnityEngine;
using UnityEngine.UI;

public class DrawOneAbility : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private DeckManager deckManager;
    [SerializeField] private HandManager handManager;
    [SerializeField] private Button abilityButton;

    private bool used = false;

    private void Start()
    {
        if (deckManager == null) deckManager = FindFirstObjectByType<DeckManager>();
        if (handManager == null) handManager = FindFirstObjectByType<HandManager>();
        if (abilityButton == null) abilityButton = GetComponent<Button>(); 
    }

    public void ActivateAbility()
    {
        if (used) return;

        if (GameManager.Instance != null && !GameManager.Instance.IsPlayerTurn) return;

        if (deckManager == null || handManager == null)
        {
            Debug.LogWarning("DrawOneAbility: Missing deckManager/handManager reference.");
            return;
        }

        used = true;

        deckManager.DrawCard(handManager);

        // To deactivate the button after use, cause its once per fight
        if (abilityButton != null)
            abilityButton.interactable = false;

        Debug.Log("Ability used: Drew 1 card (one-time).");
    }

    // For later use cases, so that the ability resets after every fight, next semester important
    public void ResetAbility()
    {
        used = false;
        if (abilityButton != null) abilityButton.interactable = true;
    }
}

