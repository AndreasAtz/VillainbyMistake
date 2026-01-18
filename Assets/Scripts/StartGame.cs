using UnityEngine;
using UnityEngine.UI;

public class StartGameButton : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private CanvasGroup buttonCanvasGroup; // For fade out
    [SerializeField] private GameManager gameManager;
    [SerializeField] private DeckManager deckManager;
    [SerializeField] private HandManager handManager;
    
    [Header("Animation")]
    [SerializeField] private float fadeOutDuration = 0.5f;
    
    private void Start()
    {
        // Ensure button is visible at start
        /*if (buttonCanvasGroup != null)
        {
            buttonCanvasGroup.alpha = 1f;
            buttonCanvasGroup.interactable = true;
            buttonCanvasGroup.blocksRaycasts = true;
        }
        */
        // Set up button click
        if (startButton != null)
        {
            startButton.onClick.AddListener(OnStartButtonClicked);
        }
        
        // Find references if not set
        if (gameManager == null)
            gameManager = FindObjectOfType<GameManager>();
        if (deckManager == null)
            deckManager = FindObjectOfType<DeckManager>();
        if (handManager == null)
            handManager = FindObjectOfType<HandManager>();
    }
    
    private void OnStartButtonClicked()
    {
        Debug.Log("Start Game button clicked");
        
        // Draw 3 starting cards
        if (deckManager != null && handManager != null)
        {
            for (int i = 0; i < 3; i++)
            {
                deckManager.DrawCard(handManager);
            }
            Debug.Log("Drew 3 starting cards");
            gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("Missing DeckManager or HandManager references!");
        }
        
        // Start the game
        if (gameManager != null)
        {
            gameManager.StartPlayerTurn();
        }
        
        // Hide the button with fade animation
    }
    
    
    private System.Collections.IEnumerator HideButton()
    {
        if (buttonCanvasGroup == null) yield break;
        
        float elapsedTime = 0f;
        float startAlpha = buttonCanvasGroup.alpha;
        
        while (elapsedTime < fadeOutDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fadeOutDuration;
            buttonCanvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, t);
            yield return null;
        }
        
        buttonCanvasGroup.alpha = 0f;
        buttonCanvasGroup.interactable = false;
        buttonCanvasGroup.blocksRaycasts = false;
        
        // Optionally disable the whole GameObject
        // gameObject.SetActive(false);
    }
    
    // Public method to show button again (for restart)
    public void ShowButton()
    {
        if (buttonCanvasGroup != null)
        {
            buttonCanvasGroup.alpha = 1f;
            buttonCanvasGroup.interactable = true;
            buttonCanvasGroup.blocksRaycasts = true;
        }
        
        gameObject.SetActive(true);
    }
    
    private void OnDestroy()
    {
        // Clean up event listeners
        if (startButton != null)
        {
            startButton.onClick.RemoveListener(OnStartButtonClicked);
        }
    }
}
