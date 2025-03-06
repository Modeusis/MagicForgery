using System;
using System.Collections.Generic;
using UI;
using UnityEngine;

namespace Environment
{
    [CreateAssetMenu(menuName = "Game/EnchantmentData")]
    public class EnchantmentData : ScriptableObject
    {
        [field:SerializeField] public List<Recipe> Enchantments { get; private set; }
    }

    [Serializable]
    public class Recipe
    {
        [field:SerializeField] public List<Ingredient> Ingredients { get; private set; }
        [field:SerializeField] public Enchantment Enchantment { get; private set; }
    }

    [Serializable]
    public class Ingredient
    {
        [field:SerializeField] public MagicEnchanterController.PotionType PotionType { get; private set; }
        [field:SerializeField, Space] public int Count { get; private set; }
    }
}