using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Button))]
    public class MenuButtonAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
    {
        [SerializeField] private float scaleSpeed = 0.5f;
        [SerializeField] private float clickDuration = 0.2f;

        [SerializeField] private float scaleOnEnter = 1.1f;
        [SerializeField] private float scaleOnClick = 0.9f;
        
        private float _startScale;
        private Vector3 _startScaleVector;

        private void Awake()
        {
            _startScaleVector = transform.localScale;
            _startScale = _startScaleVector.x;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            transform.DOKill();
            
            Sequence clickSequence = DOTween.Sequence();
            clickSequence.Append(transform.DOScale(_startScale * scaleOnClick, clickDuration));
            clickSequence.Append(transform.DOScale(_startScale, clickDuration));
            clickSequence.SetUpdate(true);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            transform.DOKill();
            transform.DOScale(_startScale * scaleOnEnter, scaleSpeed)
                .SetUpdate(true);;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            transform.DOKill();
            transform.DOScale(_startScale, scaleSpeed)
                .SetUpdate(true);
        }

        private void OnDisable()
        {
            transform.DOKill();
        }
    }
}