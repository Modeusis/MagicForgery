using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Game.Scripts.TargetMarks
{
    public class TargetArrowScript : MonoBehaviour
    {
        [SerializeField] private TargetMarksConfig TargetMarksConfig;
        
        [SerializeField] private TMP_Text stepText;

        private List<string> _messages;
        
        private Transform _currentTargetTransform;
        
        private Coroutine _messageShowingCoroutine;
        
        public void SwitchTarget(MarkType targetType)
        {
            var targetMark = TargetMarksConfig.TargetMarks.Find(mark => mark.Target == targetType);
            
            _currentTargetTransform = targetMark.TargetTransform;

            _messages = targetMark.MessagesForStep;
            
            ShowMessage(_messages[0], targetMark.TimeToShowMessage);
        }

        private void ShowMessage(string message, float duration = 1f)
        {
            if (_messageShowingCoroutine != null)
            {
                StopCoroutine(_messageShowingCoroutine);
                
                _messageShowingCoroutine = null;
            }
            
            _messageShowingCoroutine = StartCoroutine(MessageWriteCoroutine(message, duration));
        }

        private IEnumerator MessageWriteCoroutine(string message, float duration)
        {
            string actualMessage = "";
            
            int letterIndex = 0;
            
            float eachLetterDelay = duration / message.Length;
            float timer = 0f;
            
            YieldInstruction waitLetterDelay = new WaitForSeconds(eachLetterDelay);

            while (timer < duration)
            {
                actualMessage += message[letterIndex];
                stepText.text = actualMessage;
                
                letterIndex++;
                
                timer += eachLetterDelay;
                
                yield return waitLetterDelay;
            }
                
            stepText.text = message;
        }
    }
}