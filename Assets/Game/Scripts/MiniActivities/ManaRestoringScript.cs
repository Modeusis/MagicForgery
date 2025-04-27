using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Game.Scripts.Interface;
using Game.Scripts.Tutorial;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Game.Scripts.MiniActivities
{
    public class ManaRestoringScript : MonoBehaviour, IToggle
    {
        //Написать отображение маны когда правильно слово написано
        
        [Inject] private TutorialController _tutorialController;

        [Header("Tutorial")]
        [SerializeField] private int stepId = 2;
        
        [Header("Mini game settings")]
        [SerializeField] private GameObject player;
        
        [SerializeField] private Transform toggleTransform;
        [SerializeField] private WordsData wordsDataBase;
        
        [SerializeField] private TMP_InputField inputWordField;
        [SerializeField] private CanvasGroup manaRestoringCanvasGroup;

        [SerializeField] private float timeGenerationDelay = 4f;
        [SerializeField] private int manaRestoreValue = 5;
        
        [SerializeField] private TMP_Text manaRestoreText;
        
        [field: SerializeField] private List<WordBlock> WordBlocks { get; set; }
        
        private bool _isFocused;
        private bool _isToggled;
        private Vector3 _playerPosition;

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
                    _playerPosition = player.transform.position;
                    _wordsCapacity = wordsDataBase.Words.Count;
                    _textBlocksCapacity = WordBlocks.Count;
                    IsFocused = false;
                }
                
                StopCoroutine(WordGenerator());
                
                manaRestoringCanvasGroup.alpha = value ? 1f : 0f;
                inputWordField.DeactivateInputField();
                
                if (inputWordField && value)
                {
                    inputWordField.text = "";
                    inputWordField.Select();
                    if (_wordsCapacity != 0 && _textBlocksCapacity != 0)
                    {
                        StartCoroutine(WordGenerator());
                    }
                }                
                
                player.transform.position = _isToggled ? toggleTransform.position : _playerPosition;
                player.transform.rotation = _isToggled ? toggleTransform.rotation : Quaternion.identity;
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
                    _tutorialController.CompleteStep(stepId);
                }
                if (Input.GetKeyDown(KeyCode.Space))
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
            
        }

        void RestoreMana(int manaToRestore)
        {
            Player.Player.instance.CurrentMana += manaToRestore;

            if (manaRestoreText)
            {
                manaRestoreText.DOKill();

                manaRestoreText.DOFade(1f, 0.2f).OnComplete(() =>
                {
                    manaRestoreText.DOFade(0f, 0.4f);
                });
            }
        }

        public void InputTextChanged()
        {
            if (string.IsNullOrEmpty(inputWordField.text))
                return;
            var showedWordBlocks = WordBlocks.Where(block => block.IsShowed && !string.IsNullOrEmpty(block.TextBlockValue));

            foreach (var block in showedWordBlocks)
            {
                if (string.Equals(block.TextBlockValue, inputWordField.text, StringComparison.CurrentCultureIgnoreCase))
                {
                    block.IsShowed = false;
                    inputWordField.text = String.Empty;
                    RestoreMana(manaRestoreValue);
                }
            } 
        }

        IEnumerator WordGenerator()
        {
            while (IsToggled)
            {
                yield return new WaitForSeconds(timeGenerationDelay);
                GenerateWords();
            }
        }
    }
}