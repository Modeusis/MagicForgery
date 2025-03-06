using UnityEngine;

namespace UI
{
    public class Enchantment :ScriptableObject
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
        //Мб particle system добавить
    }
}