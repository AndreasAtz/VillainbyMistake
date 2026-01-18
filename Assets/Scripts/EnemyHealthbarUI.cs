using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthbarUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private EnemyController enemy;   
    [SerializeField] private Slider healthSlider;

    private void Awake()
    {
        if (healthSlider == null)
            healthSlider = GetComponent<Slider>();
    }

    private void Start()
    {
        if (enemy == null && GameManager.Instance != null)
            enemy = GameManager.Instance.Enemy;

        if (enemy == null || healthSlider == null)
        {
            Debug.LogWarning("EnemyHealthbarUI: enemy/slider not set.");
            return;
        }

        // Start Setup
        healthSlider.maxValue = enemy.MaxHealth;
        healthSlider.value = enemy.CurrentHealth;

        // Updates when game happens
        enemy.OnHealthChanged.AddListener(OnHealthChanged);
    }

    private void OnDestroy()
    {
        if (enemy != null)
            enemy.OnHealthChanged.RemoveListener(OnHealthChanged);
    }

    private void OnHealthChanged(int newHealth)
    {
        if (enemy == null || healthSlider == null) return;

        healthSlider.maxValue = enemy.MaxHealth;
        healthSlider.value = newHealth;
    }
}
