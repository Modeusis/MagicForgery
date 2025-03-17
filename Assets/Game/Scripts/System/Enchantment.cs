using UnityEngine;

namespace UI
{
    [CreateAssetMenu(menuName = "Game/Enchantment")]    
    public class Enchantment : ScriptableObject
    {
        [Header("Enchantment General")]
        public string enchantmentName;
        public string enchantmentDescription;
        
        [Header("Enchantment Damage")]
        public float physicalBonusDamage;
        public float fireBonusDamage;
        public float iceBonusDamage;
        public float lightningBonusDamage;
        public float poisonBonusDamage;

        [Header("Visuals")] 
        public Material enchantedBladeMaterial;
        public Color enchantmentColor;
        
        [Header("Requirements")] 
        public int manaCost = 10;
        public int waterCost = 10;
    }
}