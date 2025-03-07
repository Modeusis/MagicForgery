using DG.Tweening;
using TMPro;
using UnityEngine;

namespace UI
{
    public class TooltipButton : MonoBehaviour
    {
        [SerializeField, Range(1, 2)] private float scaleFactor = 1.25f;
        [SerializeField] private float scaleTime  = 0.3f;
        [SerializeField] private string buttonInfo;
        
        private bool _isHovered;

        public bool IsHovered
        {
            get => _isHovered;
            set
            {
                if (_isHovered == value)
                    return;
                _isHovered = value;
                transform.DOKill();
                
                if (_isHovered)
                {
                    transform.DOScale(Vector3.one * scaleFactor, scaleTime);
                }
                else
                {
                    transform.DOScale(Vector3.one, scaleTime);
                }
            }
        }
        
        public string ButtonInfo => buttonInfo;
    }
}