using UnityEngine;

namespace Player
{
    public class Player
    {
        private static bool _isPlayerEnabled = true;

        public static bool IsPlayerEnabled
        {
            get => _isPlayerEnabled;
            set
            {
                _isPlayerEnabled = value;
                Cursor.lockState = _isPlayerEnabled ? CursorLockMode.Locked : CursorLockMode.Confined;
            }
        }
        private static PlayerState _state = PlayerState.Standing;

        public static PlayerState State
        {
            get => _state;
            set
            {
                if (value == _state)
                    return;
                _state = value;
            }
        }

        public enum PlayerState
        {
            Standing,
            Walking,
            Running,
            Jumping
        }
    }
}