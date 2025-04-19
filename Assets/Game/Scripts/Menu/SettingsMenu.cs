using System;
using UnityEngine;

namespace Game.Scripts.MainMenu
{
    public class SettingsMenu : MonoBehaviour
    {
        [SerializeField] private ConfirmationPopUp confirmationPopUp;

        private bool _menuCurrentState = false;
        
        public void ToggleMenu()
        {
            _menuCurrentState = !_menuCurrentState;
            
            Player.Player.instance.IsOverlayShowed = _menuCurrentState;
            
            confirmationPopUp.Dismiss();
            
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
            
            confirmationPopUp.OnPopUpConfirmed.AddListener(ToggleMenu);
        }

        public void QuitToMenu()
        {
            confirmationPopUp.ShowConfirmationPopUp(MenuAction.QuitToMenu);
            
            confirmationPopUp.OnPopUpConfirmed.AddListener(ToggleMenu);
        }

        public void OpenSettingsScreen()
        {
            Debug.Log("Settings opened");
        }
    }
}