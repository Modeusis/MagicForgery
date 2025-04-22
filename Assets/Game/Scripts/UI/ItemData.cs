using UnityEngine;

namespace UI
{
    [CreateAssetMenu(menuName = "Setups/Item Data Config")]
    public class ItemData : ScriptableObject
    {
        [Header("Basic stats")]
        public string id;
        public string itemName;
        public Sprite itemSprite;
        public string itemDescription;

        public float scaleOnPickUp = 1;
        public Quaternion rotationOnPickUp = Quaternion.identity;
        public GameObject prefab;

        public bool isPotion;
    }
}