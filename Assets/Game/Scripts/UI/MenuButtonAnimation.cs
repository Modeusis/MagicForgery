using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Button))]
    public class MenuButtonAnimation : MonoBehaviour
    {
        [SerializeField] private float scaleSpeed = 0.5f;
        [SerializeField] private float clickDuration = 0.2f;

        [SerializeField] private float scaleOnEnter = 1.5f;
        [SerializeField] private float scaleOnClick = 0.8f;
        
        private float _startScale;
        
        private Button _button;
        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void OnMouseDown()
        {
            _button.transform.DOKill();

            _button.transform.DOScale(_startScale * scaleOnClick, clickDuration)
                .OnComplete(() =>
                {
                    _button.transform.DOScale(_startScale, clickDuration);
                });
        }

        private void OnMouseEnter()
        {
            _button.transform.DOKill();

            _button.transform.DOScale(_startScale * scaleOnEnter, scaleSpeed);
        }

        private void OnMouseExit()
        {
            _button.transform.DOKill();
            
            _button.transform.DOScale(_startScale, scaleSpeed);
        }
    }
}