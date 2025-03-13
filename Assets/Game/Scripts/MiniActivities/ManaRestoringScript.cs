using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Interface;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Scripts.MiniActivities
{
    //Пофиксить позицию спавна и работу INPUT
    public class ManaRestoringScript : MonoBehaviour, IToggle
    {
        [SerializeField] private GameObject player;
        
        [SerializeField] private Transform toggleTransform;
        [SerializeField] private WordsData wordsDataBase;
        
        [SerializeField] private TMP_InputField inputWordField;
        [SerializeField] private CanvasGroup manaRestoringCanvasGroup;

        [SerializeField] private float timeGenerationDelay = 4f;
        
        [field: SerializeField] private List<WordBlock> WordBlocks { get; set; }
        
        private bool _isFocused;
        private bool _isToggled;
        private Transform _playerTransform;

        private int _wordsCapacity;
        private int _textBlocksCapacity;

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
                    _wordsCapacity = wordsDataBase.Words.Count;
                    _textBlocksCapacity = WordBlocks.Count;
                }
                
                StopCoroutine(WordGenerator());
                
                manaRestoringCanvasGroup.gameObject.SetActive(value);
                manaRestoringCanvasGroup.alpha = value ? 1f : 0f;

                if (inputWordField && value)
                {
                    inputWordField.text = "";
                    inputWordField.ActivateInputField();
                    if (_wordsCapacity != 0 && _textBlocksCapacity != 0)
                    {
                        StartCoroutine(WordGenerator());
                    }
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
                if (Input.GetKeyDown(KeyCode.Backspace))
                {
                    inputWordField.text = String.Empty;
                }
            }
        }

        void GenerateWords()
        {
            var randomWordNumber = UnityEngine.Random.Range(0, _wordsCapacity);
            
            var notShowedWordBlocks = WordBlocks.Where(block => !block.IsShowed);
            int notShowedCount = notShowedWordBlocks.Count();
            
            if (notShowedCount == 0)
                return;
            
            var randomTextBlockNumber = UnityEngine.Random.Range(0, notShowedCount);
            
            notShowedWordBlocks.ToList()[randomTextBlockNumber].TextBlockValue = wordsDataBase.Words[randomWordNumber];
            
            Debug.Log($"{randomTextBlockNumber} : {randomWordNumber}");
        }

        void RestoreMana(int manaToRestore)
        {
            Player.Player.instance.CurrentMana += manaToRestore;
        }

        public void InputTextChanged()
        {
            if (string.IsNullOrEmpty(inputWordField.text))
                return;
            Debug.Log($"{inputWordField.text}");
        }

        IEnumerator WordGenerator()
        {
            while (IsToggled)
            {
                yield return new WaitForSeconds(timeGenerationDelay);
                GenerateWords();
                Debug.Log("word generator called");
            }
        }
    }
}