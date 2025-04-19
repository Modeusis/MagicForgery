using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.Scripts.MainMenu
{
    public class ConfirmationPopUp : MonoBehaviour
    {
        [SerializeField] private DefinedActionsSetup definedActionsSetup;
        
        [SerializeField] private float scaleOnCall;
        [SerializeField] private float scaleDuration;
        
        [SerializeField] private TMP_Text popUpText;
        
        [SerializeField] private Button dismissActionButton;
        [SerializeField] private Button confirmActionButton;
        
        private string _actionDescription = "";
        
        private Action _currentAction;
        
        public UnityEvent OnPopUpConfirmed;
        
        private void OnEnable()
        {
            dismissActionButton.onClick.AddListener(Dismiss);
            confirmActionButton.onClick.AddListener(Confirm);
        }

        private void OnDisable()
        {
            dismissActionButton.onClick.RemoveAllListeners();
            confirmActionButton.onClick.RemoveAllListeners();
            
            _currentAction = null;
            
            _actionDescription = "";
            popUpText.text = _actionDescription;
            
            OnPopUpConfirmed.RemoveAllListeners();
        }

        public void ShowConfirmationPopUp(MenuAction menuAction)
        {
            _currentAction = GetAction(menuAction);
            
            gameObject.SetActive(true);
        }
        
        public void Dismiss()
        {
            gameObject.SetActive(false);
        }

        public void Confirm()
        {
            _currentAction?.Invoke();
            
            if (_currentAction != null)
            {
                OnPopUpConfirmed?.Invoke();
            }
        }

        public Action GetAction(MenuAction action)
        {
            if (definedActionsSetup == null)
            {
                Debug.LogWarning("Defined action setup is null");
                
                return null;
            }
            
            var definedAction = definedActionsSetup.Actions.Find(defAction => defAction.Type == action);
            
            _actionDescription = definedAction != null ? definedAction.PopUpDescription : "No action provided";
            
            popUpText.text = _actionDescription;
            
            switch (action)
            {
                case MenuAction.Quit:
                {
                    return QuitAction;
                }
                case MenuAction.QuitToMenu:
                {
                    return QuitToMenuAction;
                }
            }
            
            return null;
        }

        private void QuitAction()
        {
            Application.Quit();
            
            Debug.Log("Quit action");
        }

        private void QuitToMenuAction()
        {
            GameLoader.Instance.ToMainMenu();
            
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}