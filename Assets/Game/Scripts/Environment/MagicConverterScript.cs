using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Game.Scripts.Interface;
using Sounds;
using UI;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

namespace Environment
{
    public class MagicConverterScript : MonoBehaviour, IToggle
    {
        [Inject] private SoundService _soundService;
        
        [Header("Objects")]
        [SerializeField] private GameObject magicConverterHead;
        [SerializeField] private GameObject magicCrystal;
        [SerializeField] private Material magicCrystalMaterial;
        
        [SerializeField] private List<PotionPlaceScript> potionPlaces; 
        [SerializeField] private EnchantmentData enchantmentData;
        [SerializeField] private PlaceHolderScript placeHolder;
        
        private Dictionary<MagicEnchanterController.PotionType, int> _potions = new Dictionary<MagicEnchanterController.PotionType, int>();
        
        [Header("Keycodes")]
        [SerializeField] private KeyCode interactKey = KeyCode.E;
        
        [Header("Effects")]
        [SerializeField] private Color crystalBaseColor;
        
        private bool _isToggled;
        private bool _isFocused;
        public bool IsBlocked { get; set; }
        
        private Enchantment _currentEnchantment;

        public Enchantment CurrentEnchantment
        {
            get => _currentEnchantment;
            set
            {
                if (_currentEnchantment == value)
                    return;
                _currentEnchantment = value;
                MagicEnchanterController.Instance.SwordEnchantment = _currentEnchantment;    
                
                if (!CurrentEnchantment)
                    CrystalColor = crystalBaseColor;
                else
                {
                    CrystalColor = CurrentEnchantment.enchantmentColor;
                }
            }
        }
        
        private Color _crystalColor;

        private Color CrystalColor
        {
            get => _crystalColor;
            set
            {
                if (_crystalColor == value)
                    return;
                _crystalColor = value;
                magicCrystalMaterial.DOKill();
                
                if (magicCrystalMaterial)
                {
                    magicCrystalMaterial.DOColor(_crystalColor, "_CrystalColor", 1f);
                }
            }
        }
        private void Awake()
        {
            magicCrystalMaterial.SetColor("_CrystalColor", crystalBaseColor);
        }
        
        void AnimateMagicConverter()
        {
            magicConverterHead.transform.DOKill();
            magicCrystal.transform.DOKill();
            DOTween.Kill("headRotateSeq");
            DOTween.Kill("headMoveSeq");
            
            if (IsToggled)
            {
                magicConverterHead.transform.DOLocalRotate(new Vector3(0, 360f, 0), 3f, RotateMode.FastBeyond360);
                magicConverterHead.transform.DOLocalMove(new Vector3(0, 1f, 0), 3f);
                
                magicCrystal.transform.DOLocalMove(new Vector3(0, 0.7f, 0), 3f);
                magicCrystal.transform.DOLocalRotate(new Vector3(0, 360f, 0), 3f, RotateMode.FastBeyond360).OnComplete(
                    () =>
                    {
                        magicCrystal.transform.DOLocalRotate(new Vector3(0, 360f, 0), 5f, RotateMode.FastBeyond360)
                            .SetLoops(-1, LoopType.Incremental)
                            .SetEase(Ease.Linear);
                        
                        var moveSeq = DOTween.Sequence();
                        var rotateSeq = DOTween.Sequence();
                        
                        rotateSeq.Append(magicConverterHead.transform.DOLocalRotate(new Vector3(0, 60f, 0), 1f, RotateMode.FastBeyond360))
                            .SetEase(Ease.InSine);
                        rotateSeq.Append(magicConverterHead.transform.DOLocalRotate(new Vector3(0, -60f, 0), 1.5f, RotateMode.FastBeyond360))
                            .SetEase(Ease.OutSine);
                        rotateSeq.Append(magicConverterHead.transform.DOLocalRotate(Vector3.zero, 1.5f, RotateMode.FastBeyond360))
                            .SetEase(Ease.OutSine);
                        
                        moveSeq.Append(magicConverterHead.transform.DOLocalMove(new Vector3(0, 1.1f, 0), 8f))
                            .SetEase(Ease.InSine);
                        moveSeq.Append(magicConverterHead.transform.DOLocalMove(new Vector3(0, 1f, 0), 6f))
                            .SetEase(Ease.OutSine);
                        
                        rotateSeq.SetLoops(-1, LoopType.Restart);
                        moveSeq.SetLoops(-1, LoopType.Restart);

                        rotateSeq.SetId("headRotateSeq");
                        moveSeq.SetId("headMoveSeq");

                    });
            }
            else
            {
                
                magicConverterHead.transform.DOLocalRotate(new Vector3(0, -360f, 0), 3f, RotateMode.FastBeyond360);
                magicConverterHead.transform.DOLocalMove(new Vector3(0, 0.6f, 0), 3f);
                
                magicCrystal.transform.DOLocalRotate(new Vector3(90f, -360f, 0), 3f, RotateMode.FastBeyond360);
                magicCrystal.transform.DOLocalMove(new Vector3(0, 0.5f, 0), 3f);
            }
        }

        public bool IsFocused
        {
            get => _isFocused;
            set
            {
                if (_isFocused == value)
                    return;
                _isFocused = value;
                TooltipController.Instance.TooltipMessage = $"Press {interactKey} to {(IsToggled ? "stop" : "start")} magic converter";
                TooltipController.Instance.IsTooltipShowed = value;
                gameObject.layer = value ? LayerMask.NameToLayer("Interactable") : LayerMask.NameToLayer("Default");
                magicConverterHead.layer = value ? LayerMask.NameToLayer("Interactable") : LayerMask.NameToLayer("Default");
                magicCrystal.layer = value ? LayerMask.NameToLayer("Interactable") : LayerMask.NameToLayer("Default");
            }
        }

        public bool IsToggled
        {
            get => _isToggled;
            set
            {
                if (_isToggled == value)
                    return;

                _soundService.Play3DSfx(SoundType.MagicConverter, transform, 3f, 0.4f);
                
                ToggleMagicConverter();
            }
        }

        private void ToggleMagicConverter()
        {
            _potions.Clear();

            if (!IsToggled)
            {
                if (BeginEnchantment())
                {
                    _isToggled = true;
                    AnimateMagicConverter();
                    foreach (var potionPlace in potionPlaces)
                    {
                        potionPlace.IsBlocked = true;
                    }
                        
                    TooltipController.Instance.TooltipMessage =
                        $"Press {interactKey} to {(IsToggled ? "stop" : "start")} magic converter";
                }
            }
            else
            {
                _isToggled = false;
                AnimateMagicConverter();
                foreach (var potionPlace in potionPlaces)
                {
                    potionPlace.IsBlocked = false;
                }
                CurrentEnchantment = null;
            }
        }

        public void Toggle()
        {
            if (IsBlocked)
                return;
            IsToggled = !IsToggled;
        }

        bool BeginEnchantment()
        {
            if (!MagicEngineController.Instance.IsEngineWorking)
            {
                TooltipController.Instance.ShowMechanicsDescription($"Engine not working :(");
                return false;
            }

            int potionCount = 0;
            
            foreach (PotionPlaceScript potionPlace in potionPlaces)
            {
                var potionType = potionPlace.PlacedPotionType;
                if (potionType == MagicEnchanterController.PotionType.Empty)
                {
                    continue;
                }
                if (!_potions.TryAdd(potionType, 1))
                {
                    _potions[potionType]++;
                }
            }

            foreach (var potionType in _potions)
            {
                potionCount += potionType.Value;
            }

            if (potionCount < 3)
            {
                TooltipController.Instance.ShowMechanicsDescription($"Not enough potions");
                return false;
            }
                
            
            Enchantment enchantment = FindCorresponding();
            
            if (enchantment is not null)
            {
                CurrentEnchantment = enchantment;
                
                CrystalColor = CurrentEnchantment.enchantmentColor;
            }
            else
            {
                CurrentEnchantment = null;
                TooltipController.Instance.ShowMechanicsDescription($"No matching enchantment found");
                return false;
            }
            
            return true;
        }

        Enchantment FindCorresponding()
        {
            var isMatch = false;
            
            foreach (var enchantment in enchantmentData.Enchantments)
            {
                
                foreach (var ingredient in enchantment.Ingredients)
                {
                    isMatch = true;
                    
                    var type = ingredient.PotionType;

                    if (!_potions.ContainsKey(type))
                    {
                        isMatch = false;
                        break;
                    }
                    
                    var count = ingredient.Count;

                    if (_potions[type] != count)
                    {
                        isMatch = false;
                        break;
                    }
                }

                if (isMatch)
                {
                    return enchantment.Enchantment;
                }
            }
            
            return null;
        }

        public Coroutine UnsetPotions(float destroyDelay = 1f)
        {
            return StartCoroutine(UnsetPotionsCoroutine(destroyDelay));
        }

        IEnumerator UnsetPotionsCoroutine(float destroyDelay)
        {
            foreach (var potionPlace in potionPlaces)
            {
                yield return potionPlace.DestroyPotion(destroyDelay);
            }
        }
    }
}