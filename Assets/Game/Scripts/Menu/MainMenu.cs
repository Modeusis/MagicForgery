using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.MainMenu
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button StartGameButton;
        [SerializeField] private Button QuitButton;
        
        private void OnEnable()
        {
            StartGameButton.onClick.AddListener(StartAction);
            QuitButton.onClick.AddListener(LeaveAction);
        }

        private void OnDisable()
        {
            StartGameButton.onClick.RemoveAllListeners();
            QuitButton.onClick.RemoveAllListeners();
        }
        
        private void StartAction() => GameLoader.Instance.StartGame();
        private void LeaveAction() => GameLoader.Instance.QuitGame();
    }
}