using UnityEngine;
using UnityEngine.UI;
using VillainByMistake.Characters;

public class CharacterHealthBarUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Character character;   // Player oder Enemy
    [SerializeField] private Slider healthSlider;

    private void Awake()
    {
        if (healthSlider == null)
            healthSlider = GetComponent<Slider>();
    }

    private void OnEnable()
    {
        if (character != null)
            character.OnHealthChanged += UpdateHealthBar;

        UpdateHealthBar();
    }

    private void OnDisable()
    {
        if (character != null)
            character.OnHealthChanged -= UpdateHealthBar;
    }

    private void UpdateHealthBar()
    {
        if (character == null || healthSlider == null) return;

        healthSlider.minValue = 0;
        healthSlider.maxValue = character.maxHealth;
        healthSlider.value = character.CurrentHealth;
    }

    // Falls ihr später Ziel wechseln wollt (z.B. anderer Gegner)
    public void SetCharacter(Character newCharacter)
    {
        if (character != null)
            character.OnHealthChanged -= UpdateHealthBar;

        character = newCharacter;

        if (character != null)
            character.OnHealthChanged += UpdateHealthBar;

        UpdateHealthBar();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

	public Slider slider;
	public Gradient gradient;
	public Image fill;

	public void SetMaxHealth(int health)
	{
		slider.maxValue = health;
		slider.value = health;

		fill.color = gradient.Evaluate(1f);
	}

    public void SetHealth(int health)
	{
		slider.value = health;

		fill.color = gradient.Evaluate(slider.normalizedValue);
	}

}