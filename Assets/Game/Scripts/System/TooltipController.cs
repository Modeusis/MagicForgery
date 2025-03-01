using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace UI
{
    public class TooltipController : MonoBehaviour
    {
        public static TooltipController Instance;
        
        [SerializeField] private TMP_Text tooltipText;
        [SerializeField] private TMP_Text mechanicsDescription;
        
        private string tooltipMessage;

        public string TooltipMessage
        {
            get => tooltipMessage;
            set
            {
                if (tooltipMessage == value)
                    return;
                tooltipMessage = value;
                tooltipText.text = tooltipMessage;
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
                    ShowTooltip(tooltipMessage);
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
            }
            else
            {
                Destroy(gameObject);
            }
        }

        void ShowTooltip(string text)
        {
            tooltipText.DOKill();
            
            tooltipText.text = text;
            tooltipText.DOFade(1, 1f);
        }

        void HideTooltip()
        {
            tooltipText.DOKill();
            
            tooltipText.DOFade(0, .6f).OnComplete(() =>
            {
                tooltipText.text = string.Empty;
            });
        }
    }
}