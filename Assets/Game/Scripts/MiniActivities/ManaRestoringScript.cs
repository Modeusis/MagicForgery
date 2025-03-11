using System;
using System.Collections;
using Game.Scripts.Interface;
using UnityEngine;

namespace Game.Scripts.MiniActivities
{
    public class ManaRestoringScript : MonoBehaviour, IToggle
    {
        [SerializeField] private GameObject player;
        
        private Transform _toggleTransform;
        
        private bool _isFocused;
        private bool _isToggled;

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
                if (_isToggled)
                {
                    Player.Player.instance.mainCamera.transform.SetParent(gameObject.transform);
                    IsFocused = false;
                }
                else
                {
                    Player.Player.instance.mainCamera.transform.SetParent(player.transform);
                }
                Player.Player.instance.IsMiniGamePlayed = _isToggled;
                Player.Player.instance.mainCamera.transform.localPosition = value ? new Vector3(0, 1, 0) : new Vector3(0, 1.3f, 0);
                
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