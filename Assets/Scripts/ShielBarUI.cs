using UnityEngine;
using UnityEngine.UI;

public class ShieldBarUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerController player;

    [Header("Shield pips (max 5)")]
    [SerializeField] private Image[] pips;

    private void Start()
    {
        if (player == null && GameManager.Instance != null)
            player = GameManager.Instance.Player;

        if (player == null)
        {
            Debug.LogWarning("ShieldBarUI: Player not set.");
            return;
        }

        // Initial
        Refresh(player.CurrentArmor);

        // Updates
        player.OnArmorChanged.AddListener(Refresh);
    }

    private void OnDestroy()
    {
        if (player != null)
            player.OnArmorChanged.RemoveListener(Refresh);
    }

    private void Refresh(int armor)
    {
        if (pips == null) return;

        // max 5 anzeigen
        int shown = Mathf.Clamp(armor, 0, pips.Length);

        for (int i = 0; i < pips.Length; i++)
        {
            if (pips[i] == null) continue;
            pips[i].enabled = (i < shown);
        }
    }
}
