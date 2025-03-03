using System;
using Game.Scripts.Interface;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class PickUpZone : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private GameObject pickUp;
        [SerializeField] private ItemData itemData;
        
        [SerializeField] private KeyCode interactKey = KeyCode.E;

        private bool _isZoneFocused;
        
        public bool IsZoneFocused
        {
            get => _isZoneFocused;
            set
            {
                if (_isZoneFocused == value)
                    return;
                _isZoneFocused = value;
                TooltipController.Instance.TooltipMessage = $"Press E to take {itemData.itemName}";
                TooltipController.Instance.IsTooltipShowed = value;
            }
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            
        }

        private void Update()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 3f) && hit.collider.gameObject == gameObject)
            {
                IsZoneFocused = true;
                if (Input.GetKeyDown(interactKey))
                {
                    if (!itemData) return;
                    itemData.prefab = pickUp;
                    if (Inventory.instance.AddItem(itemData))
                    {
                        Player.Player.instance.staffAnimator.SetTrigger("OnInteract");
                    }
                    else
                    {
                        TooltipController.Instance.ShowMechanicsDescription("Not enough space");
                    }
                }
            }
            else
            {
                IsZoneFocused = false;
            }
        }
    }
}