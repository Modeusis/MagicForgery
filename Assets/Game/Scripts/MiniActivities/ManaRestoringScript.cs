using System;
using System.Collections;
using System.Collections.Generic;
using Game.Scripts.Interface;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Scripts.MiniActivities
{
    public class ManaRestoringScript : MonoBehaviour, IToggle
    {
        [SerializeField] private GameObject player;
        
        [SerializeField] private Transform toggleTransform;
        [SerializeField] private WordsData wordsDataBase;
        
        [field: SerializeField] private List<WordBlock> WordBlocks { get; set; }
        
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
                
                player.transform.position = _isToggled ? toggleTransform.position : _playerTransform.position;
                player.transform.rotation = _isToggled ? toggleTransform.rotation : _playerTransform.rotation;
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
                if (Input.GetKeyDown(Player.Player.instance.BreakKey))
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

        IEnumerator WordGenerator()
        {
            yield return new WaitForSeconds(2);
            GenerateWords();
        }
            
    }
}