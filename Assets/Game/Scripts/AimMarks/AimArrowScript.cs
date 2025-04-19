using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Game.Scripts.AimMarks
{
    public class AimArrowScript : MonoBehaviour
    {
        [SerializeField] private AimMarksConfig aimMarksConfig;
        
        [SerializeField] private TMP_Text stepText;

        private List<string> messages;
        
        private Transform _currentTargetTransform;
        
        private Coroutine _messageShowingCoroutine;
        
        public void SwitchAim(MarkType aimType)
        {
            var aimMark = aimMarksConfig.AimMarks.Find(mark => mark.Aim == aimType);
            
            _currentTargetTransform = aimMark.AimTransform;

            messages = aimMark.MessagesForStep;
            
            ShowMessage(messages[0], aimMark.TimeToShowMessage);
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