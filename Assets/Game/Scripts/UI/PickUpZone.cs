using System;
using Game.Scripts.Interface;
using Sounds;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace UI
{
    public class PickUpZone : MonoBehaviour
    {
        [Inject] private SoundService _soundService;
        
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

        private void Update()
        {
            if (Player.Player.instance.IsPlayerEnabled && !Player.Player.instance.IsOverlayShowed)
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
                            
                            _soundService.Play3DSfx(SoundType.PotionToggle, transform, 4f, 0.6f);
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
}