using System;
using System.Collections;
using Game.Scripts.Interface;
using UnityEngine;

namespace Game.Scripts.MiniActivities
{
    public class ManaRestoringScript : MonoBehaviour, IToggle
    {
        [SerializeField] private GameObject player;
        
        [SerializeField] private Transform _toggleTransform;
        
        private bool _isFocused;
        private bool _isToggled;
        private Transform _playerTransform;

        public bool IsFocused
        {
            get => _isFocused;
            set
            {
                if (_isFocused == value)
                    return;
                _isFocused = value;
                gameObject.layer = value ? LayerMask.NameToLayer("Interactable") : LayerMask.NameToLayer("Default");
                
            }
        }

        public bool IsToggled
        {
            get => _isToggled;
            set
            {
                if (_isToggled == value)
                    return;
                _isToggled = value;

                if (value)
                {
                    _playerTransform = player.transform;
                }
                
                player.transform.position = _isToggled ? _toggleTransform.position : _playerTransform.position;
                Player.Player.instance.IsMiniGamePlayed = _isToggled;
            }
        }
        public void Toggle()
        {
            IsToggled = !IsToggled;
        }

        private void Update()
        {
            if (IsToggled)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    Toggle();
                }
            }
        }

        void GenerateWords()
        {
            
        }

        void RestoreMana(int manaToRestore)
        {
            Player.Player.instance.CurrentMana += manaToRestore;
        }
    }
}