using System;
using System.Collections;
using Environment;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Game.Scripts.AI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class CustomerOrderGenerator : MonoBehaviour
    {
        [Header("Order UI")]
        [SerializeField] private TMP_Text enchantmentName;
        [SerializeField] private TMP_Text enchantmentRequiredAccuracy;
        [SerializeField] private Image enchantmentProgressBar;

        [Header("Order generation")] 
        [SerializeField] private EnchantmentData availableEnchantments;
        [SerializeField] private Vector2 timeRange;
        [SerializeField] private Vector2 accuracyRange;
        
        [Header("Sword spawn")]
        [SerializeField] private GameObject swordPrefab;
        [SerializeField] private Transform swordParent;
        [SerializeField] private float swordScaleOnPlace;
        [SerializeField] private Vector3 positionOnPlace;
        [SerializeField] private Vector3 rotationOnPlace;
        
        private CanvasGroup _orderCanvasGroup;
        private Enchantment _orderEnchantment;
        private float _accuracyMinimum;
        private float _timerValue;
        private Coroutine _timerCoroutine;

        public bool isFinished;

        private void Start()
        {
            _orderCanvasGroup = GetComponent<CanvasGroup>();
        }
        
        public void StartOrder()
        {
            if (GenerateOrder())
            {
                enchantmentName.text = _orderEnchantment.name;
                enchantmentRequiredAccuracy.text = _accuracyMinimum.ToString();
                
                StartCoroutine(SetCanvasAlpha(1f));
                
                if (GenerateSword())
                {
                    _timerCoroutine = StartCoroutine(TimerCoroutine(_timerValue, ExpireOrder));
                }
            }
        }

        private bool GenerateOrder()
        {
            var enchantmentList = availableEnchantments.Enchantments;
            int enchantmentCount = enchantmentList.Count;
            
            if (enchantmentCount <= 0)
            {
                Debug.LogWarning("No Enchantment available");
                return false;
            }
            
            var enchantmentId = Random.Range(0, enchantmentCount);
            
            _orderEnchantment = enchantmentList[enchantmentId].Enchantment;
            _accuracyMinimum = Mathf.Round(Random.Range(accuracyRange.x, accuracyRange.y));
            _timerValue = Random.Range(timeRange.x, timeRange.y);

            return true;
        }

        private bool ValidateOrder(Sword enchantedSword)
        {
            if (!enchantedSword.IsEnchanted)
            {
                TooltipController.Instance.ShowMechanicsDescription("Sword is not enchanted");
                return false;   
            }
            if (enchantedSword.SwordEnchantment != _orderEnchantment)
            {
                TooltipController.Instance.ShowMechanicsDescription("Wrong enchantment");
                return false;
            }

            if (enchantedSword.EnchantmentAccuracy < _accuracyMinimum / 100)
            {
                TooltipController.Instance.ShowMechanicsDescription("Bad enchantment");
                return false;
            }
            return true;
        }

        private void ResetOrderGUI()
        {
            _orderEnchantment = null;
            _accuracyMinimum = 0;
            _timerValue = 0;
            
            enchantmentName.text = "";
            enchantmentRequiredAccuracy.text = "";
            
            StartCoroutine(SetCanvasAlpha(1f, false));
        }

        public void FinishOrder()
        {
            var selectedItem = Player.Player.instance.selectedItem;
            if (!selectedItem)
                return;
            
            selectedItem.prefab.TryGetComponent(out Sword selectedSword);
            
            if (ValidateOrder(selectedSword))
            {
                Inventory.instance.RemoveItem();
                isFinished = true;
                if (_timerCoroutine != null)
                {
                    StopCoroutine(_timerCoroutine);
                }
                ResetOrderGUI();
            }
        }
        
        private void ExpireOrder()
        {
            isFinished = true;
            ResetOrderGUI();
        }

        private bool GenerateSword()
        {
            if (!swordPrefab || !swordParent)
            {
                return false;
            }
            
            var sword = Instantiate(swordPrefab, swordParent);
            sword.transform.localScale *= swordScaleOnPlace;
            sword.transform.localPosition = positionOnPlace;
            sword.transform.localRotation = Quaternion.Euler(rotationOnPlace);

            return true;
        }
        
        private IEnumerator TimerCoroutine(float time, Action callback = null)
        {
            var timer = 0f;
            while (timer < time)
            {
                var t = timer / time;
                enchantmentProgressBar.fillAmount = 1 - t;
                timer += Time.deltaTime;
                yield return null;
            }

            enchantmentProgressBar.fillAmount = 0f;
            
            callback?.Invoke();
        }

        private IEnumerator SetCanvasAlpha(float duration, bool visible = true)
        {
            var timer = 0f;
            while (timer < duration)
            {
                var t = timer / duration;
                _orderCanvasGroup.alpha = visible ? t : 1 - t;
                timer += Time.deltaTime;
                yield return null;
            }
        }
    }
}