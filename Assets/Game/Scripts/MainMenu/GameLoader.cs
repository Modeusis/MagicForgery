using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Scripts.MainMenu
{
    public class GameLoader : MonoBehaviour
    {
        [SerializeField] private LoadingScreen loadingScreen;

        private const string SceneToLoad = "Game";

        private Scene _sceneToLoad;
        private void OnEnable()
        {
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(loadingScreen.gameObject);
        }
        
        public void StartGame()
        {
            loadingScreen.ShowLoadingScreen(SceneToLoad);
            
            _sceneToLoad = SceneManager.GetSceneByName(SceneToLoad);
        }
        
        public void QuitGame()
        {
            Application.Quit();
        }
    }
}