using UnityEngine;
using System;
using Sounds;
using UI;
using Zenject;

namespace Environment
{
    public class GarbageCan : MonoBehaviour
    {
        [Inject] private SoundService _soundService;
        
        [SerializeField] private float ToggleVolume = 0.3f;
        
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
            if (Player.Player.instance.IsPlayerEnabled && !Player.Player.instance.IsOverlayShowed)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
                if (Physics.Raycast(ray, out RaycastHit hit, 3f) && hit.collider.name == gameObject.name)
                {
                    InFocus = true;
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        MoveToTrash();
                    }
                }
                else
                {
                    InFocus = false;
                }
            }
        }
        
        void MoveToTrash()
        {
            Inventory.instance.RemoveItem();
            _soundService.Play3DSfx(SoundType.GarbageThrow, transform, 3f, ToggleVolume);
        }
    }
}