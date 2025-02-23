using UnityEngine;

namespace UI
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Game/ItemData")]
    public class ItemData : ScriptableObject
    {
        [Header("Basic stats")]
        public int id;
        public string itemName;
        public Sprite itemSprite;
        public string itemDescription;

        [Header("Item type")] 
        public bool isWeapon;
        public bool isEnchantable;
        public bool isEssention;
        public bool isInstrument;

        public GameObject prefab;
    }
}