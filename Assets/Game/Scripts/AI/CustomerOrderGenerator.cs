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
        
        public void StartOrder()
        {
            if (GenerateOrder())
            {
                _timerCoroutine = StartCoroutine(TimerCoroutine(_timerValue, ExpireOrder));
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
            _accuracyMinimum = Random.Range(accuracyRange.x, accuracyRange.y);
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

            if (enchantedSword.EnchantmentAccuracy < _accuracyMinimum)
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
            
            callback?.Invoke();
        }
    }
}