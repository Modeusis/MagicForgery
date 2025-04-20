using Sounds;
using UnityEngine;

namespace Game.Scripts.AI
{
    public class StepSoundScript : MonoBehaviour
    {
        private SoundService _soundService;
        
        [SerializeField, Range(0, 1)] private float volume = 0.5f;
        [SerializeField, Range(0, 1)] private float soundRadius = 10f;

        public void Initialize(SoundService soundService)
        {
            _soundService = soundService;
        }
        
        public void OnJumpHandle()
        {
            _soundService.Play3DSfx(SoundType.CustomerStep, transform, soundRadius, volume);
        }
    }
}
