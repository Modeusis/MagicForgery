using System;
using Environment;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Game.Scripts.AI
{
    [RequireComponent(typeof(Canvas))]
    public class CustomerOrderGenerator : MonoBehaviour
    {
        [Header("Order UI")]
        [SerializeField] private TMP_Text enchantmentName;
        [SerializeField] private TMP_Text enchantmentRequiredAccuracy;
        [SerializeField] private Image enchantmentProgressBar;
        
        [Header("Customer")]
        [SerializeField] private CustomerMovement customerMovement;

        [Header("Order generation")] 
        [SerializeField] private EnchantmentData availableEnchantments;
        [SerializeField] private Vector2 timeRange;
        [SerializeField] private Vector2 accuracyRange;
        
        private Enchantment _orderEnchantment;
        private float _accuracyMinimum;
        private float _timerValue;

        public float TimerValue
        {
            get => _timerValue;
            set
            {
                if (_timerValue == value)
                    return;
                _timerValue = value;
            }
        }
        public event Action OnCustomerOrderFinished;

        private void OnEnable()
        {
            customerMovement.OnCustomerDestinationReached += StartOrder;
        }
        private void OnDisable()
        {
            customerMovement.OnCustomerDestinationReached -= StartOrder;
        }
        
        private void StartOrder()
        {
            
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
    }
}