using System;
using DG.Tweening;
using Environment;
using Game.Scripts.Interface;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public partial class Player : MonoBehaviour
    {
        public static Player instance;
        
        [Header("Light")]
        [SerializeField] private Light staffLight;
        
        [Header("UI")]
        [SerializeField] private GameObject cursor;
        [SerializeField] private UiCursor uiCursor;
        [SerializeField] private GameObject background;
        
        [Header("Mana")]
        [SerializeField] private Image manaBar;
        [SerializeField] private int manaCapacity = 100;
        
        [Header("Keys")]
        [SerializeField] private KeyCode interactKey = KeyCode.E;
        [SerializeField] private KeyCode breakKey = KeyCode.F;
        
        public Camera mainCamera;
        private bool _isOverlayShowed;
        public KeyCode InteractKey => interactKey;
        public KeyCode BreakKey => breakKey;

        private IToggle _lastToggledObject;
        private IDrawable _drawableObject = null;
        
        private int _currentMana;
        
        public bool IsFinalScreenShown { get; set; } = false;
        
        public ItemData selectedItem;
        
        public int CurrentMana
        {
            get => _currentMana;
            set
            {
                if (value > manaCapacity)
                {
                    _currentMana = manaCapacity;
                    manaBar.DOFillAmount((float)_currentMana/manaCapacity, 0.3f);
                    return;
                }
                
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
                CurrentMana = 0;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            if (instance.IsFinalScreenShown)
            {
                return;
            }
            
            if (IsPlayerEnabled && !IsOverlayShowed && !IsMiniGamePlayed)
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

                var isRaycastHit = Physics.Raycast(ray, out RaycastHit hit, 3f);

                if (!isRaycastHit)
                {
                    if (_lastToggledObject != null)
                    {
                        _lastToggledObject.IsFocused = false;
                        _lastToggledObject = null;
                    }
                    return;
                }
                    
                
                if (hit.transform.TryGetComponent(out IToggle toggleObject))
                {
                    if (toggleObject != null)
                    {
                        toggleObject.IsFocused = true;
                        _lastToggledObject = toggleObject;
                        if (Input.GetKeyDown(interactKey))
                        {
                            staffLight.DOKill();
                            
                            toggleObject.Toggle();
                            staffAnimator.SetTrigger("OnInteract");
                            staffLight.enabled = true;
                            staffLight.DOIntensity(2f, 0.2f).OnComplete(() =>
                            {
                                staffLight.DOIntensity(0f, 0.6f).OnComplete(() =>
                                {
                                    staffLight.enabled = false;
                                });
                            });
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

                if (hit.transform.TryGetComponent(out IPressable pressableObject))
                {
                    if (Input.GetKeyDown(interactKey))
                    {
                        staffAnimator.SetTrigger("OnInteract");
                        pressableObject.Press();
                    }
                }
                
                if (Input.GetMouseButtonDown(0))
                {
                    hit.transform.TryGetComponent(out _drawableObject);
                }
                if (Input.GetMouseButtonUp(0))
                {
                    _drawableObject = null;
                }
                if (_drawableObject != null)
                {
                    if (Input.GetMouseButton(0) && _drawableObject.IsDrawing)
                    {
                        _drawableObject.Draw(hit);
                    }
                }
                
            }
        }
    }
}