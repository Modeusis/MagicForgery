using System.Collections.Generic;
using NUnit.Framework;

namespace UI
{
    public class Inventory
    {
        private static Inventory _playerInventory;
        public static Inventory PlayerInventory = _playerInventory ??= new Inventory();
        
        private List<InventoryItem> _items;

        public List<InventoryItem> Items
        {
            get => _items;
            set
            {
                _items = value;
            }
        }

        private Inventory()
        {
            _items = new List<InventoryItem>();
        }

        void AddItem(InventoryItem item)
        {
            Items.Add(item);
        }

        void RemoveItem(InventoryItem item)
        {
            Items.Remove(item);
        }
    }
}