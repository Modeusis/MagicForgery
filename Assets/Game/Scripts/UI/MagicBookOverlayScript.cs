using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MagicBookOverlayScript : MonoBehaviour
    {
        [SerializeField] private List<GameObject> pageList;
        [SerializeField] private Button nextButton;
        [SerializeField] private Button previousButton;
        
        private int _currentPage;
        private int _pageCount;
        
        private bool _nextEnabled;
        private bool _previousEnabled;

        private bool NextEnabled
        {
            get => _nextEnabled;
            set
            {
                _nextEnabled = value;
            }
        }
        
        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                if (_currentPage != value)
                    return;
                _currentPage = value;
            }
        }
        
        void Awake()
        {
            _pageCount = pageList.Count;
        }

        void OpenBook()
        {
            gameObject.SetActive(true);
        }
        
        void NextPage()
        {
            CurrentPage++;
            if (CurrentPage == pageList.Count)
            {
                nextButton.interactable = false;
            }
        }

        void PreviousPage()
        {
            CurrentPage--;
            if (CurrentPage == pageList.Count)
            {
                nextButton.interactable = false;
            }
        }

        void CloseBook()
        {
            
        }
    }
}