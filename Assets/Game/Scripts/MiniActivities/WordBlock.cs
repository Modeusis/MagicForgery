using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Scripts.MiniActivities
{
    [RequireComponent(typeof(TMP_Text))]
    public class WordBlock : MonoBehaviour
    {
        [field:SerializeField] private bool isShowed;
        [field:SerializeField] private string textBlockValue;
        
        private TMP_Text _textBlock;
        private Transform _rectTransform;
        private float _randomX;
        public bool IsShowed
        {
            get => isShowed;
            set
            {
                if (isShowed == value)
                    return;
                isShowed = value;
                
                _textBlock.DOKill();
                StopAllCoroutines();
                
                if (isShowed)
                {
                    _randomX = UnityEngine.Random.Range(-Screen.height / 2, Screen.height / 2);
                    _textBlock.DOFade(1, 0.5f);
                    _textBlock.text = TextBlockValue;
                    StartCoroutine(TextFallingCoroutine(() =>
                    {
                        TextBlockValue = null;
                    }));
                }
                else
                {
                    _textBlock.DOFade(0, 0.5f).OnComplete(() =>
                    {
                        _textBlock.text = TextBlockValue;
                    });
                }
            }
        }

        public string TextBlockValue
        {
            get => textBlockValue;
            set
            {
                if (textBlockValue == value)
                    return;
                if (value != null)
                {
                    textBlockValue = value;
                    IsShowed = true;
                }
                else
                {
                    IsShowed = false;
                    textBlockValue = null;
                }
                
            }
        }

        IEnumerator TextFallingCoroutine(Action onComplete, float duration = 5f)
        {
            var startPos = new Vector2(_randomX, Screen.height / 2);
            var finalPos = new Vector2(startPos.x, -(Screen.height - 20) / 2);
            float timer = 0f;
            
            while (timer < duration)
            {
                var t = timer / duration;
                _textBlock.rectTransform.anchoredPosition = Vector2.Lerp(startPos, finalPos, t);
                timer += Time.deltaTime;
                yield return null;
            }
            
            onComplete?.Invoke();
        }

        private void Awake()
        {
            _textBlock = GetComponent<TMP_Text>();
        }
    }
}