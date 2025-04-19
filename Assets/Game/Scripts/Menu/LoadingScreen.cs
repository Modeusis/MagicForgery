using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Scripts.MainMenu
{
    [RequireComponent(typeof(CanvasGroup))]
    public class LoadingScreen : MonoBehaviour
    {
        public static LoadingScreen Instance { get; private set; }
        
        private CanvasGroup _canvasGroup;
        
        private AsyncOperation _sceneLoadOperation;
        
        public void Awake()
        {
            if (Instance == null)
            {
                _canvasGroup = GetComponent<CanvasGroup>();
                
                Instance = this;
                
                DontDestroyOnLoad(Instance);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            if (_sceneLoadOperation == null)
            {
                return;
            }
                

            if (_sceneLoadOperation.isDone)
            {
                HideLoadingScreen();
                
                _sceneLoadOperation = null;
            }
        }
        
        public void ShowLoadingScreen(int sceneId)
        {
            Debug.Log($"Loading Scene: {sceneId}");
            
            StartCoroutine(FadeLoadingScreen(_canvasGroup, .5f, () =>
            {
                _sceneLoadOperation = SceneManager.LoadSceneAsync(sceneId);
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