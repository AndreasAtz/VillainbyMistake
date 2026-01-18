using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VillainByMistake.Cards;

public class PlayerController : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private int maxHealth = 10;
    [SerializeField] private int currentHealth = 10;
    [SerializeField] private int maxEnergy = 2;
    [SerializeField] private int currentEnergy;
    [SerializeField] private int currentArmor = 0;
    
    //[Header("Deck & Hand")]
    //[SerializeField] private List<Card> startingDeck = new List<Card>();
    [SerializeField] private List<Card> deck = new List<Card>();
    [SerializeField] private List<Card> hand = new List<Card>();
    [SerializeField] private List<Card> discardPile = new List<Card>();
    [SerializeField] private int maxHandSize = 3;
    
    [Header("References")]
    [SerializeField] private HandDisplay handDisplay;
    
    // Active effects
    private List<ActiveEffect> activeEffects = new List<ActiveEffect>();
    
    // Events
    public UnityEvent<int> OnHealthChanged;
    public UnityEvent<int> OnEnergyChanged;
    public UnityEvent<int> OnArmorChanged;
    public UnityEvent OnHandUpdated;
    
    // Properties for easy access
    public int CurrentHealth => currentHealth;
    public int CurrentEnergy => currentEnergy;
    public int CurrentArmor => currentArmor;
    public List<Card> Hand => hand;
    
    private void Start()
    {
        currentHealth = maxHealth;
        //InitializeDeck();
        //DrawStartingHand();
    }
    
    /*private void InitializeDeck()
    {
        deck.Clear();
        foreach (Card card in startingDeck)
        {
            deck.Add(card);
        }
        ShuffleDeck();
    }
    */
    public void StartTurn()
    {
        // Reset energy
        currentEnergy = maxEnergy;
        OnEnergyChanged?.Invoke(currentEnergy);
        
        // Process start-of-turn effects
        ProcessStartTurnEffects();
        
        Debug.Log($"Player Turn Started - Energy: {currentEnergy}, Health: {currentHealth}");
    }
    
    public bool TryPlayCard(Card card)
    {
        /* Check conditions
        if (!GameManager.Instance.IsPlayerTurn) return false;
        if (currentEnergy < card.energyCost) return false;
        if (!hand.Contains(card)) return false;
        */
        if (!GameManager.Instance.IsPlayerTurn) //The next 3 ifs test for debugging purposes because rene is a tester that tests tests
        {
        Debug.Log("TryPlayCard blocked: NOT PlayerTurn");
        return false;
        }

        if (currentEnergy < card.energyCost)
        {
        Debug.Log($"TryPlayCard blocked: NOT enough energy ({currentEnergy} < {card.energyCost})");
        return false;
        }

        if (!hand.Contains(card))
        {
        Debug.Log("TryPlayCard blocked: card NOT in player hand list");
        return false;
        }
        
        // Spend energy
        currentEnergy -= card.energyCost;
        OnEnergyChanged?.Invoke(currentEnergy);
        
        // Remove from hand
        hand.Remove(card);
        
        // Play the card based on target type
        PlayCardEffect(card);
        
        // Move to discard
        discardPile.Add(card);
        
        // Update hand display
        OnHandUpdated?.Invoke();
        handDisplay?.UpdateHandDisplay();
        
        Debug.Log($"Played: {card.cardName}");
        return true;
    }
    
    private void PlayCardEffect(Card card)
    {
        switch (card.targetType)
        {
            case Card.TargetType.Self:
                ApplyCardToSelf(card);
                break;
                
            case Card.TargetType.SingleEnemy:
                ApplyCardToEnemy(card);
                break;
                
            case Card.TargetType.AllEnemies:
                // For now, just apply to single enemy (expand later)
                ApplyCardToEnemy(card);
                break;
        }
    }
    
    private void ApplyCardToSelf(Card card)
    {
        // Apply direct effects
        if (card.heal > 0) Heal(card.heal);
        if (card.armor > 0) AddArmor(card.armor);
        
        // Apply special effects to self
        if (card.hasSpecialEffect && CheckEffectChance(card.effectChance))
        {
            ApplyEffectToSelf(card.effectType, card.effectValue, card.effectDuration);
        }
    }
    
    private void ApplyCardToEnemy(Card card)
    {
        EnemyController enemy = GameManager.Instance.Enemy;
        
        // Apply direct damage
        if (card.damage > 0) enemy.TakeDamage(card.damage);
        
        // Apply special effects to enemy
        if (card.hasSpecialEffect && CheckEffectChance(card.effectChance))
        {
            enemy.ApplyEffect(card.effectType, card.effectValue, card.effectDuration);
        }
    }
    
    private bool CheckEffectChance(int chance)
    {
        return Random.Range(0, 100) < chance;
    }
    
    private void ApplyEffectToSelf(EffectType effect, int value, int duration)
    {
        // Add to active effects
        activeEffects.Add(new ActiveEffect(effect, value, duration));
        
        // Immediate application
        switch (effect)
        {
            case EffectType.Shield:
                AddArmor(value);
                break;
            case EffectType.Regeneration:
                Heal(value);
                // This would also need to be reapplied each turn
                break;
        }
    }
    
    private void ProcessStartTurnEffects()
    {
        List<ActiveEffect> effectsToRemove = new List<ActiveEffect>();
        
        foreach (var effect in activeEffects)
        {
            // Process effect for this turn
            switch (effect.Type)
            {
                case EffectType.Regeneration:
                    Heal(effect.Value);
                    break;
                // Add other ongoing effects here
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
    public void DrawCards(int amount)
    {
        // Simple method that just updates hand display
        // Actual card drawing is handled by DeckManager
        OnHandUpdated?.Invoke();
        handDisplay?.UpdateHandDisplay();
    }
    /*
    public void DrawCards(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (hand.Count >= maxHandSize) break;
            if (deck.Count == 0) ReshuffleDiscard();
            if (deck.Count == 0) return;
            
            Card drawnCard = deck[0];
            deck.RemoveAt(0);
            hand.Add(drawnCard);
        }
        
        OnHandUpdated?.Invoke();
        handDisplay?.UpdateHandDisplay();
    }
    
    private void DrawStartingHand()
    {
        DrawCards(maxHandSize);
    }
    */
    private void ShuffleDeck()
    {
        for (int i = 0; i < deck.Count; i++)
        {
            Card temp = deck[i];
            int randomIndex = Random.Range(i, deck.Count);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }
    }
    
    private void ReshuffleDiscard()
    {
        deck.AddRange(discardPile);
        discardPile.Clear();
        ShuffleDeck();
        Debug.Log("Reshuffled discard pile into deck");
    }
    
    public void TakeDamage(int damage)
    {
        // Armor absorbs damage first
        if (currentArmor > 0)
        {
            int damageToArmor = Mathf.Min(damage, currentArmor);
            currentArmor -= damageToArmor;
            damage -= damageToArmor;
            OnArmorChanged?.Invoke(currentArmor);
        }
        
        if (damage > 0)
        {
            currentHealth -= damage;
            if (currentHealth < 0) currentHealth = 0;
            OnHealthChanged?.Invoke(currentHealth);
        }
        
        if (currentHealth <= 0)
    {
        GameManager.Instance.CheckGameOver();
    }
    }
    
    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth);
    }
    
    public void AddArmor(int amount)
    {
        currentArmor += amount;
        OnArmorChanged?.Invoke(currentArmor);
    }
    
    // Helper class for active effects
    private class ActiveEffect
    {
        public EffectType Type { get; private set; }
        public int Value { get; private set; }
        public int Duration { get; set; }
        
        public ActiveEffect(EffectType type, int value, int duration)
        {
            Type = type;
            Value = value;
            Duration = duration;
        }
    }
    public void AddCardToHand(Card card)
{
    // Add to hand list
    if (hand.Count < maxHandSize)
    {
        hand.Add(card);
        
        // Let HandManager handle the visual creation
        if (GameManager.Instance != null && 
            GameManager.Instance.handManager != null)
        {
            GameManager.Instance.handManager.AddCardToHand(card);
        }
        else
        {
            Debug.LogError("HandManager not found to create card visual!");
        }
        
        OnHandUpdated?.Invoke();
        handDisplay?.UpdateHandDisplay();
    }
    else
    {
        Debug.Log("Hand is full!");
    }
}

// Remove any other card creation code from PlayerController
// Look for any Instantiate() calls or other AddCardToHand methods

    /*TEST Rene
    public void AddCardToHand(Card card)
    {
        hand.Add(card);
        OnHandUpdated?.Invoke();
    }
    */
    public void AddCardToListOnly(Card card)
{
    if (hand.Count < maxHandSize)
    {
        hand.Add(card);
        OnHandUpdated?.Invoke();
        handDisplay?.UpdateHandDisplay();
    }
}
}
