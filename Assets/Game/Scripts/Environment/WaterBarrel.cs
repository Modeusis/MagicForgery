using Game.Scripts.Interface;
using Sounds;
using UI;
using UnityEngine;
using Zenject;

namespace Environment
{
    public class WaterBarrel : MonoBehaviour, IToggle
    {
        [Inject] private SoundService _soundService;
        
        [SerializeField] private ParticleSystem fillEffect;
        [SerializeField] private ItemData emptyBucket; 
        
        [SerializeField] private int waterAddValue;
        
        private bool _isFocused;
        public bool IsFocused
        {
            get => _isFocused;
            set
            {
                if (_isFocused == value)
                    return;
                _isFocused = value;
                gameObject.layer = _isFocused ? LayerMask.NameToLayer("Interactable") : LayerMask.NameToLayer("Default");
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
                        if (MagicEngineController.Instance.WaterAmount >= MagicEngineController.Instance.GetMaxWater())
                        {
                            TooltipController.Instance.ShowMechanicsDescription("Water already at max");
                            
                            return;
                        }
                            
                        
                        MagicEngineController.Instance.AddWater(waterAddValue);
                        
                        fillEffect?.Play();
                        
                        _soundService.Play3DSfx(SoundType.EngineWaterRestore, transform, 5f, 0.6f);
                        
                        EmptyBucket();
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
        void EmptyBucket()
        {
            Inventory.instance.RemoveItem();

            Inventory.instance.AddItem(emptyBucket);
        }
    }
}