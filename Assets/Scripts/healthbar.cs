using UnityEngine;
using UnityEngine.UI;
using VillainByMistake.Characters;
// Sad first over the top fancy Healthbar try didnt work out so we switched to easier implementation, this is not used, maybe in the future

public class CharacterHealthBarUI : MonoBehaviour
{
    [Header("Character Reference")]
    [SerializeField] private Character character;   
    
    [Header("UI Components")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Image healthFill;
    [SerializeField] private Gradient healthGradient; 
    
    [Header("Optional Text Display")]
    [SerializeField] private Text healthText; 
    [SerializeField] private bool showText = true;
    
    [Header("Smooth Animation")]
    [SerializeField] private bool useSmoothAnimation = true;
    [SerializeField] private float smoothSpeed = 5f;
    
    private float targetHealthValue;
    private bool isInitialized = false;

    private void Awake()
    {
        // Auto-find components if not assigned
        if (healthSlider == null)
            healthSlider = GetComponent<Slider>();
        
        if (healthFill == null && healthSlider != null)
            healthFill = healthSlider.fillRect?.GetComponent<Image>();
        
        if (healthText == null)
            healthText = GetComponentInChildren<Text>();
        
        // Initialize if character is pre-assigned
        if (character != null)
            Initialize(character);
    }

    private void OnEnable()
    {
        if (character != null && !isInitialized)
            Initialize(character);
    }

    private void OnDisable()
    {
        if (character != null)
            character.OnHealthChanged -= UpdateHealthBar;
    }

    private void Update()
    {
        // Animation for health bar cause we fancy
        if (useSmoothAnimation && healthSlider != null)
        {
            healthSlider.value = Mathf.Lerp(healthSlider.value, targetHealthValue, 
                Time.deltaTime * smoothSpeed);
            
            // Update gradient color during animation
            if (healthFill != null && healthGradient != null)
            {
                float normalizedValue = healthSlider.value / healthSlider.maxValue;
                healthFill.color = healthGradient.Evaluate(normalizedValue);
            }
        }
    }

    public void Initialize(Character targetCharacter)
    {
        if (targetCharacter == null)
        {
            Debug.LogWarning("Cannot initialize health bar with null character!");
            return;
        }

        if (character != null)
            character.OnHealthChanged -= UpdateHealthBar;

        character = targetCharacter;
        
        character.OnHealthChanged += UpdateHealthBar;
        
        if (healthSlider != null)
        {
            healthSlider.minValue = 0;
            healthSlider.maxValue = character.maxHealth;
            targetHealthValue = character.CurrentHealth;
            
            if (!useSmoothAnimation)
                healthSlider.value = targetHealthValue;
        }
        
        // Update gradient
        UpdateGradientColor();
        
        // Update text
        UpdateHealthText();
        
        isInitialized = true;
    }

    private void UpdateHealthBar()
    {
        if (character == null || healthSlider == null) return;

        targetHealthValue = character.CurrentHealth;
        
        if (!useSmoothAnimation)
        {
            healthSlider.value = targetHealthValue;
            UpdateGradientColor();
        }
        
        UpdateHealthText();
    }

    private void UpdateGradientColor()
    {
        if (healthFill == null || healthGradient == null) return;
        
        float healthPercentage = character.CurrentHealth / (float)character.maxHealth;
        healthFill.color = healthGradient.Evaluate(healthPercentage);
    }

    private void UpdateHealthText()
    {
        if (healthText == null || !showText) return;
        
        healthText.text = $"{character.CurrentHealth}/{character.maxHealth}";
    }

    public void SetHealthValues(int currentHealth, int maxHealth)
    {
        if (healthSlider != null)
        {
            healthSlider.minValue = 0;
            healthSlider.maxValue = maxHealth;
            targetHealthValue = currentHealth;
            
            if (!useSmoothAnimation)
                healthSlider.value = currentHealth;
            
            if (!useSmoothAnimation && healthFill != null && healthGradient != null)
            {
                float healthPercentage = currentHealth / (float)maxHealth;
                healthFill.color = healthGradient.Evaluate(healthPercentage);
            }
            
            if (healthText != null && showText)
            {
                healthText.text = $"{currentHealth}/{maxHealth}";
            }
        }
    }

    public void SetCharacter(Character newCharacter)
    {
        Initialize(newCharacter);
    }

    // Clean up
    private void OnDestroy()
    {
        if (character != null)
            character.OnHealthChanged -= UpdateHealthBar;
    }
}