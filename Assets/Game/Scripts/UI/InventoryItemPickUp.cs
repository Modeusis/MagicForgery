using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI
{
    public class InventoryItemPickUp : MonoBehaviour
    {
        [SerializeField] private ItemData itemData;
        [SerializeField] private GameObject pickup;
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.name == gameObject.name)
                {
                    PickUp();
                }
            }
        }

        private void PickUp()
        {
            if (!pickup)
            {
                itemData.prefab = gameObject;
                Inventory.instance.AddItem(itemData);
                gameObject.SetActive(false);
            }
            else
            {
                itemData.prefab = pickup;
                Inventory.instance.AddItem(itemData);
            }
            Player.Player.instance.staffAnimator.SetTrigger("OnInteract");
        }
    }
}