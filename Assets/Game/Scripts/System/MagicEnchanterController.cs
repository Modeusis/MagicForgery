using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Environment;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MagicEnchanterController : MonoBehaviour
    {
        //Удаление текущего зачарования, при выполнении зачарования либо при dismiss в окне
        //Перенос скрипта зачарованного меча на меч старый при подъеме его
        //Постановка спрайта меча не вручную а из объекта
        //Мини игра для определения качества зачарования
        public static MagicEnchanterController Instance;
        
        [Header("Animation")]
        [SerializeField] private Image progressBar;
        [SerializeField] private GameObject canvasProgressBar;
        [SerializeField] private Material manaFlowMaterial;
        
        [Header("Enchanter components")]
        [SerializeField] private MagicConverterScript magicConverter;
        [SerializeField] private PlaceHolderScript swordCase;
        
        
        [Header("Sounds")]
        [SerializeField] private AudioClip enchantmentSound;
        
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
                
                Debug.Log($"Enchanting {_swordToEnchant}");
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
                
                Debug.Log($"Enchanted by {_swordEnchantment.enchantmentName}");
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

        public void EnchantSword(float accuracy)
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
            
            StartCoroutine(SwordEnchantCoroutine(() =>
            {
                SwordToEnchant.SwordEnchantment = _swordEnchantment;
                SwordToEnchant.SetAccuracy(accuracy);
            }));
        }

        IEnumerator SwordEnchantCoroutine(Action callback, float duration = 4f)
        {
            float timer = 0f;
            float fadeDuration = 0.5f;
            
            float flowSpeed = manaFlowMaterial.GetFloat("_FlowPower");
            
            while (timer < fadeDuration)
            {
                var t = timer / fadeDuration;
                _canvasGroup.alpha = Mathf.Lerp(0f, 1f, t);
                timer += Time.deltaTime;
                yield return null;
            }
            bool isDone = false;
            
            manaFlowMaterial.DOFloat(1f , "_FlowPower", 1f);

            progressBar.DOFillAmount(1f, duration).OnComplete(() =>
            {
                isDone = true;
            });
            
            yield return new WaitUntil(() => isDone);
            
            manaFlowMaterial.DOFloat(flowSpeed, "_FlowPower", 1f);
            
            timer = 0f;
            
            while (timer < fadeDuration)
            {
                var t = timer / fadeDuration;
                _canvasGroup.alpha = Mathf.Lerp(1f, 0f, t);
                timer += Time.deltaTime;
                yield return null;
            }

            _canvasGroup.alpha = 0f;
            progressBar.fillAmount = 0f;
            
            callback?.Invoke();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                EnchantSword(0.8f);
            }
        }
    }
}
