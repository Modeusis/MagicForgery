using Game.Scripts.Interface;
using UI;
using UnityEngine;

namespace Environment
{
    public class PlaceHolderLeverScript : MonoBehaviour, IToggle
    {
        //передалать эту хуятину в рэйкаст с интерфейсом и еще чето там и вообще на плеера рейкасты на плеера
        [SerializeField] private GameObject lever;
        [SerializeField] private KeyCode interactKey = KeyCode.E;
        [SerializeField] private PlaceHolderScript placeHolder;
            
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
                TooltipController.Instance.TooltipMessage = $"{interactKey.ToString()} to {(_isToggled ? "close" : "open")} place holder";
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
                placeHolder.IsPlaceHolderOpened = IsToggled;
                TooltipController.Instance.TooltipMessage = $"{interactKey.ToString()} to {(_isToggled ? "close" : "open")} place holder";
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