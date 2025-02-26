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
        [SerializeField] private GameObject pickup;
        [SerializeField] private Material outlineMaterial;
        
        private List<Material> _pickupMaterials;
        private Material _originalMaterial;
        private Renderer _rend;

        private bool _inFocus;

        private bool InFocus
        {
            get => _inFocus;
            set
            {
                if (_inFocus == value)
                    return;
                _inFocus = value;
                if (_rend)
                {
                    if (_inFocus)
                    {
                        Debug.Log("In Focus");
                        _pickupMaterials.Add(outlineMaterial);
                        _rend.materials = _pickupMaterials.ToArray();
                    }
                    else
                    {
                        _pickupMaterials.Remove(outlineMaterial);
                        _rend.materials = _pickupMaterials.ToArray();
                        Debug.Log("Out Focus");
                    }
                }
            }
        }
        
        private void Awake()
        {
            if (!pickup)
            {
                _rend = GetComponent<Renderer>();
            }

            if (_rend)
            {
                _pickupMaterials = new List<Material>(_rend.materials);
            }
        }

        private void Update()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out RaycastHit hit, 3f) && hit.collider.name == gameObject.name)
            {
                if (_rend)
                {
                    InFocus = true;
                }
                if (Input.GetKeyDown(KeyCode.E))
                {
                    PickUp();
                }
            }
            else
            {
                InFocus = false;
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