using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace UI
{
    [RequireComponent(typeof(Image))]
    public class InventorySlot : MonoBehaviour
    {
        public int slotId;
        private bool _isSlotSelected;

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
            Debug.Log($"Item {slotId} was selected");
        }
    }
}