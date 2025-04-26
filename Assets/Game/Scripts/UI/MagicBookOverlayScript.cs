using System;
using System.Collections.Generic;
using DG.Tweening;
using Environment;
using Game.Scripts.Tutorial;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class MagicBookOverlayScript : MonoBehaviour
    {
        [Inject] private TutorialController _tutorialController;
        
        [Header("Tutorial")]
        [SerializeField] private int stepId = 4;
        
        [Header("Book overlay settings")]
        [SerializeField] private List<GameObject> pageList;
        [SerializeField] private Button nextButton;
        [SerializeField] private Button previousButton;
        [SerializeField] private MagicBookScript magicBook;
        [SerializeField] private TMP_Text uiTooltip;
        [SerializeField] private GameObject tooltipBackground;
        [SerializeField] private Canvas canvas;
        
        private int _currentPage;
        private int _pageCount;
        
        private CanvasGroup _canvasGroup;
        
        private bool _isTooltipShown;
        private bool _nextEnabled;
        private bool _previousEnabled;

        private bool IsTooltipShown
        {
            get => _isTooltipShown;
            set
            {
                if (_isTooltipShown == value)
                    return;
                _isTooltipShown = value;
                
                tooltipBackground.transform.DOKill();
                
                if (!value)
                {
                    uiTooltip.text = "";
                    tooltipBackground.transform.DOScale(Vector3.zero, 0.2f).OnComplete(() =>
                    {
                        tooltipBackground.SetActive(false);
                    });
                }
                else
                {
                    tooltipBackground.SetActive(true);
                    tooltipBackground.transform.DOScale(Vector3.one, 0.2f);
                }
            }
        }
        private bool NextEnabled
        {
            get => _nextEnabled;
            set
            {
                if (_nextEnabled == value)
                    return;
                _nextEnabled = value;
                nextButton.interactable = _nextEnabled;
            }
        }
        
        private bool PreviousEnabled
        {
            get => _previousEnabled;
            set
            {
                if (_previousEnabled == value)
                    return;
                _previousEnabled = value;
                previousButton.interactable = _previousEnabled;
            }
        }
        
        private int CurrentPage
        {
            get => _currentPage;
            set
            {
                if (_currentPage == value)
                    return;
                _currentPage = value;
                for (int i = 0; i < _pageCount; i++)
                {
                    if (i != _currentPage)
                    {
                        pageList[i].SetActive(false);
                        continue;
                    }
                    pageList[i].SetActive(true);
                }

                if (_currentPage == _pageCount - 1)
                {
                    NextEnabled = false;
                    PreviousEnabled = true;
                }
                else if (_currentPage == 0)
                {
                    NextEnabled = true;
                    PreviousEnabled = false;
                }
                else
                {
                    NextEnabled = true;
                    PreviousEnabled = true;
                }
            }
        }
        
        void Awake()
        {
            _pageCount = pageList.Count;
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        private void OnEnable()
        {
            _canvasGroup.blocksRaycasts = true;
            CurrentPage = 0;
        }
        
        public void NextPage()
        {
            CurrentPage++;
        }

        public void PreviousPage()
        {
            CurrentPage--;
        }

        public void CloseBook()
        {
            gameObject.SetActive(false);
            IsTooltipShown = false;
            Player.Player.instance.IsOverlayShowed = false;
            _canvasGroup.blocksRaycasts = false;
            magicBook.Toggle();
            
            _tutorialController.CompleteStep(stepId);
        }

        public void IconFocused(TooltipButton focusedButton)
        {
            IsTooltipShown = true;
            uiTooltip.text = focusedButton.ButtonInfo;
            focusedButton.IsHovered = true;
            
        }

        public void IconUnfocused(TooltipButton focusedObject)
        {
            IsTooltipShown = false;
            focusedObject.IsHovered = false;
        }

        private void Update()
        {
            if (IsTooltipShown)
            {
                Vector2 mousePosition = Input.mousePosition;
                
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    canvas.transform as RectTransform,
                    mousePosition,
                    canvas.worldCamera,
                    out Vector2 localPoint
                );
                
                tooltipBackground.transform.localPosition = new Vector2(localPoint.x + 150f, localPoint.y + 70);
            }
        }
    }
}