using System;
using System.Collections;
using System.Collections.Generic;
using Game.Scripts.Utilities;
using Game.Scripts.Utilities.FSM;
using TMPro;
using UnityEngine;

namespace Game.Scripts.TargetMarks.ArrowStates
{
    public class ArrowActiveState : State
    {
        private readonly EventBus _eventBus;
        
        private readonly TargetMarksConfig _targetsConfig;
        
        private Arrow _arrow;
        
        private TMP_Text _textField;
        
        private Coroutine _messageShowingCoroutine;

        private TargetMark _target;
        
        public ArrowActiveState(StateType stateType, TargetMarksConfig targets, Arrow arrow, TMP_Text textField, EventBus eventBus)
        {
            StateType = stateType;
            
            _arrow = arrow;
            
            _textField = textField;
            
            _targetsConfig = targets;
            
            _eventBus = eventBus;
            
            _eventBus?.Subscribe<MarkType>(TargetChangeHandler);
        }
        
        public override void Enter()
        {
            _arrow.ShowArrow();
            
            _arrow.RotateArrowToTarget(_target.TargetPosition);
        }

        public override void Update()
        {
            _arrow.LookAtTarget(_target.TargetPosition);
        }

        public override void Exit()
        {
            
            
            _target = null;
            
            _messageShowingCoroutine = null;
        }
        
        private void ShowMessage(List<string> message, float duration = 1f)
        {
            if (_messageShowingCoroutine != null)
            {
                _textField.StopCoroutine(_messageShowingCoroutine);
                
                _messageShowingCoroutine = null;
            }
            
            _messageShowingCoroutine = _textField.StartCoroutine(MessageWriteCoroutine(message[0], duration));
        }

        private IEnumerator MessageWriteCoroutine(string message, float duration, Action onComplete = null)
        {
            string actualMessage = "";
            
            int letterIndex = 0;
            
            float eachLetterDelay = duration / message.Length;
            float timer = 0f;
            
            YieldInstruction waitLetterDelay = new WaitForSeconds(eachLetterDelay);

            while (timer < duration)
            {
                actualMessage += message[letterIndex];
                _textField.text = actualMessage;
                
                letterIndex++;
                
                timer += eachLetterDelay;
                
                yield return waitLetterDelay;
            }
                
            _textField.text = message;
        }

        public void TargetChangeHandler(MarkType markType)
        {
            if (TryGetTarget(markType, out _target))
            {
                ShowMessage(_target.MessagesForStep, _target.TimeToShowMessage);
                
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
            
            return false;
        }
    }
}