using UnityEngine;
using System;
using UI;

namespace Environment
{
    public class GarbageCan : MonoBehaviour
    {
        [SerializeField] private AudioClip toTrashSound;

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
            SoundManager.instance.PlaySfx(toTrashSound);
        }
    }
}