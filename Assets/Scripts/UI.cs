using UnityEngine;
using UnityEngine.UI;
using VillainByMistake.Characters;
//Andis thingi

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
            if (playerCharacter != null)
            {
                playerCharacter.OnHealthChanged += UpdatePlayerHealth;
                playerCharacter.OnEnergyChanged += UpdatePlayerEnergy;
                playerCharacter.OnShieldChanged += UpdatePlayerShield;
                
                if (activeAbilityButton != null)
                {
                    activeAbilityButton.onClick.AddListener(OnActiveAbilityClicked);
                    
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
            
            if (enemyCharacter != null)
            {
                enemyCharacter.OnHealthChanged += UpdateEnemyHealth;
            }
            else
            {
                Debug.LogWarning("Enemy Character reference is missing in SimpleCharacterUI!");
            }
            
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
            
            if (playerHealthSlider != null)
            {
                playerHealthSlider.maxValue = playerCharacter.maxHealth;
                playerHealthSlider.value = playerCharacter.CurrentHealth;
            }
            
            if (playerHealthText != null)
            {
                playerHealthText.text = $"{playerCharacter.CurrentHealth}/{playerCharacter.maxHealth}";
            }
            
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
            
            if (enemyHealthSlider != null)
            {
                enemyHealthSlider.maxValue = enemyCharacter.maxHealth;
                enemyHealthSlider.value = enemyCharacter.CurrentHealth;
            }
            
            if (enemyHealthText != null)
            {
                enemyHealthText.text = $"{enemyCharacter.CurrentHealth}/{enemyCharacter.maxHealth}";
            }
            
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
        
        public void SetPlayerCharacter(Character character)
        {
            if (playerCharacter != null)
            {
                playerCharacter.OnHealthChanged -= UpdatePlayerHealth;
                playerCharacter.OnEnergyChanged -= UpdatePlayerEnergy;
                playerCharacter.OnShieldChanged -= UpdatePlayerShield;
            }
            
            playerCharacter = character;
            
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