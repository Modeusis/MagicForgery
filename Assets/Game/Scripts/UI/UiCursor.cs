using System;
using UnityEngine;

namespace UI
{
    public class UiCursor : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;
        
        private bool _isActive;
        
        public bool IsActive
        {
            get => _isActive;
            set
            {
                if (_isActive == value)
                    return;
                _isActive = value;
                gameObject.SetActive(_isActive);
            }
        }

        private void Update()
        {
            if (IsActive)
            {
                Vector2 mousePosition = Input.mousePosition;

                // Переводим позицию курсора в координаты Canvas
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    canvas.transform as RectTransform,
                    mousePosition,
                    canvas.worldCamera,
                    out Vector2 localPoint
                );
                
                transform.localPosition = new Vector2(localPoint.x + 30f, localPoint.y - 25f);
            }
        }
    }
}