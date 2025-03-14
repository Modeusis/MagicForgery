using System;
using Game.Scripts.Interface;
using UI;
using UnityEngine;

namespace Environment
{
    public class ManaControllerScript : MonoBehaviour, IToggle
    {
        [SerializeField] private ParticleSystem manaEffect;
        [SerializeField] private int manaTransferValue;
        
        private bool _isFocused;
        private bool _isToggled;
        
        
        public bool IsFocused
        {
            get => _isFocused;
            set
            {
                if (_isFocused == value)
                    return;
                _isFocused = value;
                gameObject.layer = value ? LayerMask.NameToLayer("Interactable") : LayerMask.NameToLayer("Default");
                TooltipController.Instance.IsTooltipShowed = value;
                TooltipController.Instance.TooltipMessage =
                    $"{Player.Player.instance.InteractKey} to add mana to controller";
            }
        }

        public bool IsToggled
        {
            get => _isToggled;
            set
            {
                if (_isToggled == value)
                    return;
                if (Player.Player.instance.CurrentMana < manaTransferValue)
                {
                    TooltipController.Instance.ShowMechanicsDescription("Not enough mana to transfer");
                    return;
                }

                TransferMana(manaTransferValue);
                
            }
        }
        public void Toggle()
        {
            IsToggled = !IsToggled;
            manaEffect.Play();
        }

        void TransferMana(int value)
        {
            int valueToTransfer = MagicEngineController.Instance.AddMana(value);
            Player.Player.instance.CurrentMana -= valueToTransfer;
        }
    }
}