using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthbarUI : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private Slider slider;

    private void Awake()
    {
        if (slider == null)
            slider = GetComponent<Slider>();
    }

    private void Start()
    {
        if (player == null && GameManager.Instance != null)
            player = GameManager.Instance.Player;

        if (player == null || slider == null)
        {
            Debug.LogWarning("PlayerHealthbarUI: player/slider not set.");
            return;
        }

        // Initial
        slider.maxValue = player.MaxHealth;
        slider.value = player.CurrentHealth;

        // Updates
        player.OnHealthChanged.AddListener(OnHealthChanged);
    }

    private void OnDestroy()
    {
        if (player != null)
            player.OnHealthChanged.RemoveListener(OnHealthChanged);
    }

    private void OnHealthChanged(int newHealth)
    {
        if (player == null || slider == null) return;

        slider.maxValue = player.MaxHealth;
        slider.value = newHealth;
    }
}

