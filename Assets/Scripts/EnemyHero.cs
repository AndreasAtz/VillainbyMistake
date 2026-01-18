using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VillainByMistake.Cards;

public class EnemyController : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private int maxHealth = 10;
    [SerializeField] private int currentHealth = 10;
    [SerializeField] private int energy = 2;
    
    [Header("AI Deck")]
    [SerializeField] private List<Card> enemyDeck = new List<Card>();
    
    // Active effects on enemy
    private List<EnemyActiveEffect> activeEffects = new List<EnemyActiveEffect>();
    
    // Events
    public UnityEvent<int> OnHealthChanged;
    
    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;
    
    private void Start()
    {
        currentHealth = maxHealth;
    }
    
    public void StartTurn()
    {
        // Used for next semester, so that enemy can take dmg from poison eg.
        ProcessStartTurnEffects();
        
        // The turn the enemy makes
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
            
            // Move to discard 
            enemyDeck.RemoveAt(0);
        }
        else
        {
            int damage = Random.Range(1, 3);
            // Basic attack if no cards, our go to for now
            player.TakeDamage(damage);
            Debug.Log($"Enemy performs basic attack for {damage} damage");
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
                break;
                
            case EffectType.Shield:
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