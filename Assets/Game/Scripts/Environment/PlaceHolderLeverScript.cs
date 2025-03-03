using UI;
using UnityEngine;

namespace Environment
{
    public class PlaceHolderLeverScript : MonoBehaviour
    {
        //передалать эту хуятину в рэйкаст с интерфейсом и еще чето там и вообще на плеера рейкасты на плеера
        [SerializeField] private GameObject lever;
        [SerializeField] private KeyCode interactKey = KeyCode.E;
        [SerializeField] private PlaceHolderScript placeHolder;
            
        private Animator _leverAnimator;

        private bool _isLeverInFocus;

        private bool IsLeverInFocus
        {
            get => _isLeverInFocus;
            set
            {
                if (_isLeverInFocus == value)
                    return;
                _isLeverInFocus = value;
                lever.layer = value ? LayerMask.NameToLayer("Interactable") : LayerMask.NameToLayer("Default");
                gameObject.layer = value ? LayerMask.NameToLayer("Interactable") : LayerMask.NameToLayer("Default");
                TooltipController.Instance.TooltipMessage = $"{interactKey.ToString()} to {(_isLeverToggled ? "close" : "open")} sword handler";
                TooltipController.Instance.IsTooltipShowed = value;
            }
        }
        
        private bool _isLeverToggled;
        public bool IsLeverToggled
        {
            get => _isLeverToggled;
            set
            {
                if (_isLeverToggled == value)
                    return;
                _isLeverToggled = value;
                ToggleLever();
                TooltipController.Instance.TooltipMessage = $"{interactKey.ToString()} to {(_isLeverToggled ? "close" : "open")} sword handler";
            }
        }
        private void Awake()
        {
            _leverAnimator = lever.GetComponent<Animator>();
        }

        private void Update()
        {
            if (Player.Player.instance.IsPlayerEnabled)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit, 3f) && hit.collider.gameObject.CompareTag("PlaceHolderLever"))
                {
                    IsLeverInFocus = true;
                    if (Input.GetKeyDown(interactKey))
                    {
                        IsLeverToggled = !IsLeverToggled;
                    }
                }
                else
                {
                    IsLeverInFocus = false; 
                }
            }
        }

        void ToggleLever()
        {
            if (_leverAnimator)
            {
                _leverAnimator.SetBool("IsLeverToggled", IsLeverToggled);
                placeHolder.IsPlaceHolderOpened = IsLeverToggled;
            }
        }
    }
}