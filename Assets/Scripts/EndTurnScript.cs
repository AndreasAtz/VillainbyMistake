using UnityEngine;
using UnityEngine.UI;

public class EndTurnButton : MonoBehaviour
{
    [SerializeField] private Button endTurnButton;
    [SerializeField] private GameObject buttonVisual;
    
    private void Start()
    {
        if (endTurnButton == null)
            endTurnButton = GetComponent<Button>();
        
        if (endTurnButton != null)
        {
            endTurnButton.onClick.AddListener(OnEndTurnClicked);
        }
        
        GameManager.Instance.OnGameStateChanged.AddListener(OnGameStateChanged);
        
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
// help