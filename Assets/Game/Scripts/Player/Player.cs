using System;
using UnityEngine;

namespace Player
{
    public class Player : MonoBehaviour
    {
        public static Player instance;
        
        private bool _isPlayerEnabled = true;
        public Animator staffAnimator;
        public bool IsPlayerEnabled
        {
            get => _isPlayerEnabled;
            set
            {
                _isPlayerEnabled = value;
                Cursor.lockState = _isPlayerEnabled ? CursorLockMode.Locked : CursorLockMode.Confined;
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
                        break;
                    case PlayerState.Walking:
                        staffAnimator.SetFloat("Speed", 1);
                        break;
                    case PlayerState.Running:
                        staffAnimator.SetFloat("Speed", 2);
                        break;
                }
                Debug.Log(_state);
            }
        }

        private void Awake()
        {
            instance = this;
        }
        
    }
}