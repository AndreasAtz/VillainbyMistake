using UnityEngine;
using UnityEngine.UI;
using VillainByMistake.Characters;

namespace VillainByMistake.UI
{
    public class SimpleCharacterUI : MonoBehaviour
    {
        [Header("Player References")]
        public Character playerCharacter;
        
        [Header("Player UI Elements")]
        public Text playerNameText;
        public Slider playerHealthSlider;
        public Text playerHealthText;
        public Text playerEnergyText;
        public Text playerShieldText;
        public Button activeAbilityButton;
        
        [Header("Enemy References")]
        public Character enemyCharacter;
        
        [Header("Enemy UI Elements")]
        public Text enemyNameText;
        public Slider enemyHealthSlider;
        public Text enemyHealthText;
        
        void Start()
        {
            InitializeUI();
        }
        
        void InitializeUI()
        {
            // Subscribe to player events
            if (playerCharacter != null)
            {
                playerCharacter.OnHealthChanged += UpdatePlayerHealth;
                playerCharacter.OnEnergyChanged += UpdatePlayerEnergy;
                playerCharacter.OnShieldChanged += UpdatePlayerShield;
                
                // Setup active ability button
                if (activeAbilityButton != null)
                {
                    activeAbilityButton.onClick.AddListener(OnActiveAbilityClicked);
                    
                    // Safely set button text
                    Text buttonText = activeAbilityButton.GetComponentInChildren<Text>();
                    if (buttonText != null)
                    {
                        buttonText.text = playerCharacter.GetActiveAbilityDescription();
                    }
                }
            }
            else
            {
                Debug.LogWarning("Player Character reference is missing in SimpleCharacterUI!");
            }
            
            // Subscribe to enemy events
            if (enemyCharacter != null)
            {
                enemyCharacter.OnHealthChanged += UpdateEnemyHealth;
            }
            else
            {
                Debug.LogWarning("Enemy Character reference is missing in SimpleCharacterUI!");
            }
            
            // Initial update
            UpdateAllUI();
        }
        
        void UpdateAllUI()
        {
            UpdatePlayerHealth();
            UpdatePlayerEnergy();
            UpdatePlayerShield();
            UpdateEnemyHealth();
        }
        
        void UpdatePlayerHealth()
        {
            if (playerCharacter == null) return;
            
            // Update health slider
            if (playerHealthSlider != null)
            {
                playerHealthSlider.maxValue = playerCharacter.maxHealth;
                playerHealthSlider.value = playerCharacter.CurrentHealth;
            }
            
            // Update health text
            if (playerHealthText != null)
            {
                playerHealthText.text = $"{playerCharacter.CurrentHealth}/{playerCharacter.maxHealth}";
            }
            
            // Update name text
            if (playerNameText != null)
            {
                playerNameText.text = playerCharacter.characterName;
            }
        }
        
        void UpdatePlayerEnergy()
        {
            if (playerCharacter == null || playerEnergyText == null) return;
            
            playerEnergyText.text = $"Energy: {playerCharacter.CurrentEnergy}/{playerCharacter.maxEnergy}";
        }
        
        void UpdatePlayerShield()
        {
            if (playerCharacter == null || playerShieldText == null) return;
            
            if (playerCharacter.CurrentShield > 0)
            {
                playerShieldText.text = $"Shield: {playerCharacter.CurrentShield}";
            }
            else
            {
                playerShieldText.text = "";
            }
        }
        
        void UpdateEnemyHealth()
        {
            if (enemyCharacter == null) return;
            
            // Update enemy health slider
            if (enemyHealthSlider != null)
            {
                enemyHealthSlider.maxValue = enemyCharacter.maxHealth;
                enemyHealthSlider.value = enemyCharacter.CurrentHealth;
            }
            
            // Update enemy health text
            if (enemyHealthText != null)
            {
                enemyHealthText.text = $"{enemyCharacter.CurrentHealth}/{enemyCharacter.maxHealth}";
            }
            
            // Update enemy name text
            if (enemyNameText != null)
            {
                enemyNameText.text = enemyCharacter.characterName;
            }
        }
        
        void OnActiveAbilityClicked()
        {
            if (playerCharacter != null)
            {
                playerCharacter.UseActiveAbility();
            }
        }
        
        // Public methods to update UI manually (if needed)
        public void RefreshPlayerUI()
        {
            UpdatePlayerHealth();
            UpdatePlayerEnergy();
            UpdatePlayerShield();
        }
        
        public void RefreshEnemyUI()
        {
            UpdateEnemyHealth();
        }
        
        // Clean up events
        private void OnDestroy()
        {
            if (playerCharacter != null)
            {
                playerCharacter.OnHealthChanged -= UpdatePlayerHealth;
                playerCharacter.OnEnergyChanged -= UpdatePlayerEnergy;
                playerCharacter.OnShieldChanged -= UpdatePlayerShield;
            }
            
            if (enemyCharacter != null)
            {
                enemyCharacter.OnHealthChanged -= UpdateEnemyHealth;
            }
        }
        
        // Optional: Add this to manually set characters if they're created at runtime
        public void SetPlayerCharacter(Character character)
        {
            // Unsubscribe from old character
            if (playerCharacter != null)
            {
                playerCharacter.OnHealthChanged -= UpdatePlayerHealth;
                playerCharacter.OnEnergyChanged -= UpdatePlayerEnergy;
                playerCharacter.OnShieldChanged -= UpdatePlayerShield;
            }
            
            // Set new character
            playerCharacter = character;
            
            // Subscribe to new character
            if (playerCharacter != null)
            {
                playerCharacter.OnHealthChanged += UpdatePlayerHealth;
                playerCharacter.OnEnergyChanged += UpdatePlayerEnergy;
                playerCharacter.OnShieldChanged += UpdatePlayerShield;
                
                UpdateAllUI();
            }
        }
    }
}