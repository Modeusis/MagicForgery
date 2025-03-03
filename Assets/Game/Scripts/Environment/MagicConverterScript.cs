using DG.Tweening;
using Game.Scripts.Interface;
using UI;
using UnityEngine;

namespace Environment
{
    public class MagicConverterScript : MonoBehaviour, IToggle
    {
        [Header("Objects")]
        [SerializeField] private GameObject magicConverterHead;
        [SerializeField] private GameObject magicCrystal;
        
        [Header("Keycodes")]
        [SerializeField] private KeyCode interactKey = KeyCode.E;
        
        private bool _isToggled;
        private bool _isFocused;
        
        void AnimateMagicConverter()
        {
            magicConverterHead.transform.DOKill();
            magicCrystal.transform.DOKill();
            
            if (IsToggled)
            {
                magicConverterHead.transform.DOLocalRotate(new Vector3(0, 360f, 0), 3f, RotateMode.FastBeyond360);
                magicConverterHead.transform.DOLocalMove(new Vector3(0, 1f, 0), 3f);
                
                magicCrystal.transform.DOLocalMove(new Vector3(0, 0.7f, 0), 3f);
                magicCrystal.transform.DOLocalRotate(new Vector3(0, 360f, 0), 3f, RotateMode.FastBeyond360);
                
            }
            else
            {
                
                magicConverterHead.transform.DOLocalRotate(new Vector3(0, -360f, 0), 3f, RotateMode.FastBeyond360);
                magicConverterHead.transform.DOLocalMove(new Vector3(0, 0.6f, 0), 3f);
                
                magicCrystal.transform.DOLocalRotate(new Vector3(90f, -360f, 0), 3f, RotateMode.FastBeyond360);
                magicCrystal.transform.DOLocalMove(new Vector3(0, 0.5f, 0), 3f);
                

            }
        }

        public bool IsFocused
        {
            get => _isFocused;
            set
            {
                if (_isFocused == value)
                    return;
                _isFocused = value;
                TooltipController.Instance.TooltipMessage = $"Press {interactKey} to {(IsToggled ? "stop" : "start")} magic converter";
                TooltipController.Instance.IsTooltipShowed = value;
                gameObject.layer = value ? LayerMask.NameToLayer("Interactable") : LayerMask.NameToLayer("Default");
                magicConverterHead.layer = value ? LayerMask.NameToLayer("Interactable") : LayerMask.NameToLayer("Default");
                magicCrystal.layer = value ? LayerMask.NameToLayer("Interactable") : LayerMask.NameToLayer("Default");
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
                TooltipController.Instance.TooltipMessage = $"Press {interactKey} to {(IsToggled ? "stop" : "start")} magic converter";
                AnimateMagicConverter();
            }
        }
        public void Toggle()
        {
            if (MagicEngineController.Instance.IsEngineWorking)
            {
                IsToggled = !IsToggled;
            }
            else
            {
                TooltipController.Instance.ShowMechanicsDescription("Currently unavailable");
            }
        }
    }
}