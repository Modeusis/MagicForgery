using System;
using UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace Environment
{
    public class MagicSphereScript : MonoBehaviour
    {
        private bool _isMagicSphereToggled;
        private bool _isMagicSphereInFocus;
        
        public bool isMagicSphereBlocked;
        
        private Animator _magicSphereAnimator;
        
        [SerializeField] private KeyCode interactKey = KeyCode.E;
        
        private bool IsMagicSphereToggled
        {
            get => _isMagicSphereToggled;
            set
            {
                if (_isMagicSphereToggled == value)
                    return;
                _isMagicSphereToggled = value;
                ToggleMagicSphere();
                TooltipController.Instance.TooltipMessage = $"{interactKey.ToString()} to toggle magic sphere";
            }
        }
        
        private bool IsMagicSphereInFocus
        {
            get => _isMagicSphereInFocus;
            set
            {
                if (_isMagicSphereInFocus == value)
                    return;
                _isMagicSphereInFocus = value;
                TooltipController.Instance.TooltipMessage = $"{interactKey.ToString()} to toggle magic sphere";
                TooltipController.Instance.IsTooltipShowed = value;
                gameObject.layer = value ? LayerMask.NameToLayer("Interactable") : LayerMask.NameToLayer("Default");
            }
        }

        private void Update()
        {
            if (Player.Player.instance.IsPlayerEnabled)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit, 3f) && hit.collider.gameObject.CompareTag("MagicSphere"))
                {
                    IsMagicSphereInFocus = true;
                    if (Input.GetKeyDown(interactKey) && !isMagicSphereBlocked)
                    {
                        IsMagicSphereToggled = !IsMagicSphereToggled;
                    }
                }
                else
                {
                    IsMagicSphereInFocus = false;
                }
            }
        }

        void ToggleMagicSphere()
        {
            Debug.Log("Toggle magic sphere");
        }

        void AnimateMagicSphere()
        {
            
        }
    }
}