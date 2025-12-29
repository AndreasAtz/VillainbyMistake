using UnityEngine;
using System.Collections.Generic;
using VillainByMistake.Cards;

namespace VillainByMistake.Characters
{
    public class Rigby : Character
    {
        [Header("First Character Specific")]
        public int upgradeAmount = 1; // How much to upgrade cards
        
        // Reference to hand (will be set by GameManager)
        private List<Card> hand;
        
        public void SetHandReference(List<Card> handReference)
        {
            hand = handReference;
        }
        
        public override void UseActiveAbility()
        {
            Debug.Log($"{characterName} uses active ability: Upgrade random card");
            
            if (hand == null || hand.Count == 0)
            {
                Debug.Log("No cards in hand to upgrade!");
                return;
            }
            
            // Get random card from hand
            int randomIndex = Random.Range(0, hand.Count);
            Card cardToUpgrade = hand[randomIndex];
            
            // Simple upgrade for alpha: Increase damage or heal by 1
            if (cardToUpgrade.damage > 0)
            {
                cardToUpgrade.damage += upgradeAmount;
                Debug.Log($"Upgraded {cardToUpgrade.cardName}: Damage +{upgradeAmount}");
            }
            else if (cardToUpgrade.heal > 0)
            {
                cardToUpgrade.heal += upgradeAmount;
                Debug.Log($"Upgraded {cardToUpgrade.cardName}: Heal +{upgradeAmount}");
            }
            else if (cardToUpgrade.armor > 0)
            {
                cardToUpgrade.armor += upgradeAmount;
                Debug.Log($"Upgraded {cardToUpgrade.cardName}: Armor +{upgradeAmount}");
            }
            
            // Optional: Visual feedback
            // StartCoroutine(ShowUpgradeEffect(cardToUpgrade));
        }
        
        public override string GetActiveAbilityDescription()
        {
            return $"Upgrade a random card in hand: +{upgradeAmount} to its main stat";
        }
        
        // Override death check for passive
        protected override void CheckDeath()
        {
            if (currentHealth <= 0)
            {
                Debug.Log($"{characterName} would die, but his passive prevents it for the run!");
                currentHealth = 1; // Set to 1 instead of dying
                OnHealthChanged?.Invoke();
                
                // In full game, you'd track that the passive was used
                // and remove it for the rest of the run
            }
        }
    }
}