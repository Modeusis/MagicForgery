using System;
using UnityEngine;

namespace Game.Scripts.MainMenu
{
    public class SettingsMenu : MonoBehaviour
    {
        [SerializeField] private ConfirmationPopUp confirmationPopUp;

        private bool _menuCurrentState = false;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ToggleMenu();
            }
        }
        
        private void ToggleMenu()
        {
            _menuCurrentState = !_menuCurrentState;
            
            gameObject.SetActive(_menuCurrentState);
            
            Time.timeScale = _menuCurrentState ? 0f : 1f;
        }

        public void CloseMenu()
        {
            ToggleMenu();
        }

        public void QuitGame()
        {
            confirmationPopUp.ShowConfirmationPopUp(MenuAction.Quit);
        }

        public void QuitToMenu()
        {
            confirmationPopUp.ShowConfirmationPopUp(MenuAction.QuitToMenu);
        }

        public void OpenSettingsScreen()
        {
            Debug.Log("Settings opened");
        }
    }
}