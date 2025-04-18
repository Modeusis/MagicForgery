using TMPro;
using UnityEngine;

namespace Game.Scripts.AimMarks
{
    public class AimArrowScript : MonoBehaviour
    {
        private string _currentMessage;
        
        [SerializeField] private AimMarksConfig aimMarksConfig;
        
        [SerializeField] private TMP_Text stepText;
            
        private void ShowMessage()
        {
            
        }

        public void SwitchAim(MarkType aimType)
        {
            
        }
    }
}