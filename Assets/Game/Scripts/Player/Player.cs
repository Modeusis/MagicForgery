using System;
using DG.Tweening;
using Environment;
using Game.Scripts.Interface;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class Player : MonoBehaviour
    {
        public static Player instance;
        
        [Header("UI")]
        [SerializeField] private GameObject cursor;
        [SerializeField] private UiCursor uiCursor;
        [SerializeField] private GameObject background;
        
        [Header("Mana")]
        [SerializeField] private Image manaBar;
        [SerializeField] private int manaCapacity = 100;
        
        [Header("Keys")]
        [SerializeField] private KeyCode interactKey = KeyCode.E;
        
        public Camera mainCamera;
        private bool _isOverlayShowed;
        public KeyCode InteractKey => interactKey;

        private IToggle _lastToggledObject;
        
        private int _currentMana;
        
        public ItemData selectedItem;
        
        public int CurrentMana
        {
            get => _currentMana;
            set
            {
                _currentMana = value;
                manaBar.DOFillAmount((float)_currentMana/manaCapacity, 0.3f);
            }
        }
        
        private bool _isPlayerEnabled = true;
        public Animator staffAnimator;
        public Animator handAnimator;
        public bool IsPlayerEnabled
        {
            get => _isPlayerEnabled;
            set
            {
                _isPlayerEnabled = value;
                Cursor.lockState = _isPlayerEnabled ? CursorLockMode.Locked : CursorLockMode.Confined;
                cursor.SetActive(_isPlayerEnabled);
                background.SetActive(!_isPlayerEnabled);
                if (!_isPlayerEnabled)
                {
                    TooltipController.Instance.IsTooltipShowed = false;
                    State = PlayerState.Standing;
                }
            }
        }

        private bool _isMiniGamePlayed;
        public bool IsMiniGamePlayed
        {
            get => _isMiniGamePlayed;
            set
            {
                _isMiniGamePlayed = value;
                Cursor.lockState = _isMiniGamePlayed ? CursorLockMode.Locked : CursorLockMode.Confined;
                cursor.SetActive(!_isMiniGamePlayed);
                if (!_isMiniGamePlayed)
                {
                    TooltipController.Instance.IsTooltipShowed = false;
                    State = PlayerState.Standing;
                }
            }
        }


        public bool IsOverlayShowed
        {
            get => _isOverlayShowed;
            set
            {
                if (_isOverlayShowed == value)
                    return;
                _isOverlayShowed = value;
                cursor.SetActive(!_isOverlayShowed);
                Cursor.lockState = _isOverlayShowed ? CursorLockMode.Confined : CursorLockMode.Locked;
                uiCursor.IsActive = _isOverlayShowed;
                if (_isOverlayShowed)
                {
                    TooltipController.Instance.IsTooltipShowed = false;
                    State = PlayerState.Standing;
                }
            }
        }
        public enum PlayerState
        {
            Standing,
            Walking,
            Running,
            Jumping
        }
        
        private PlayerState _state = PlayerState.Standing;

        public PlayerState State
        {
            get => _state;
            set
            {
                if (value == _state)
                    return;
                _state = value;
                switch (_state)
                {
                    case PlayerState.Standing:
                        staffAnimator.SetFloat("Speed", 0);
                        handAnimator.SetFloat("Speed", 0);
                        break;
                    case PlayerState.Walking:
                        staffAnimator.SetFloat("Speed", 1);
                        handAnimator.SetFloat("Speed", 1);
                        break;
                    case PlayerState.Running:
                        staffAnimator.SetFloat("Speed", 2);
                        handAnimator.SetFloat("Speed", 2);
                        break;
                }
            }
        }

        private void Awake()
        {
            if (!instance)
            {
                instance = this;
                mainCamera = Camera.main;
                CurrentMana = 50;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            if (IsPlayerEnabled && !IsOverlayShowed && !IsMiniGamePlayed)
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit, 3f) && hit.transform.GetComponent<IToggle>() != null)
                {
                    IToggle toggleObject = hit.transform.GetComponent<IToggle>();
                    if (toggleObject != null)
                    {
                        toggleObject.IsFocused = true;
                        _lastToggledObject = toggleObject;
                        if (Input.GetKeyDown(interactKey))
                        {
                            toggleObject.Toggle();
                            staffAnimator.SetTrigger("OnInteract");
                        }
                    }
                }
                else
                {
                    if (_lastToggledObject != null)
                    {
                        _lastToggledObject.IsFocused = false;
                        _lastToggledObject = null;
                    }
                }
            }
        }
    }
}