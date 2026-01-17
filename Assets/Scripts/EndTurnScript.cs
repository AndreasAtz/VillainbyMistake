using UnityEngine;
using UnityEngine.UI;

public class EndTurnButton : MonoBehaviour
{
    [SerializeField] private Button endTurnButton;
    [SerializeField] private GameObject buttonVisual;
    
    private void Start()
    {
        // Auto-find button if not set
        if (endTurnButton == null)
            endTurnButton = GetComponent<Button>();
        
        if (endTurnButton != null)
        {
            endTurnButton.onClick.AddListener(OnEndTurnClicked);
        }
        
        // Subscribe to game state changes
        GameManager.Instance.OnGameStateChanged.AddListener(OnGameStateChanged);
        
        // Set initial state
        UpdateButtonState(GameManager.Instance.CurrentState);
    }
    
    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameStateChanged.RemoveListener(OnGameStateChanged);
        }
        
        if (endTurnButton != null)
        {
            endTurnButton.onClick.RemoveListener(OnEndTurnClicked);
        }
    }
    
    private void OnEndTurnClicked()
    {
        // Only allow ending turn during player's turn
        if (GameManager.Instance.IsPlayerTurn)
        {
            GameManager.Instance.EndPlayerTurn();
        }
    }
    
    private void OnGameStateChanged(GameState newState)
    {
        UpdateButtonState(newState);
    }
    
    private void UpdateButtonState(GameState state)
    {
        bool isPlayerTurn = state == GameState.PlayerTurn;
        bool isGameOver = state == GameState.GameOver;
        
        // Enable button only during player's turn and not game over
        bool shouldBeActive = isPlayerTurn && !isGameOver;
        
        if (endTurnButton != null)
        {
            endTurnButton.interactable = shouldBeActive;
        }
        
        if (buttonVisual != null)
        {
            buttonVisual.SetActive(shouldBeActive);
        }
    }
}