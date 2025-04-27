using Sounds;
using UnityEngine;
using Zenject;

namespace Player
{
    public class PlayerSoundScript : MonoBehaviour
    {
        [Inject] private SoundService _soundService; 
        
        [SerializeField] private float stepVolume = 0.6f;
        [SerializeField] private float interactVolume = 0.2f;
        
        
        private SoundType _currentStepSound = SoundType.PlayerStepOutside;
        
        public SoundType CurrentStepSound
        {
            get => _currentStepSound;
            set
            {
                if (_currentStepSound == value)
                    return;

                if (value != SoundType.PlayerStepOutside && value != SoundType.PlayerStepInside)
                    return;
                
                _currentStepSound = value;
            }
        }
        
        public void OnStep()
        {
            _soundService.Play2DSfx(CurrentStepSound, stepVolume);
        }

        public void OnInteract()
        {
            // _soundService.Play2DSfx(SoundType.InteractSound, interactVolume);
        }
    }
}