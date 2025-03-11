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
        
        private bool _isToggled;
        private bool _isFocused;
        
        public bool isMagicSphereBlocked;
        
        

        void AnimateMagicSphere()
        {
            transform.DOKill();

            if (IsToggled)
            {
                transform.DOLocalMove(new Vector3(2.54799962f,2.5f,-0.584999979f), 1f).SetEase(Ease.OutSine).OnComplete(
                    () =>
                    {
                        transform.DOLocalRotate(new Vector3(0, 360, 0), 1f, RotateMode.FastBeyond360).SetEase(Ease.Linear);
                    });
            }
            else
            {
                transform.DOLocalRotate(Vector3.zero, 1f, RotateMode.FastBeyond360).SetEase(Ease.Linear);
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
                gameObject.layer = value ? LayerMask.NameToLayer("Interactable") : LayerMask.NameToLayer("Default");
                _isFocused = value;
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
                    _isToggled = value;
                }
                AnimateMagicSphere();
            }
        }
        public void Toggle()
        {
            IsToggled = !IsToggled;
        }

        bool ValidateMagicSphereToggle()
        {
            if (!MagicEngineController.Instance.IsEngineWorking)
                return false;
            if (!placeHolder.IsSwordPlaced)
                return false;
            return true;
        }
    }
}