using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using JetBrains.Annotations;
using TMPro;
using UnityEngine.Serialization;

namespace UI
{
    public class InventorySlot : MonoBehaviour
    {
        public int slotId;
        private bool _isSlotSelected;
        
        [CanBeNull] private ItemData item;

        [CanBeNull]
        public ItemData Item
        {
            get => item;
            set
            {
                if (item == value)
                    return;
                if (value == null)
                {
                    ItemUnset();
                    item = value;
                }
                item = value;
                ItemSet();
            }
        }
        
        public bool IsSlotSelected
        {
            get => _isSlotSelected;
            set
            {
                if (_isSlotSelected == value)
                    return;
                _isSlotSelected = value;
                SelectItem();
            }
        }

        
        
        private bool _isSlotToggled;
        
        public bool IsSlotToggled
        {
            get => _isSlotToggled;
            set
            {
                if (_isSlotToggled == value)
                    return;
                _isSlotToggled = value;
                ToggleSlot();
            }
        }
        void ItemSet()
        {
            var itemSprite = transform.Find("ItemSprite").GetComponent<Image>();
            var itemName = transform.Find("ItemName").GetComponent<TMP_Text>();
            itemName.color = new Color(255f, 215f, 0f, 1f);
            itemName.text = Item.itemName;
            itemSprite.sprite = Item.itemSprite;
            itemSprite.color = new Color(1f, 1f, 1f, 1f);
        }

        void ItemUnset()
        {
            var itemSprite = transform.Find("ItemSprite").GetComponent<Image>();
            var itemName = transform.Find("ItemName").GetComponent<TMP_Text>();
            itemName.color = new Color(1f, 1f, 1f, 0f);
            itemName.text = "";
            itemSprite.sprite = null;
            itemSprite.color = new Color(1f, 1f, 1f, 0f);
        }
        private void ToggleSlot()
        {
            if (IsSlotToggled)
            {
                transform.DOScale(new Vector3(1.2f, 1.2f, 1f), .4f);
            }
            else
            {
                transform.DOScale(Vector3.one, .4f);
            }
        }

        private void SelectItem()
        {
            if (item)
            {
                
            }
        }
    }
}