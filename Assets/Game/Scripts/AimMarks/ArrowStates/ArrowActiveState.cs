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
        private readonly TargetMarksConfig _targetsConfig;
        
        private Transform _arrowTransform;
        
        private TMP_Text _textField;
        
        private Coroutine _messageShowingCoroutine;

        private TargetMark _target;
        
        public ArrowActiveState(StateType stateType, TargetMarksConfig targets, Transform targetArrow, TMP_Text textField)
        {
            StateType = stateType;
            
            _arrowTransform = targetArrow;
            
            _textField = textField;
            
            _targetsConfig = targets;
            
            EventBus.Instance.Subscribe<MarkType>(TargetChangeHandler);
        }
        
        public override void Enter()
        {
            
        }

        public override void Update()
        {
            
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