using System;
using UnityEngine;

namespace UI
{
    public class MagicEnchanterController : MonoBehaviour
    {
        public static MagicEnchanterController Instance;
        
        [Header("Animation")]
        [SerializeField] private Animator swordHandlerAnimator;
        [SerializeField] private Animator magicConverterAnimator;
        [SerializeField] private Animator featherAnimator;
    
        [Header("Sounds")]
        // [SerializeField] private AudioClip swordSound; добавить звук в сами объекты для поднятия и положить
        [SerializeField] private AudioClip magicConverterSound;
        [SerializeField] private AudioClip magicCrystalSound;
        [SerializeField] private AudioClip swordHandlerOpenSound;
        [SerializeField] private AudioClip magicSphereOpenSound;
        [SerializeField] private AudioClip featherWritingSound;
        [SerializeField] private AudioClip featherLevitatingSound;
        
        [Header("GameObjects")]
        [SerializeField] private GameObject magicConverterHead;
        [SerializeField] private GameObject magicCrystal;
        
        private ItemData _firstItemData;
        private ItemData _secondItemData;
        private ItemData _thirdItemData;
        private ItemData _swordItemData;
        
        private bool _isSwordHandlerOpened;
        private bool _isSwordPlaced;
        private bool _isFirstPotionPlaceIsSet;
        private bool _isSecondPotionPlaceIsSet;
        private bool _isThirdPotionPlaceIsSet;
        private bool _isMagicEnchanterReady;
        private bool _isEnchantingFinished;
        private bool _isWorkstationAvailable;

        public bool IsSwordHandlerOpened
        {
            get => _isSwordHandlerOpened;
            set
            {
                if (_isSwordHandlerOpened == value) return;
                _isSwordHandlerOpened = value;
            }
        }
        
        public bool IsSwordPlaced
        {
            get => _isSwordPlaced;
            set
            {
                if (_isSwordPlaced == value) return;
                _isSwordPlaced = value;
            }
        }
        
        public bool IsFirstPotionPlaceIsSet
        {
            get => _isFirstPotionPlaceIsSet;
            set
            {
                if (_isFirstPotionPlaceIsSet == value) return;
                _isFirstPotionPlaceIsSet = value;
            }
        }
        public bool IsSecondPotionPlaceIsSet
        {
            get => _isSecondPotionPlaceIsSet;
            set
            {
                if (_isSecondPotionPlaceIsSet == value) return;
                _isSecondPotionPlaceIsSet = value;
            }
        }
        public bool IsThirdPotionPlaceIsSet
        {
            get => _isThirdPotionPlaceIsSet;
            set
            {
                if (_isThirdPotionPlaceIsSet == value) return;
                _isThirdPotionPlaceIsSet = value;
            }
        }
        public bool IsMagicEnchanterReady
        {
            get => _isMagicEnchanterReady;
            set
            {
                if (_isMagicEnchanterReady == value) return;
                _isMagicEnchanterReady = value;
            }
        }
        public bool IsEnchantingFinished
        {
            get => _isEnchantingFinished;
            set
            {
                if (_isEnchantingFinished == value) return;
                _isEnchantingFinished = value;
            }
        }
        public bool IsWorkstationAvailable
        {
            get => _isWorkstationAvailable;
            set
            {
                if (_isWorkstationAvailable == value) return;
                _isWorkstationAvailable = value;
            }
        }
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        void ToggleWorkstationPower()
        {
            
        }
        
        void OpenSwordHandler()
        {
            if (IsWorkstationAvailable)
            {
                swordHandlerAnimator.SetBool("IsOpened", !IsSwordHandlerOpened);
                IsSwordHandlerOpened = !IsSwordHandlerOpened;
            }
        }
        
        void PlaceSword()
        {
            
        }

        void ValidateEnchanting()
        {
            
        }
        
        void BeginEnchanting()
        {
            
        }
    }
}
