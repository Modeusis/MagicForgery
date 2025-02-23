using System;
using UI;
using UnityEngine;

namespace Environment
{
    public class BgMusicTrigger : MonoBehaviour
    {
        [SerializeField] private AudioClip currentBgMusic;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("Entered BgMusicTrigger");
                SoundManager.instance.PlayBgMusic(currentBgMusic);
            }
        }
    }
}