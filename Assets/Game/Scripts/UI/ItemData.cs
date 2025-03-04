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

        public float scaleOnPickUp = 1;
        public Quaternion rotationOnPickUp = Quaternion.identity;
        public GameObject prefab;

        public bool isPotion;
    }
}