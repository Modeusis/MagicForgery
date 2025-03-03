using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TooltipController : MonoBehaviour
    {
        public static TooltipController Instance;

        [SerializeField] private GameObject tooltipBorder;
        [SerializeField] private GameObject mechanicsBorder;
        [SerializeField] private TMP_Text tooltipText;
        [SerializeField] private TMP_Text mechanicsDescription;
        
        private string _tooltipMessage;
        private Image _tooltipImage;
        private Image _mechanicsImage;
        
        public string TooltipMessage
        {
            get => _tooltipMessage;
            set
            {
                if (_tooltipMessage == value)
                    return;
                _tooltipMessage = value;
                tooltipText.text = _tooltipMessage;
            }
        }
        
        private bool _isTooltipShowed;

        public bool IsTooltipShowed
        {
            get => _isTooltipShowed;
            set
            {
                if (_isTooltipShowed == value)
                    return;
                _isTooltipShowed = value;
                if (_isTooltipShowed)
                {
                    ShowTooltip(_tooltipMessage);
                }
                else
                {
                    HideTooltip();
                }
            }
        }
        
        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
                _tooltipImage = tooltipBorder.GetComponent<Image>();
                _mechanicsImage = mechanicsBorder.GetComponent<Image>();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        void ShowTooltip(string text)
        {
            _tooltipImage.DOKill();
            tooltipText.DOKill();
            
            tooltipText.text = text;
            
            _tooltipImage.DOFade(1, .6f);
            tooltipText.DOFade(1, .6f);
        }

        void HideTooltip()
        {
            _tooltipImage.DOKill();
            tooltipText.DOKill();

            tooltipText.DOFade(0, .6f);
            _tooltipImage.DOFade(0, .6f).OnComplete(() =>
            {
                tooltipText.text = string.Empty;
            });
        }

        public void ShowMechanicsDescription(string text, float duration = 5f)
        {
            _mechanicsImage.DOKill();
            mechanicsDescription.DOKill();
            
            DOTween.Kill("MechanicsBorderDescription");
            DOTween.Kill("MechanicsDescription");
            
            mechanicsDescription.text = text;
            
            Sequence textSequence = DOTween.Sequence();
            Sequence sequence = DOTween.Sequence();
            
            sequence.Append(_mechanicsImage.DOFade(1, 1f));
            sequence.Append(_mechanicsImage.DOFade(0, 1f).SetDelay(duration));
            
            textSequence.Append(mechanicsDescription.DOFade(1, 1f));
            textSequence.Append(mechanicsDescription.DOFade(0, 1f).SetDelay(duration));

            sequence.SetId("MechanicsBorderDescription");
            textSequence.SetId("MechanicsDescription");
        }
    }
}