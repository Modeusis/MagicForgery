using System;
using UnityEngine;

namespace UI
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager instance;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioSource musicAudioSource;
        private void Awake()
        {
            instance = this;
        }

        public void PlayBgMusic(AudioClip clip)
        {
            musicAudioSource.clip = clip;
        }

        public void PlaySfx(AudioClip clip)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}