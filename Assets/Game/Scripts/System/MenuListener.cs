using Game.Scripts.MainMenu;
using UnityEngine;

namespace Game.Scripts.System
{
    public class MenuListener : MonoBehaviour
    {
        [SerializeField] private SettingsMenu menu;
        
        private Player.Player _player;
        
        private void Update()
        {
            if (Player.Player.instance.IsMiniGamePlayed || Player.Player.instance.IsFinalScreenShown)
            {
                return;
            }
            
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                menu.ToggleMenu();
            }
        }
    }
}