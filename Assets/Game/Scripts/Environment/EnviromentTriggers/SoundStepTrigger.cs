using System;
using Game.Scripts.AI;
using Player;
using Sounds;
using UnityEngine;

namespace Environment
{
    public class SoundStepTrigger : MonoBehaviour
    {
        [SerializeField] private PlayerSoundScript playerSound;
        [SerializeField] private SoundType newStepSound;
        private void OnTriggerEnter(Collider other)
        {
            if (!playerSound)
                return;

            playerSound.CurrentStepSound = newStepSound;
        }
    }
}