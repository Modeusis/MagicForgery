using Sounds;
using UnityEngine;

namespace Game.Scripts.AI
{
    public class StepSoundScript : MonoBehaviour
    {
        [SerializeField, Range(0, 1)] private float volume = 0.5f;
        [SerializeField, Range(0, 1)] private float soundRadius = 10f;
        
        public void OnJumpHandle()
        {
            SoundService.Instance.Play3DSfx(SoundType.CustomerStep, transform, soundRadius, volume);
        }
    }
}
