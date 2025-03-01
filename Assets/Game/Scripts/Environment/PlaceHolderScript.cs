using UI;
using UnityEngine;

namespace Environment
{
    public class PlaceHolderScript : MonoBehaviour
    {

        [SerializeField] private KeyCode interactKey = KeyCode.E;
        
        [SerializeField] private GameObject swordPlace;

        public bool isPlaceHolderBlocked;
        
        private Animator _placeHolderAnimator;
        
        private bool _isSwordPlaced;
        private bool _isPlaceHolderOpened;
        private bool _isPlaceHolderInFocus;
        
        private bool IsSwordPlaced
        {
            get => _isSwordPlaced;
            set
            {
                if (_isSwordPlaced == value)
                    return;
                _isSwordPlaced = value;
            }
        }
        
        private bool IsPlaceHolderOpened
        {
            get => _isPlaceHolderOpened;
            set
            {
                if (_isPlaceHolderOpened == value)
                    return;
                _isPlaceHolderOpened = value;
                TooltipController.Instance.TooltipMessage = $"{interactKey.ToString()} to {(value ? "close" : "open")}";
            }
        }
        
        private bool IsPlaceHolderInFocus
        {
            get => _isPlaceHolderInFocus;
            set
            {
                if (_isPlaceHolderInFocus == value)
                    return;
                _isPlaceHolderInFocus = value;
                TooltipController.Instance.TooltipMessage = $"{interactKey.ToString()} to {(value ? "close" : "open")}";
                TooltipController.Instance.IsTooltipShowed = value;
                gameObject.layer = value ? LayerMask.NameToLayer("Interactable") : LayerMask.NameToLayer("Default");
                swordPlace.layer = value ? LayerMask.NameToLayer("Interactable") : LayerMask.NameToLayer("Default");
            }
        }
        
        void Awake()
        {
            _placeHolderAnimator = GetComponent<Animator>();
        }

        void Update()
        {
            if (Player.Player.instance.IsPlayerEnabled && !isPlaceHolderBlocked)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit, 2f) && hit.collider.gameObject.CompareTag("PlaceHolder"))
                {
                    IsPlaceHolderInFocus = true;
                    if (Input.GetKeyDown(interactKey))
                    {
                        OpenSwordHandler();
                    }
                }
                else
                {
                    IsPlaceHolderInFocus = false;
                }
            }
            
        }
        
        void OpenSwordHandler()
        {
            _placeHolderAnimator.SetBool("IsOpened", !IsPlaceHolderOpened); 
            IsPlaceHolderOpened = !IsPlaceHolderOpened;
        }

        void PlaceSword()
        {
            
        }
    }
}