using UI;
using UnityEngine;

namespace Environment
{
    public class Sword : MonoBehaviour
    {
        private Enchantment _swordEnchantment;

        public Enchantment SwordEnchantment
        {
            get => _swordEnchantment;
            set
            {
                if (_swordEnchantment == value)
                    return;
                _swordEnchantment = value;
                IsEnchanted = _swordEnchantment;
            }
        }
        
        public bool IsEnchanted { get; set; }

        public float EnchantmentAccuracy { get; private set; }
        public float PhysicalBonusDamageByAccuracy { get; private set; }
        public float PoisonBonusDamageByAccuracy { get; private set; }
        public float LightningBonusDamageByAccuracy { get; private set; }
        public float IceBonusDamageByAccuracy { get; private set; }
        public float FireBonusDamageByAccuracy { get; private set; }

        void SetAccuracy(float accuracy)
        {
            if (accuracy is >= 0 and <= 1)
            {
                EnchantmentAccuracy = accuracy;
                
                PhysicalBonusDamageByAccuracy = accuracy * SwordEnchantment.physicalBonusDamage;
                PoisonBonusDamageByAccuracy = accuracy * SwordEnchantment.poisonBonusDamage;
                LightningBonusDamageByAccuracy = accuracy * SwordEnchantment.lightningBonusDamage;
                IceBonusDamageByAccuracy = accuracy * SwordEnchantment.iceBonusDamage;
                FireBonusDamageByAccuracy = accuracy * SwordEnchantment.fireBonusDamage;
            }
            else
            {
                Debug.Log("Accuracy is out of range.");
            }
            
        }
    }
}