using System;
using System.Collections.Generic;
using DG.Tweening;
using Environment;
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
        
        private Sword swordToEnchant;
        private Enchantment _swordEnchantment;

        public Sword SwordToEnchant
        {
            get => swordToEnchant;
            set
            {
                if (swordToEnchant == value)
                    return;
                swordToEnchant = value;
            }
        }

        public Enchantment SwordEnchantment
        {
            get => _swordEnchantment;
            set
            {
                if (_swordEnchantment == value)
                    return;
                _swordEnchantment = value;
            }
        }
        
        public enum PotionType
        {
            Empty,
            Red,
            Blue,
            Green,
            Yellow,
            Metal
        }

        void Awake()
        {
            if (!Instance)
            {
                Instance = this;    
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
