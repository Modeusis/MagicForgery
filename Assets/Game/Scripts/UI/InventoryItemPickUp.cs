using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI
{
    public class InventoryItemPickUp : MonoBehaviour
    {
        [SerializeField] private ItemData itemData;

        private bool _inFocus;

        private bool InFocus
        {
            get => _inFocus;
            set
            {
                if (_inFocus == value)
                    return;
                _inFocus = value;
                gameObject.layer = LayerMask.NameToLayer(_inFocus ? "Interactable" : "Default");
            }
        }
        private void Update()
        {
            if (Player.Player.instance.IsPlayerEnabled)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
                if (Physics.Raycast(ray, out RaycastHit hit, 3f) && hit.collider.name == gameObject.name)
                {
                    InFocus = true;
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        InFocus = false;
                        PickUp();
                    }
                }
                else
                {
                    InFocus = false;
                }
            }
        }

        private void PickUp()
        {
            itemData.prefab = gameObject;
            var result = Inventory.instance.AddItem(itemData);
            if (result) 
            {
               gameObject.SetActive(false);
            }
            Player.Player.instance.staffAnimator.SetTrigger("OnInteract");
        }
    }
}