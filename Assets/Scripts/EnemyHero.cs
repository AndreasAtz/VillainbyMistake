using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VillainByMistake.Cards;

public class EnemyController : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private int maxHealth = 30;
    [SerializeField] private int currentHealth;
    [SerializeField] private int energy = 3;
    
    [Header("AI Deck")]
    [SerializeField] private List<Card> enemyDeck = new List<Card>();
    
    // Active effects on enemy
    private List<EnemyActiveEffect> activeEffects = new List<EnemyActiveEffect>();
    
    // Events
    public UnityEvent<int> OnHealthChanged;
    
    public int CurrentHealth => currentHealth;
    
    private void Start()
    {
        currentHealth = maxHealth;
    }
    
    public void StartTurn()
    {
        // Process enemy's start of turn effects (like poison damage)
        ProcessStartTurnEffects();
        
        // Simple AI: Play 1 card or basic attack
        PlayAITurn();
        
        // End turn after delay
        Invoke(nameof(EndTurn), 1.5f);
    }
    
    private void ProcessStartTurnEffects()
    {
        List<EnemyActiveEffect> effectsToRemove = new List<EnemyActiveEffect>();
        
        foreach (var effect in activeEffects)
        {
            // Process effect for this turn
            switch (effect.Type)
            {
                case EffectType.Poison:
                    TakeDamage(effect.Value);
                    Debug.Log($"Enemy takes {effect.Value} poison damage");
                    break;
                // Add other effects as needed
            }
            
            // Reduce duration
            effect.Duration--;
            if (effect.Duration <= 0)
            {
                effectsToRemove.Add(effect);
            }
        }
        
        // Remove expired effects
        foreach (var effect in effectsToRemove)
        {
            activeEffects.Remove(effect);
        }
    }
    
    private void PlayAITurn()
    {
        PlayerController player = GameManager.Instance.Player;
        
        // Simple AI logic: Always attack if possible
        if (enemyDeck.Count > 0)
        {
            // Pick first available card
            Card cardToPlay = enemyDeck[0];
            
            // Play the card
            if (cardToPlay.damage > 0)
            {
                player.TakeDamage(cardToPlay.damage);
                Debug.Log($"Enemy plays {cardToPlay.cardName} for {cardToPlay.damage} damage");
            }
            
            // Move to discard (simple implementation)
            enemyDeck.RemoveAt(0);
        }
        else
        {
            // Basic attack if no cards
            player.TakeDamage(5);
            Debug.Log("Enemy performs basic attack for 5 damage");
        }
    }
    
    // Method for player to apply effects to enemy
    public void ApplyEffect(EffectType effect, int value, int duration)
    {
        // Add effect to enemy
        activeEffects.Add(new EnemyActiveEffect(effect, value, duration));
        
        Debug.Log($"Enemy affected by {effect} (Value: {value}, Duration: {duration})");
        
        // Apply immediate effect if applicable
        switch (effect)
        {
            case EffectType.Poison:
                // Poison starts on NEXT turn, so just log for now
                break;
                
            case EffectType.Shield:
                // Enemies don't typically have armor, but we could add it
                // For now, just ignore or implement if needed
                break;
        }
    }
    
    private void EndTurn()
    {
        GameManager.Instance.EndEnemyTurn();
    }
    
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;
        OnHealthChanged?.Invoke(currentHealth);
        
        Debug.Log($"Enemy takes {damage} damage. Health: {currentHealth}/{maxHealth}");
        
        GameManager.Instance.CheckGameOver();
    }
    
    // Helper class for enemy effects
    private class EnemyActiveEffect
    {
        public EffectType Type { get; private set; }
        public int Value { get; private set; }
        public int Duration { get; set; }
        
        public EnemyActiveEffect(EffectType type, int value, int duration)
        {
            Type = type;
            Value = value;
            Duration = duration;
        }
    }
}