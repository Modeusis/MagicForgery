using UI;
using UnityEngine;

namespace Environment
{
    public class Sword : MonoBehaviour
    {
        [field:SerializeField] public string SwordName { get; private set; }
        [field:SerializeField] public string SwordDescription { get; private set; }
        [field:SerializeField] public Sprite SwordIcon { get; private set; }
        
        
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
        
        //Serialize для отладки убрать при билде
        [field:SerializeField] public float EnchantmentAccuracy { get; private set; }
        [field:SerializeField] public float PhysicalBonusDamageByAccuracy { get; private set; }
        [field:SerializeField] public float PoisonBonusDamageByAccuracy { get; private set; }
        [field:SerializeField] public float LightningBonusDamageByAccuracy { get; private set; }
        [field:SerializeField] public float IceBonusDamageByAccuracy { get; private set; }
        [field:SerializeField] public float FireBonusDamageByAccuracy { get; private set; }

        public void SetAccuracy(float accuracy)
        {
            if (!IsEnchanted)
            {
                Debug.Log("Sword isn't enchanted");
                return;
            }
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