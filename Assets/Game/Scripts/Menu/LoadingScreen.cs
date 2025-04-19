using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Scripts.MainMenu
{
    [RequireComponent(typeof(CanvasGroup))]
    public class LoadingScreen : MonoBehaviour
    {
        private CanvasGroup _canvasGroup;
        
        private AsyncOperation _sceneLoadOperation;
        
        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        private void Update()
        {
            if (_sceneLoadOperation == null)
                return;

            if (_sceneLoadOperation.isDone)
            {
                HideLoadingScreen();
                
                _sceneLoadOperation = null;
            }
        }
        
        public void ShowLoadingScreen(string sceneName)
        {
            StartCoroutine(FadeLoadingScreen(_canvasGroup, .5f, () =>
            {
                _sceneLoadOperation = SceneManager.LoadSceneAsync(sceneName);
            }));
        }

        public void HideLoadingScreen()
        {
            StartCoroutine(FadeLoadingScreen(_canvasGroup, .5f));
        }
        
        private IEnumerator FadeLoadingScreen(CanvasGroup loadScreen, float duration, Action onFinish = null)
        {
            var startAlpha = loadScreen.alpha;
            var loadTarget = startAlpha == 1f ? 0f : 1f;

            loadScreen.interactable = loadTarget == 1f;
            
            float timer = 0f;
            
            while (timer < duration) 
            {
                loadScreen.alpha = Mathf.Lerp(startAlpha, loadTarget, timer / duration);    
                timer += Time.deltaTime; 
                yield return null; 
            }
            
            loadScreen.alpha = loadTarget;
            
            onFinish?.Invoke();
        }
    }
}