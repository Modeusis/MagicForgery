using Sounds;
using UnityEngine;
using Zenject;

namespace Environment
{
    public class BgMusicTrigger : MonoBehaviour
    {
        [Inject] private SoundService _soundService;
        
        [SerializeField] private SoundType soundType;
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _soundService.PlayBackgroundMusic(soundType);
            }
        }
    }
}