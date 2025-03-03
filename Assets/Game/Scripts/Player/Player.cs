using System;
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
        [SerializeField] private GameObject background;
        
        [Header("Mana")]
        [SerializeField] private Image manaBar;
        [SerializeField] private int manaCapacity = 100;
        
        [Header("Keys")]
        [SerializeField] private KeyCode interactKey = KeyCode.E;

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
                manaBar.fillAmount = _currentMana / manaCapacity;
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
                CurrentMana = 20;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            if (IsPlayerEnabled)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

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