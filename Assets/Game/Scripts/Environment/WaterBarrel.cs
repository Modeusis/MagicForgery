using Game.Scripts.Interface;
using UI;
using UnityEngine;

namespace Environment
{
    public class WaterBarrel : MonoBehaviour, IToggle
    {
        [SerializeField] private int oneBucketAmount;
        [SerializeField] private ParticleSystem fillEffect;
        
        private bool _isFocused;
        public bool IsFocused
        {
            get => _isFocused;
            set
            {
                if (_isFocused == value)
                    return;
                _isFocused = value;
            }
        }
        private bool _isToggled;
        public bool IsToggled
        {
            get => _isToggled;
            set
            {
                if (_isToggled == value)
                    return;
                var playerItem = Player.Player.instance.selectedItem;
                
                if (playerItem)
                {
                    if (playerItem.itemName == "Filled bucket")
                    {
                        
                    }
                    else
                    {
                        TooltipController.Instance.ShowMechanicsDescription("Select filled bucket");
                    }
                }
                else
                {
                    TooltipController.Instance.ShowMechanicsDescription("Select filled bucket");
                }
            }
        }
        public void Toggle()
        {
            IsToggled = !IsToggled;
        }
    }
}