using Game.Scripts.MainMenu;
using UnityEngine;

namespace Game.Scripts.System
{
    public class MenuListener : MonoBehaviour
    {
        [SerializeField] private SettingsMenu menu;
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                menu.ToggleMenu();
            }
        }
    }
}