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
        
        [Header("Materials")]
        [SerializeField] private Material magicFluidMaterial;
        
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
        private bool _isWorkstationPowered;

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
        
        public bool IsWorkstationPowered
        {
            get => _isWorkstationPowered;
            set
            {
                if (_isWorkstationPowered == value) return;
                _isWorkstationPowered = value;
                AnimateMagicConverter();
            }
        }
        
        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
                magicFluidMaterial.SetFloat("_FlowPower", 0f);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                ToggleWorkstationPower();
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                OpenSwordHandler();
            }
        }

        void ToggleWorkstationPower()
        {
            IsWorkstationPowered = !IsWorkstationPowered;
            
        }

        void AnimateMagicConverter()
        {
            magicConverterHead.transform.DOKill();
            magicCrystal.transform.DOKill();
            magicFluidMaterial.DOKill();
            
            if (IsWorkstationPowered)
            {
                magicConverterHead.transform.DOLocalRotate(new Vector3(0, 360f, 0), 3f, RotateMode.FastBeyond360);
                magicConverterHead.transform.DOLocalMove(new Vector3(0, 1f, 0), 3f);
                
                magicCrystal.transform.DOLocalMove(new Vector3(0, 0.7f, 0), 3f);
                magicCrystal.transform.DOLocalRotate(new Vector3(0, 360f, 0), 3f, RotateMode.FastBeyond360);


                magicFluidMaterial.DOFloat(.25f, "_FlowPower", 2f);
            }
            else
            {
                
                magicConverterHead.transform.DOLocalRotate(new Vector3(0, -360f, 0), 3f, RotateMode.FastBeyond360);
                magicConverterHead.transform.DOLocalMove(new Vector3(0, 0.6f, 0), 3f);
                
                magicCrystal.transform.DOLocalRotate(new Vector3(90f, -360f, 0), 3f, RotateMode.FastBeyond360);
                magicCrystal.transform.DOLocalMove(new Vector3(0, 0.5f, 0), 3f);
                
                magicFluidMaterial.DOFloat(.0f, "_FlowPower", 2f);
            }
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
