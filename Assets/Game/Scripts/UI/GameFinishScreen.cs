using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Game.Scripts.MainMenu;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class GameFinishScreen : MonoBehaviour
    {
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private Button buttonToMainMenu;
        [SerializeField] private Button buttonQuit;
        
        [SerializeField] private List<string> finalMessagesVariants;
        
        private CanvasGroup _canvasGroup;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }
        private void OnEnable()
        {
            buttonQuit?.onClick.AddListener(Quit);
            buttonToMainMenu?.onClick.AddListener(ToMainMenu);
        }

        private void OnDisable()
        {
            buttonQuit?.onClick.RemoveListener(Quit);
            buttonToMainMenu?.onClick.RemoveListener(ToMainMenu);
        }

        public void ShowFinalScreen()
        {
            gameObject.SetActive(true);
            
            Player.Player.instance.IsFinalScreenShown = true;
            
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            
            titleText.SetText(finalMessagesVariants[Random.Range(0, finalMessagesVariants.Count)]);
            
            _canvasGroup.interactable = true;
            
            StartCoroutine(CanvasGroupFade(0, 1));
        }
        
        private void ToMainMenu() {
            DOTween.KillAll();
            
            GameLoader.Instance.ToMainMenu();
        }
        private void Quit() => GameLoader.Instance.QuitGame();
        
        private IEnumerator CanvasGroupFade(float start, float end, float fadeDuration = 0.5f)
        {
            float timer = 0f;
            
            while (timer < fadeDuration)
            {
                var t = timer / fadeDuration;
                _canvasGroup.alpha = Mathf.Lerp(start, end, t);
                timer += Time.deltaTime;
                yield return null;
            }
            
            _canvasGroup.alpha = end;
        }
    }
}