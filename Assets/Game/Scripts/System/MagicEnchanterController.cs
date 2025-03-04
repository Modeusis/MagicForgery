using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace UI
{
    public class MagicEnchanterController : MonoBehaviour
    {
        public static MagicEnchanterController Instance;
        
        [Header("Animation")]
        [SerializeField] private Animator swordHandlerAnimator;
        [SerializeField] private Animator featherAnimator;
    
        [Header("Sounds")]
        [SerializeField] private AudioClip magicConverterSound;
        [SerializeField] private AudioClip magicCrystalSound;
        [SerializeField] private AudioClip swordHandlerOpenSound;
        [SerializeField] private AudioClip magicSphereOpenSound;
        [SerializeField] private AudioClip featherWritingSound;
        [SerializeField] private AudioClip featherLevitatingSound;

        public enum PotionType
        {
            Red,
            Blue,
            Green,
            Yellow,
            Metal
        }
    }
}
