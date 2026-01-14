using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TMPro.TMP_Text energyText;
    
    private void Start()
    {
        if (slider == null) slider = GetComponent<Slider>();
        
        GameManager.Instance.Player.OnEnergyChanged.AddListener(UpdateEnergyBar);
        UpdateEnergyBar(GameManager.Instance.Player.CurrentEnergy);
    }
    
    private void UpdateEnergyBar(int energy)
    {
        if (slider != null)
        {
            slider.maxValue = 2;
            slider.value = energy;
        }
        
        if (energyText != null)
            energyText.text = $"{energy}/2";
    }
    
    private void OnDestroy()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.Player.OnEnergyChanged.RemoveListener(UpdateEnergyBar);
    }
}