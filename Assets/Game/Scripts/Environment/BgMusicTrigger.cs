using Sounds;
using UnityEngine;

namespace Environment
{
    public class BgMusicTrigger : MonoBehaviour
    {
        [SerializeField] private SoundType soundType;
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                SoundService.Instance.PlayBackgroundMusic(soundType);
            }
        }
    }
}