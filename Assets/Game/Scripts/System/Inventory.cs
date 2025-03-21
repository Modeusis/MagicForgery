using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace UI
{
    public class Inventory : MonoBehaviour
    {
        //Полностью переделать
        public static Inventory instance;
        [SerializeField] private List<ItemData> items;
        [SerializeField] private List<InventorySlot> slots;
        
        
        private void Awake()
        {
            if (!instance)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }
            
            items.Capacity = 4;
            slots.Capacity = 4;
        }
        
        public bool AddItem(ItemData item)
        {
            InventorySlot slot = slots.Find(x => !x.Item);
            if (slot && items.Count < items.Capacity)
            {
                items.Add(item);
                slot.Item = item;

                if (!Player.Player.instance.selectedItem)
                {
                    slot.IsSlotSelected = true;
                }
                
                return true;
            } 
            TooltipController.Instance.ShowMechanicsDescription("You cannot add more items");
            return false;
        }

        public void RemoveItem()
        {
            InventorySlot slot = slots.Find(x => x.IsSlotSelected);
            if (slot && slot.Item)
            {
                items.Remove(slot.Item);
                slot.IsSlotSelected = false;
                Player.Player.instance.selectedItem = null;
                slot.Item = null;
            }
        }
    }
}