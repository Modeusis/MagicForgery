using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using Player;

namespace UI
{
    public class InventoryMenuManager : MonoBehaviour
    {
        [Header("UI Keys")]
        [SerializeField] private KeyCode openInventoryKey = KeyCode.Tab;
        
        [Header("Inventory")]
        [SerializeField] private GameObject inventoryMenu;
        [SerializeField] private List<InventorySlot> inventorySlots;
        
        private bool _isInventoryOpen;
        private int? _toggledSlot;

        private int? ToggledSlot
        {
            get => _toggledSlot;
            set
            {
                if (!value.HasValue)
                    return;
                if (_toggledSlot == value)
                    return;
                _toggledSlot = value;
                foreach (InventorySlot slot in inventorySlots)
                {
                    slot.IsSlotToggled = slot.slotId == _toggledSlot; 
                }
            }
        }
        private Vector3 _mousePosition;
        private bool IsInventoryOpen
        {
            get => _isInventoryOpen;
            set
            {
                if (_isInventoryOpen == value)
                    return;
                _isInventoryOpen = value;
                ToggleMenu();
                _mousePosition = Input.mousePosition;
            }
        }

        private float _mouseX;
        private float _mouseY;
        
        void Start()
        {
            inventoryMenu.SetActive(false);
        }

        void Update()
        {
            IsInventoryOpen = Input.GetKey(openInventoryKey);
            if (IsInventoryOpen)
            {
                ItemSelect();
            }
        }
        
        void ToggleMenu()
        {
            inventoryMenu.SetActive(IsInventoryOpen);
            if (!IsInventoryOpen && ToggledSlot != null)
            {
                var selectedSlot = inventorySlots.Find(x => x.slotId == ToggledSlot);
                foreach (var slot in inventorySlots)
                {
                    slot.IsSlotSelected = slot.slotId == selectedSlot.slotId;
                }
            }
            Player.Player.instance.IsPlayerEnabled = !IsInventoryOpen;
        }

        void ItemSelect()
        {
            _mouseX = Input.mousePosition.x;
            _mouseY = Input.mousePosition.y;
            
            float mouseXGapFromCenter = Mathf.Clamp(_mouseX - _mousePosition.x, -300f, 300f);
            float mouseYGapFromCenter = Mathf.Clamp(_mouseY - _mousePosition.y, -300f, 300f);
            
            if (mouseXGapFromCenter < -30 && Mathf.Abs(mouseXGapFromCenter) > Mathf.Abs(mouseYGapFromCenter))
            {
                ToggledSlot = 0;
            }
            else if (mouseYGapFromCenter > 30 && Mathf.Abs(mouseXGapFromCenter) < Mathf.Abs(mouseYGapFromCenter))
            {
                ToggledSlot = 1;
            }
            else if (mouseXGapFromCenter > 30 && Mathf.Abs(mouseXGapFromCenter) > Mathf.Abs(mouseYGapFromCenter))
            {
                ToggledSlot = 2;
            }
            else if (mouseYGapFromCenter < -30 && Mathf.Abs(mouseXGapFromCenter) < Mathf.Abs(mouseYGapFromCenter))
            {
                ToggledSlot = 3;
            }
            else
            {
                ToggledSlot = null;
            }
        }
        
    }
}