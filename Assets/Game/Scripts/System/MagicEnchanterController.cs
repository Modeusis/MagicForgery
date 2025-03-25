using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Environment;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MagicEnchanterController : MonoBehaviour
    {
        //Перенос скрипта зачарованного меча на меч старый при подъеме его
        //Создать точку входа и G
        //Отображения класса зачарования вместо числа (анимированное изображение)
        
        //DTO для зачарования (структура копирующая SO)
        //Полностью переписать систему предметов? или создать temp SO который бы давался в руки
        
        //Написать шейдер изменения цвета зачарования
        
        public static MagicEnchanterController Instance;
        
        [Header("Animation")]
        [SerializeField] private Image progressBar;
        [SerializeField] private GameObject canvasProgressBar;
        [SerializeField] private Material manaFlowMaterial;
        
        [Header("Enchanter components")]
        [SerializeField] private MagicConverterScript magicConverter;
        [SerializeField] private MagicSphereScript magicSphere;
        [SerializeField] private PlaceHolderScript swordCase;
        [SerializeField] private EnchantmentDrawing magicEnchantCircle;
        
        [Header("Sounds")]
        [SerializeField] private AudioClip enchantmentSound;
        
        private bool _isEnchanting;
        public bool IsEnchanting
        {
            get => _isEnchanting;
            set
            {
                if (_isEnchanting == value)
                    return;
                _isEnchanting = value;
                
                if (magicConverter)
                    magicConverter.IsBlocked = value;
                if (magicSphere)
                    magicSphere.IsBlocked = value;
                if (swordCase)
                    swordCase.IsBlocked = value;
            }
        }
        
        private CanvasGroup _canvasGroup;
        private Sword _swordToEnchant;
        private Enchantment _swordEnchantment;

        public Sword SwordToEnchant
        {
            get => _swordToEnchant;
            set
            {
                if (_swordToEnchant == value)
                    return;
                _swordToEnchant = value;
                Debug.Log($"Sword {value} placed");
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
                if (canvasProgressBar)
                {
                    _canvasGroup = canvasProgressBar.GetComponent<CanvasGroup>();
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void EnchantSword()
        {
            
            if (!SwordEnchantment)
            {
                TooltipController.Instance.ShowMechanicsDescription("No enchantment found");
                return;
            }
            
            if (!SwordToEnchant)
            {
                TooltipController.Instance.ShowMechanicsDescription("No sword found");
                return;
            }

            if (SwordToEnchant.IsEnchanted)
            {
                TooltipController.Instance.ShowMechanicsDescription("Sword is already enchanted");
                return;
            }
            
            if (swordCase.IsPlaceHolderOpened)
            {
                TooltipController.Instance.ShowMechanicsDescription("Close sword case");
                return;
            }

            if (MagicEngineController.Instance.ManaAmount < SwordEnchantment.manaCost ||
                MagicEngineController.Instance.WaterAmount < SwordEnchantment.waterCost)
            {
                TooltipController.Instance.ShowMechanicsDescription("Not enough fuel in engine");
                return;
            }
            
            var accuracy = magicEnchantCircle.lastEnchantmentAccuracy;
            
            if (accuracy < 0)
            {
                TooltipController.Instance.ShowMechanicsDescription("Invalid accuracy");
                return;
            }
            
            StartCoroutine(SwordEnchantCoroutine(accuracy));
        }

        IEnumerator SwordEnchantCoroutine(float enchantmentAccuracy,float duration = 4f)
        {
            IsEnchanting = true;
            progressBar.fillAmount = 0f;
            
            MagicEngineController.Instance.SpendMana(SwordEnchantment.manaCost);
            MagicEngineController.Instance.SpendWater(SwordEnchantment.waterCost);
            
            float flowSpeed = manaFlowMaterial.GetFloat("_FlowPower");
            var canvasVisibleCoroutine = StartCoroutine(CanvasGroupFade(0, 1));
            
            yield return canvasVisibleCoroutine;
            
            SwordToEnchant.SwordEnchantment = _swordEnchantment;
            SwordToEnchant.SetAccuracy(enchantmentAccuracy);
            
            yield return magicConverter.UnsetPotions();
            
            manaFlowMaterial.DOFloat(2f , "_FlowPower", 1f); 
            progressBar.DOFillAmount(1, duration).SetEase(Ease.OutSine);
            
            yield return new WaitForSeconds(duration);
            yield return StartCoroutine(CanvasGroupFade(1, 0));
            
            manaFlowMaterial.DOFloat(flowSpeed, "_FlowPower", 1f);
            progressBar.fillAmount = 0f;
            IsEnchanting = false;
            magicConverter.Toggle();
        }

        IEnumerator CanvasGroupFade(float start, float end, float fadeDuration = 0.5f)
        {
            float timer = 0f;
            
            while (timer < fadeDuration)
            {
                var t = timer / fadeDuration;
                _canvasGroup.alpha = Mathf.Lerp(start, end, t);
                timer += Time.deltaTime;
                yield return null;
            }
            
            _canvasGroup.alpha = end;
        }
    }
}
