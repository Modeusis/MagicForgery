using System;
using Game.Scripts.Interface;
using UI;
using UnityEngine;

namespace Environment
{
    public class EngineLeverScript : MonoBehaviour, IToggle
    {
        [SerializeField] private GameObject lever;
        [SerializeField] private KeyCode interactKey = KeyCode.E;
        
        private Animator _leverAnimator;
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
                lever.layer = value ? LayerMask.NameToLayer("Interactable") : LayerMask.NameToLayer("Default");
                gameObject.layer = value ? LayerMask.NameToLayer("Interactable") : LayerMask.NameToLayer("Default");
                TooltipController.Instance.TooltipMessage = $"{interactKey.ToString()} to {(_isToggled ? "shutdown" : "start")} engine";
                TooltipController.Instance.IsTooltipShowed = value;
            }
        }
        
        public bool IsToggled
        {
            get => _isToggled;
            set
            {
                if (_isToggled == value)
                    return;
                _isToggled = value;
                _leverAnimator.SetBool("IsLeverToggled", _isToggled);
                MagicEngineController.Instance.IsEngineWorking = _isToggled;
                TooltipController.Instance.TooltipMessage = $"{interactKey.ToString()} to {(_isToggled ? "shutdown" : "start")} engine";
            }
        }
        private void Awake()
        {
                _leverAnimator = lever.GetComponent<Animator>();
        }
        public void Toggle()
        {
            IsToggled = !IsToggled;
        }
    }
}

