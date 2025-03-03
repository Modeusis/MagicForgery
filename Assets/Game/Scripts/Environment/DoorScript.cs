using System;
using DG.Tweening;
using Game.Scripts.Interface;
using TMPro;
using UI;
using UnityEngine;

namespace Environment
{
    public class DoorScript : MonoBehaviour, IToggle
    {
        [Header("Sounds")]
        [SerializeField] private AudioClip doorOpenSound;
        [SerializeField] private AudioClip doorCloseSound;
    
        private Animator _animator;
        private bool _isTriggered;
        private Animator _door;
        
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
                TooltipController.Instance.TooltipMessage = $"{Player.Player.instance.InteractKey.ToString()} to {(_isToggled ? "close" : "open")}";
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
                _door.SetBool("IsDoorOpened", _isToggled);
                TooltipController.Instance.TooltipMessage = $"{Player.Player.instance.InteractKey.ToString()} to {(_isToggled ? "close" : "open")}";
                SoundManager.instance.PlaySfx(_isToggled ? doorOpenSound : doorCloseSound);
            }
        }
        public void Toggle()
        {
            IsToggled = !IsToggled;
        }

        private void Awake()
        {
            _door = GetComponent<Animator>();
        }
    }
}
