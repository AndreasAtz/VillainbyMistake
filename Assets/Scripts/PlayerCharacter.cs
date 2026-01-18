using UnityEngine;
// Used to be the Base Class we didnt use this at all, just here for the memory

namespace VillainByMistake.Characters
{
    public abstract class Character : MonoBehaviour
    {
        [Header("Base Stats")]
        public string characterName;
        public int maxHealth = 10;
        [SerializeField] protected int currentHealth;
        
        [Header("Resources")]
        public int maxEnergy = 2;
        [SerializeField] protected int currentEnergy;
        
        [Header("Shield")]
        [SerializeField] protected int currentShield = 0;
        
        [Header("Visuals")]
        public Sprite characterPortrait;
        
        // Properties for UI
        public int CurrentHealth => currentHealth;
        public int CurrentEnergy => currentEnergy;
        public int CurrentShield => currentShield;
        
        // Events for UI updates
        public System.Action OnHealthChanged;
        public System.Action OnEnergyChanged;
        public System.Action OnShieldChanged;
        
        protected virtual void Start()
        {
            InitializeCharacter();
        }
        
        public virtual void InitializeCharacter()
        {
            currentHealth = maxHealth;
            currentEnergy = maxEnergy;
            currentShield = 0;
            
            // Notify UI
            OnHealthChanged?.Invoke();
            OnEnergyChanged?.Invoke();
            OnShieldChanged?.Invoke();
        }
        
        public virtual void TakeDamage(int damage)
        {
            // First reduce shield, then health
            if (currentShield > 0)
            {
                int remainingDamage = damage - currentShield;
                currentShield = Mathf.Max(0, currentShield - damage);
                
                if (remainingDamage > 0)
                {
                    currentHealth -= remainingDamage;
                }
            }
            else
            {
                currentHealth -= damage;
            }
            
            currentHealth = Mathf.Max(0, currentHealth);
            
            OnHealthChanged?.Invoke();
            OnShieldChanged?.Invoke();
            
            Debug.Log($"{characterName} took {damage} damage. Health: {currentHealth}/{maxHealth}");
            
            CheckDeath();
        }
        
        public virtual void Heal(int amount)
        {
            currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
            OnHealthChanged?.Invoke();
            Debug.Log($"{characterName} healed {amount}. Health: {currentHealth}/{maxHealth}");
        }
        
        public virtual void AddShield(int amount)
        {
            currentShield += amount;
            OnShieldChanged?.Invoke();
            Debug.Log($"{characterName} gained {amount} shield. Total: {currentShield}");
        }
        
        public virtual bool SpendEnergy(int amount)
        {
            if (currentEnergy >= amount)
            {
                currentEnergy -= amount;
                OnEnergyChanged?.Invoke();
                Debug.Log($"{characterName} spent {amount} energy. Remaining: {currentEnergy}");
                return true;
            }
            Debug.Log($"Not enough energy! Needed: {amount}, Has: {currentEnergy}");
            return false;
            
        }
        
        public virtual void RestoreEnergy(int amount = 0)
        {
            if (amount == 0) amount = maxEnergy; 
            currentEnergy = Mathf.Min(currentEnergy + amount, maxEnergy);
            OnEnergyChanged?.Invoke();
            Debug.Log($"{characterName} restored {amount} energy. Total: {currentEnergy}");
        }
        
        protected virtual void CheckDeath()
        {
            if (currentHealth <= 0)
            {
                Debug.Log($"{characterName} died!");
            }
        }
        
        public abstract void UseActiveAbility();
        public abstract string GetActiveAbilityDescription();
    }
    
}