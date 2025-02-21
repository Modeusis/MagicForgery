using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using JetBrains.Annotations;

namespace UI
{
    [RequireComponent(typeof(Image))]
    public class InventorySlot : MonoBehaviour
    {
        public int slotId;
        private bool _isSlotSelected;
        
        [CanBeNull] private InventoryItem _item;
        
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
            _item.IsSelected = true;
        }
    }
}