using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Scripts.MainMenu
{
    public class GameLoader : MonoBehaviour
    {
        public static GameLoader Instance { get; private set; }

        private const int GameSceneId = 0;
        private const int MainMenuSceneId = 1;

        public void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                
                DontDestroyOnLoad(Instance);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        public void StartGame()
        {
            DOTween.KillAll();
            
            LoadingScreen.Instance.ShowLoadingScreen(GameSceneId);
        }

        public void ToMainMenu()
        {
            DOTween.KillAll();
            
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            
            LoadingScreen.Instance.ShowLoadingScreen(MainMenuSceneId);
        }
        
        public void QuitGame()
        {
            Application.Quit();
        }
    }
}