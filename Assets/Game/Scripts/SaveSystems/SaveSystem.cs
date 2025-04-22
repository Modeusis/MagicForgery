using System.Collections.Generic;
using System.Linq;
using Game.Scripts.AI;
using Player;
using UI;
using UnityEngine;
using Zenject;

namespace Game.Scripts.SaveSystems
{
    public class SaveSystem : MonoBehaviour
    {
        [Inject] private OrderGenerator _orderGenerator;
        
        [SerializeField] private PlayerMovement playerMovement;
        [SerializeField] private List<ItemData> allItems;
        
        [SerializeField] private Vector3 defaultPosition;
        
        private Save LastSave { get; set; }

        private void Awake()
        {
            if (Save.TryLoadSaveFromFile(out Save save))
            {
                LastSave = save;
            }
        }

        public bool TryLoadPlayerPos(out Vector3 pos)
        {
            if (LastSave == null)
            {
                pos = defaultPosition;
                
                return false;
            }
            
            pos = LastSave.PlayerPosition;

            return true;
        }

        public List<ItemData> LoadItemData()
        {
            if (LastSave == null)
            {
                return null;
            }
            
            var list = new List<ItemData>();
            for (int i = 0; i < LastSave.PlayerItems.Count; i++)
            {
                for (int j = 0; j < allItems.Count; j++)
                {
                    if (LastSave.PlayerItems[i] != allItems[j].id)
                        continue;
                    
                    list.Add(allItems[j]);
                }
            }
            
            return list;
        }
        public int LoadPlayerServed() => LastSave.PlayerCustomersServed;
        
        public void SaveGame()
        {
            var list = Inventory.instance.Items.Select(item => item.id).Where(itemName => itemName != "Temp").ToList();
            
            LastSave = new Save(playerMovement.transform.position, list,_orderGenerator.GetCurrentCustomerCounter());
        }

        public void TryToLoadFromFile()
        {
            
        }
    }
}