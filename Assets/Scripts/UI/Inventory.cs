using System.Collections.Generic;
using NUnit.Framework;

namespace UI
{
    public class Inventory
    {
        private static Inventory _playerInventory;
        public static Inventory PlayerInventory = _playerInventory ??= new Inventory();
        
        private List<Item> _items;

        public List<Item> Items
        {
            get => _items;
            set
            {
                _items = value;
            }
        }
        
        
    }
}