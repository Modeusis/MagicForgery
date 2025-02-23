using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;

namespace UI
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager instance;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioSource musicAudioSource;
        [SerializeField] private AudioMixer mainMixer;
        [SerializeField] private AudioClip starterBgMusic;
        [SerializeField] private List<AudioClip> stepsSfx;
        [SerializeField] private float fadeDuration = .5f;
        public AudioMixerGroup musicGroup;

        public AudioClip currentStepSfx;
        
        public enum StepSfx
        {
            Wood,
            Grass,
            Sand
        }
        
        private StepSfx _sfx;

        public StepSfx Sfx
        {
            get => _sfx;
            set
            {
                if (_sfx == value) return;
                _sfx = value;
                
            }
        }
        
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }

        private void OnEnable()
        {
            musicAudioSource.clip = starterBgMusic;
            musicAudioSource.Play();
        }

        public void PlayBgMusic(AudioClip clip)
        {
            if (musicAudioSource.clip == clip)
                return;

            StartCoroutine(CrossFadeMusic(clip));
        }

        private IEnumerator CrossFadeMusic(AudioClip newClip)
        {
            float startVolume = musicAudioSource.volume;
            for (float t = 0; t < fadeDuration; t += Time.deltaTime)
            {
                musicAudioSource.volume = Mathf.Lerp(startVolume, 0, t / fadeDuration);
                yield return null;
            }
            musicAudioSource.volume = 0;
            musicAudioSource.Stop();
            
            musicAudioSource.clip = newClip;
            musicAudioSource.Play();
            
            for (float t = 0; t < fadeDuration; t += Time.deltaTime)
            {
                musicAudioSource.volume = Mathf.Lerp(0, startVolume, t / fadeDuration);
                yield return null;
            }
            musicAudioSource.volume = startVolume;
        }

        public void PlaySfx(AudioClip clip)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}