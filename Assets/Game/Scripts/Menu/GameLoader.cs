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
        private const string MainMenuScene = "MainMenu";
        
        private void OnEnable()
        {
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(loadingScreen.gameObject);
        }
        
        public void StartGame()
        {
            loadingScreen.ShowLoadingScreen(SceneToLoad);
        }

        public void ToMainMenu()
        {
            loadingScreen.ShowLoadingScreen(MainMenuScene);
        }
        
        public void QuitGame()
        {
            Application.Quit();
        }
    }
}