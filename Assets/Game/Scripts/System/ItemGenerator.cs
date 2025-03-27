using UI;
using UnityEngine;

namespace System
{
    public class ItemGenerator
    {
        private int _id;
        private string _itemName;
        private Sprite _itemSprite;
        private string _itemDescription;

        private float _scaleOnPickUp = 1;
        private Quaternion _rotationOnPickUp = Quaternion.identity;
        private GameObject _prefab;

        public bool IsItemDataSet;
        
        public void SetItemData(int id, string itemName, Sprite itemSprite, string itemDescription, GameObject prefab, float scaleOnPickUp, Quaternion rotationOnPickUp)
        {
            _id = id;
            _itemName = itemName;
            _itemSprite = itemSprite;
            _itemDescription = itemDescription;
            _scaleOnPickUp = scaleOnPickUp;
            _rotationOnPickUp = rotationOnPickUp;
            _prefab = prefab;
            
            IsItemDataSet = true;
        }

        public ItemData GenerateItem()
        {
            ItemData scriptableObject = ScriptableObject.CreateInstance<ItemData>();
            
            scriptableObject.id = _id;
            scriptableObject.itemName = _itemName;
            scriptableObject.itemSprite = _itemSprite;
            scriptableObject.itemDescription = _itemDescription;
            scriptableObject.scaleOnPickUp = _scaleOnPickUp;
            scriptableObject.rotationOnPickUp = _rotationOnPickUp;
            scriptableObject.prefab = _prefab;
            
            return scriptableObject;
        }
    }
}