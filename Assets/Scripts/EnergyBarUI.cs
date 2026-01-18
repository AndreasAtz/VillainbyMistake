using UnityEngine;
using UnityEngine.UI;
//The real EnergyBar

public class EnergyBarUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerController player;

    [Header("Energy Pips (size = MaxEnergy)")]
    [SerializeField] private Image[] pips;

    private void Start()
    {
        // Find the player if we did not set it up correctly
        if (player == null && GameManager.Instance != null)
            player = GameManager.Instance.Player;

        if (player == null)
        {
            Debug.LogWarning("EnergyBarUI: Player not set.");
            return;
        }

        // Show initial energy
        Refresh(player.CurrentEnergy);

        // Subscribe to energy changes - so we can update the UI
        player.OnEnergyChanged.AddListener(Refresh);
    }

    private void OnDestroy()
    {
        if (player != null)
            player.OnEnergyChanged.RemoveListener(Refresh);
    }

    private void Refresh(int currentEnergy)
    {
        if (pips == null) return;

        for (int i = 0; i < pips.Length; i++)
        {
            if (pips[i] == null) continue;
            pips[i].enabled = (i < currentEnergy);
        }
    }
}
