using UnityEngine;
using UnityEngine.UI;
using VillainByMistake.Characters;

public class CharacterHealthBarUI : MonoBehaviour
{
    [Header("Character Reference")]
    [SerializeField] private Character character;   // Player or Enemy
    
    [Header("UI Components")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Image healthFill;
    [SerializeField] private Gradient healthGradient; // Color from healthy to damaged
    
    [Header("Optional Text Display")]
    [SerializeField] private Text healthText; // Shows "100/100"
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
        // Smooth animation for health bar
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

    /// <summary>
    /// Initialize the health bar with a character
    /// </summary>
    public void Initialize(Character targetCharacter)
    {
        if (targetCharacter == null)
        {
            Debug.LogWarning("Cannot initialize health bar with null character!");
            return;
        }

        // Unsubscribe from previous character
        if (character != null)
            character.OnHealthChanged -= UpdateHealthBar;

        // Set new character
        character = targetCharacter;
        
        // Subscribe to health changes
        character.OnHealthChanged += UpdateHealthBar;
        
        // Set initial values
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

    /// <summary>
    /// Update health bar when character health changes
    /// </summary>
    private void UpdateHealthBar()
    {
        if (character == null || healthSlider == null) return;

        // Set target value for smooth animation
        targetHealthValue = character.CurrentHealth;
        
        // If not using smooth animation, set immediately
        if (!useSmoothAnimation)
        {
            healthSlider.value = targetHealthValue;
            UpdateGradientColor();
        }
        
        UpdateHealthText();
        
        // Optional: Play damage/heal animation
        PlayHealthChangeEffect();
    }

    /// <summary>
    /// Update the fill color based on health percentage
    /// </summary>
    private void UpdateGradientColor()
    {
        if (healthFill == null || healthGradient == null) return;
        
        float healthPercentage = character.CurrentHealth / (float)character.maxHealth;
        healthFill.color = healthGradient.Evaluate(healthPercentage);
    }

    /// <summary>
    /// Update the health text display
    /// </summary>
    private void UpdateHealthText()
    {
        if (healthText == null || !showText) return;
        
        healthText.text = $"{character.CurrentHealth}/{character.maxHealth}";
    }

    /// <summary>
    /// Play visual effect when health changes
    /// </summary>
    private void PlayHealthChangeEffect()
    {
        // You can add particle effects, screen shake, etc. here
        // Example: StartCoroutine(FlashHealthBar());
    }

    /// <summary>
    /// Manually set health values (useful for non-character objects)
    /// </summary>
    public void SetHealthValues(int currentHealth, int maxHealth)
    {
        if (healthSlider != null)
        {
            healthSlider.minValue = 0;
            healthSlider.maxValue = maxHealth;
            targetHealthValue = currentHealth;
            
            if (!useSmoothAnimation)
                healthSlider.value = currentHealth;
            
            // Update gradient if using immediate update
            if (!useSmoothAnimation && healthFill != null && healthGradient != null)
            {
                float healthPercentage = currentHealth / (float)maxHealth;
                healthFill.color = healthGradient.Evaluate(healthPercentage);
            }
            
            // Update text
            if (healthText != null && showText)
            {
                healthText.text = $"{currentHealth}/{maxHealth}";
            }
        }
    }

    /// <summary>
    /// Change which character this health bar is tracking
    /// </summary>
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