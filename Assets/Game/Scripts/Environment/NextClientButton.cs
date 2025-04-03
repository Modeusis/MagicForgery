using System;
using DG.Tweening;
using Game.Scripts.AI;
using Game.Scripts.Interface;
using UnityEngine;
using UnityEngine.Events;

namespace Environment
{
    [RequireComponent(typeof(BoxCollider))]
    public class NextClientButton : MonoBehaviour, IPressable
    {
        [SerializeField] private Transform buttonTransform;
        [SerializeField] private Material buttonMaterial;
        [SerializeField] private CustomerGenerator customerSpawner;
        
        [Header("Active color")]
        [SerializeField] private Color buttonActiveColor; 
        [SerializeField] private Color buttonGlowActiveColor;
        
        [Header("Inactive color")]
        [SerializeField] private Color buttonInactiveColor;
        [SerializeField] private Color buttonGlowInactiveColor;
        
        private event Action OnPressed;        
        
        private Sequence _buttonTween;
        private string _buttonColorParameterName = "_ButtonColor";
        private string _buttonGlowColorParameterName = "_GlowColor";

        private void OnEnable()
        {
            SetButtonColor(true);
            OnPressed += customerSpawner.GenerateCustomer;
            customerSpawner.OnCustomerSpawnChanged += SetButtonColor;
        }

        private void OnDisable()
        {
            OnPressed -= customerSpawner.GenerateCustomer;
            customerSpawner.OnCustomerSpawnChanged -= SetButtonColor;
        }

        public void Press()
        {
            OnPressed?.Invoke();
            ButtonClickAnimation();
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

        private void SetButtonColor(bool isActive)
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