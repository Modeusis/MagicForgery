using System;
using DG.Tweening;
using Game.Scripts.Interface;
using UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace Environment
{
    public class MagicSphereScript : MonoBehaviour, IToggle
    {
        [SerializeField] private PlaceHolderScript placeHolder;
        [SerializeField] private GameObject magicSphereCanvasGroup;
        [SerializeField] private QuestionSpriteScript questionMark;
        [SerializeField] private GameObject magicSpherePlace;
        
        private bool _isToggled;
        private bool _isFocused;
        private Sword _sword;
        
        public bool IsBlocked { get; set; }
        void AnimateMagicSphere()
        {
            transform.DOKill();

            if (IsToggled)
            {
                transform.DOLocalMove(new Vector3(2.54799962f, 2.2f, -0.584999979f), 1f).SetEase(Ease.OutSine);
                transform.DOLocalRotate(new Vector3(0, 360, 0), 1f, RotateMode.FastBeyond360).SetEase(Ease.OutSine)
                    .OnComplete(() =>
                    {
                        transform.DOLocalRotate(new Vector3(0, 20, 0), .5f, RotateMode.FastBeyond360)
                            .SetEase(Ease.Linear)
                            .SetLoops(-1, LoopType.Incremental);
                    });
            }
            else
            {
                transform.DOLocalRotate(new Vector3(0, 360, 0), 1f, RotateMode.FastBeyond360).SetEase(Ease.InSine);
                transform.DOLocalMove(new Vector3(2.54799962f,1.9174999f,-0.584999979f), 1f).SetEase(Ease.InSine);
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
                TooltipController.Instance.TooltipMessage =
                    $"Press {Player.Player.instance.InteractKey} to toggle sphere";
                TooltipController.Instance.IsTooltipShowed = value;
                magicSpherePlace.layer = value ? LayerMask.NameToLayer("Interactable") : LayerMask.NameToLayer("Default");
            }
        }

        public bool IsToggled
        {
            get => _isToggled;
            set
            {
                if (_isToggled == value)
                    return;
                if (ValidateMagicSphereToggle())
                {
                    if (value)
                    {
                        _isToggled = true;
                        AnimateMagicSphere();
                        magicSphereCanvasGroup.SetActive(true);
                        Player.Player.instance.IsOverlayShowed = true; 
                    }
                    else
                    {
                        _isToggled = false;
                        AnimateMagicSphere();
                        magicSphereCanvasGroup.SetActive(false);
                        Player.Player.instance.IsOverlayShowed = false;
                    }
                }
            }
        }
        public void Toggle()
        {
            if (IsBlocked)
                return;
            IsToggled = !IsToggled;
        }

        bool ValidateMagicSphereToggle()
        {
            if (!MagicEngineController.Instance.IsEngineWorking)
            {
                TooltipController.Instance.ShowMechanicsDescription("No mana power from engine");
                return false;
            }
                
            return true;
        }

        private void Update()
        {
            if (!IsToggled)
            {
                if (MagicEnchanterController.Instance.SwordEnchantment && MagicEnchanterController.Instance.SwordToEnchant 
                                                                       && !MagicEnchanterController.Instance.IsEnchanting)
                {
                    questionMark.IsVisible = true;
                }
                else
                {
                    questionMark.IsVisible = false;
                }
            }
            else
            {
                questionMark.IsVisible = false;
            }
        }
    }
}