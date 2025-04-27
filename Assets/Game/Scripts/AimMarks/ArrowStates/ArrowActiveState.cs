using System;
using System.Collections;
using System.Collections.Generic;
using Game.Scripts.Utilities;
using Game.Scripts.Utilities.FSM;
using Sounds;
using TMPro;
using UnityEngine;

namespace Game.Scripts.TargetMarks.ArrowStates
{
    public class ArrowActiveState : State
    {
        private readonly EventBus _eventBus;
        
        private readonly TargetMarksConfig _targetsConfig;
        
        private readonly SoundService _soundService;
        
        private Arrow _arrow;
        
        private TMP_Text _textField;
        
        private Coroutine _messageShowingCoroutine;

        private TargetMark _target;

        private int _wordsOnOneRow;
        
        private float _minAimMarkDistance;
        
        public ArrowActiveState(StateType stateType, TargetMarksConfig targets, Arrow arrow, TMP_Text textField,
            EventBus eventBus, int wordsMax, float minDistance, SoundService soundService)
        {
            StateType = stateType;
            
            _arrow = arrow;
            
            _soundService = soundService;
            
            _textField = textField;
            
            _targetsConfig = targets;
            
            _eventBus = eventBus;
            _eventBus?.Subscribe<MarkType>(TargetChangeHandler);
            
            _wordsOnOneRow = wordsMax;
            
            _minAimMarkDistance = minDistance;
        }
        
        public override void Enter()
        {
            if (_target == null)
            {
                return;
            }
            
            _arrow.ShowArrow();
            
            ShowMessage(_target.MessagesForStep, _target.MessageSymbolsDelay);
        }

        public override void Update()
        {
            if (_arrow.GetDistanceToAim(_target.TargetPosition) < _minAimMarkDistance)
            {
                _eventBus?.Publish(new TagCloseToAim());
                
                return;
            }
            
            _arrow.LookAtTarget(_target.TargetPosition);
        }

        public override void Exit()
        {
            _textField.text = "";

            if (_messageShowingCoroutine != null)
            {
                _textField.StopCoroutine(_messageShowingCoroutine);
                
                _messageShowingCoroutine = null;
            }
        }
        
        private void ShowMessage(string message, float duration = 1f)
        {
            if (_messageShowingCoroutine != null)
            {
                _textField.StopCoroutine(_messageShowingCoroutine);
                
                _messageShowingCoroutine = null;
                
                _textField.text = message;
            }
            
            _messageShowingCoroutine = _textField.StartCoroutine(MessageWriteCoroutine(message, _wordsOnOneRow, duration));
        }

        private IEnumerator MessageWriteCoroutine(string message, int wordsPerRow, float delayChars = .2f, float delayBeforeCleaning = 2f)
        {
            int letterIndex = 0;
            
            string actualMessage = "";
            
            var wordCounter = 0;
            
            YieldInstruction waitLetterDelay = new WaitForSeconds(delayChars);
            
            while (letterIndex < message.Length)
            {
                actualMessage += message[letterIndex];
                
                _soundService.Play2DSfx(SoundType.CharacterPrinted, 0.5f);
                
                if (message[letterIndex] == ' ')
                {
                    wordCounter++;
                };

                if (wordCounter == wordsPerRow)
                {
                    wordCounter = 0;
                    
                    _textField.text = "";
                    actualMessage = "";
                }
                else
                {
                    _textField.text = actualMessage;
                }
                
                
                letterIndex++;
                
                yield return waitLetterDelay;
            }
                
            _textField.text = actualMessage;
            
            yield return new WaitForSeconds(delayBeforeCleaning);
            
            _textField.text = "";
        }

        public void TargetChangeHandler(MarkType markType)
        {
            if (TryGetTarget(markType, out _target))
            {
                return;
            }
            
            Debug.LogWarning("Aim with such target was not found");
        }
        
        public bool TryGetTarget(MarkType markType, out TargetMark targetMark)
        {
            var findMark = _targetsConfig.TargetMarks.Find(mark => mark.Target == markType);

            if (findMark != null)
            {
                targetMark = findMark;
                
                return true;
            }
            
            targetMark = null;
            
            Debug.Log($"Mark not found");
            
            return false;
        }
    }
}