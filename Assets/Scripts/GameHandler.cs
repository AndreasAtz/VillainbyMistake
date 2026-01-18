using UnityEngine;
using UnityEngine.Events;
using VillainByMistake.Cards;

public enum GameState {PlayerTurn, EnemyTurn, GameOver }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    [SerializeField] private GameState currentState = GameState.PlayerTurn;
    [SerializeField] private PlayerController player;
    [SerializeField] private EnemyController enemy;
    [SerializeField] private DeckManager deckManager;
    [SerializeField] public HandManager handManager;
    
    public UnityEvent<GameState> OnGameStateChanged;
    public UnityEvent<bool> OnGameOver; 
    
    // Getters for our other scripts
    public GameState CurrentState => currentState;
    public PlayerController Player => player;
    public EnemyController Enemy => enemy;
    public bool IsPlayerTurn => currentState == GameState.PlayerTurn;
    
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

    }
    
    private void Start()
    {
         for (int i = 0; i < 3; i++)
        {
            deckManager.DrawCard(handManager);
        }
        //FirstPlayerTurn();
        StartPlayerTurn();
    }

    /*public void FirstPlayerTurn()
    {
        currentState = GameState.PlayerTurn;
        deckManager.DrawCard(handManager);
        deckManager.DrawCard(handManager);
        deckManager.DrawCard(handManager);
        player.StartTurn();
        StartPlayerTurn();
    }
    */
    public void StartPlayerTurn()
    {
        currentState = GameState.PlayerTurn;
        player.StartTurn();
        OnGameStateChanged?.Invoke(currentState);
    }
    
    public void EndPlayerTurn()
    {
        if (currentState != GameState.PlayerTurn) return;
        
        currentState = GameState.EnemyTurn;
        OnGameStateChanged?.Invoke(currentState);
        
        Invoke(nameof(StartEnemyTurn), 0.5f);
    }
    
    private void StartEnemyTurn()
    {
        enemy.StartTurn();
    }
    
    public void EndEnemyTurn()
    {
        deckManager.DrawCard(handManager);
        StartPlayerTurn();
        
    }
    
    /*public void CheckGameOver()
    {
        if (player.CurrentHealth <= 0)
        {
            currentState = GameState.GameOver;
            OnGameOver?.Invoke(false);
            Debug.Log("Game Over - Player Lost");
        }
        else if (enemy.CurrentHealth <= 0)
        {
            currentState = GameState.GameOver;
            OnGameOver?.Invoke(true);
            Debug.Log("Game Over - Player Won!");
        }
    }
    */
    public void CheckGameOver()
{
    // Check if game is already over
    if (currentState == GameState.GameOver) return;
    
    if (player.CurrentHealth <= 0)
    {
        currentState = GameState.GameOver;
        OnGameOver?.Invoke(false);
        Debug.Log("Game Over - Player Lost");
        StopGameCompletely();
    }
    else if (enemy.CurrentHealth <= 0)
    {
        currentState = GameState.GameOver;
        OnGameOver?.Invoke(true);
        Debug.Log("Game Over - Player Won!");
        StopGameCompletely();
    }
}

private void StopGameCompletely()
{
    Debug.Log("Stopping game completely...");
    
    // Freeze the game so no moves can be done
    Time.timeScale = 0f;
    
    // Disable the GameManager so nothing can be triggered (to not trigger any bugs)
    this.enabled = false;
    
    // Cancel any invokes so nothing can happen after its ended, more usefull for later when we have poison eg.
    CancelInvoke();
    
    // Found this while googling 
    StopAllCoroutines();

    // Disable the enemy 
    if (enemy != null)
    {
        enemy.enabled = false;
    }
    
    Debug.Log("Game completely frozen and stopped");
}

}
