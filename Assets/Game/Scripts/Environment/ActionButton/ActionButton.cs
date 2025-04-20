using System;
using DG.Tweening;
using Game.Scripts.Interface;
using Sounds;
using UnityEngine;
using Zenject;

namespace Environment
{
    public class ActionButton : MonoBehaviour, IPressable
    {
        [Inject] private SoundService _soundService;
        
        [SerializeField] private float ToggleVolume = 2f;
        
        [SerializeField] private Transform buttonTransform;
        [SerializeField] private Material buttonMaterial;
        
        [Header("Active color")]
        [SerializeField] private Color buttonActiveColor; 
        [SerializeField] private Color buttonGlowActiveColor;
        
        [Header("Inactive color")]
        [SerializeField] private Color buttonInactiveColor;
        [SerializeField] private Color buttonGlowInactiveColor;
        
        protected event Action OnPressed;        
        
        private Sequence _buttonTween;
        private string _buttonColorParameterName = "_ButtonColor";
        private string _buttonGlowColorParameterName = "_GlowColor";

        public void Press()
        {
            OnPressed?.Invoke();
            ButtonClickAnimation();
            _soundService.Play3DSfx(SoundType.ActionButtonClick, transform, 3f, ToggleVolume);
        }

        private void ButtonClickAnimation()
        {
            _buttonTween?.Kill();
            
            _buttonTween = DOTween.Sequence();

            _buttonTween.Append(buttonTransform.DOLocalMoveY(0.05f, 0.2f)
                .SetEase(Ease.InSine));
            _buttonTween.Append(buttonTransform.DOLocalMoveY(0.1f, 0.1f)
                .SetEase(Ease.Flash));
        }

        protected void SetButtonColor(bool isActive)
        {
            if (!buttonMaterial)
                return;
            
            var color = isActive ? buttonActiveColor : buttonInactiveColor;
            var glowColor = isActive ? buttonGlowActiveColor : buttonGlowInactiveColor;
            
            buttonMaterial.SetColor(_buttonColorParameterName, color);
            buttonMaterial.SetColor(_buttonGlowColorParameterName, glowColor);
        }
    }
}