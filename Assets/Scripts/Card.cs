using UnityEngine;
using System.Collections.Generic;

namespace VillainByMistake.Cards
{
    public enum CardType
    {
        Fire,
        Water,
        Nature,
        Light,
        Dark
    }

    public enum DamageType
    {
        Fire,
        Water,
        Nature,
        Light,
        Dark,
        None
    }
    
    public enum EffectType
    {
        None,
        Poison,        // Damage over time
        Shield,        // Temporary armor
        Regeneration,          // Restore health
    }
    
    [CreateAssetMenu(fileName = "NewCard", menuName = "Villain By Mistake/Card")]
    public class Card : ScriptableObject
    {
        [Header("Card Identity")]
        public string cardName;
        public string description;
        public Sprite artwork;

        [Header("Cost (Currently just for simpler programming)")]
        public int energyCost = 1;
        
        [Header("Card Type")]
        public CardType cardType;
        public DamageType damageType;
        
        [Header("Basic Stats")]
        public int damage;         // Direct damage
        public int heal;           // Direct healing
        public int armor;          // Direct armor gain
        
        [Header("Special Effects")]
        public bool hasSpecialEffect = false;
        public EffectType effectType = EffectType.None;
        
        // Effect parameters (only show relevant ones in Inspector)
        [Tooltip("Amount of effect (damage, shield amount, etc.)")]
        public int effectValue;
        
        [Tooltip("How many turns the effect lasts (0 = instant)")]
        public int effectDuration;
        
        [Tooltip("Chance to apply effect (0-100)")]
        [Range(0, 100)]
        public int effectChance = 100;
        
        [Header("Targeting")]
        [Tooltip("Who this effect targets")]
        public TargetType targetType = TargetType.SingleEnemy;
        
        public enum TargetType
        {
            Self,
            SingleEnemy,
            AllEnemies,
        }
        
        // Helper method for description
        public string GetEffectDescription()
        {
            if (!hasSpecialEffect || effectType == EffectType.None)
                return description;
                
            string effectText = effectType.ToString();
            
            switch (effectType)
            {
                case EffectType.Poison:
                    return $"{description}\nApply {effectValue} poison for {effectDuration} turns";
                    
                case EffectType.Shield:
                    return $"{description}\nGain {effectValue} shield";
                    
                default:
                    return $"{description}\n{effectType}: {effectValue}";
            }
        }
    }
}