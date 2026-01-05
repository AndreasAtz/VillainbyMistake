using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using VillainByMistake;
using VillainByMistake.Cards;

public class CardDisplay : MonoBehaviour
{
    public Card cardData;

    public Image cardImage;
    public TMP_Text nameText;
    public TMP_Text healthText;
    public TMP_Text damageText;
    public Image typeImage;

    void Start()
    {
        UpdateCardDisplay();
    }

    public void UpdateCardDisplay()
    {
        if (cardData == null)
        {
            if (nameText) nameText.text = string.Empty;
            if (cardImage) { cardImage.sprite = null; cardImage.enabled = false; }
            if (damageText) damageText.text = string.Empty;
            if (healthText) healthText.text = string.Empty;
            if (typeImage) typeImage.enabled = false;
            return;
        }

        if (nameText) nameText.text = cardData.cardName ?? string.Empty;

        if (cardImage)
        {
            cardImage.sprite = cardData.artwork;
            cardImage.enabled = cardData.artwork != null;
        }

        if (damageText)
        {
            damageText.text = cardData.damage > 0 ? cardData.damage.ToString() : string.Empty;
        }

        if (healthText)
        {
            if (cardData.heal > 0)
                healthText.text = cardData.heal.ToString();
            else if (cardData.armor > 0)
                healthText.text = cardData.armor.ToString();
            else
                healthText.text = string.Empty;
        }

        if (typeImage)
        {
            typeImage.enabled = true;
            switch (cardData.cardType)
            {
                case CardType.Fire:
                    typeImage.color = new Color32(232, 59, 59, 255);
                    break;
                case CardType.Water:
                    typeImage.color = new Color32(59, 153, 232, 255);
                    break;
                case CardType.Nature:
                    typeImage.color = new Color32(59, 184, 59, 255);
                    break;
                case CardType.Light:
                    typeImage.color = new Color32(255, 223, 88, 255);
                    break;
                case CardType.Dark:
                    typeImage.color = new Color32(153, 63, 158, 255);
                    break;
                default:
                    typeImage.color = Color.white;
                    break;
            }
        }
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        if (!Application.isPlaying)
            UpdateCardDisplay();
    }
#endif
}
