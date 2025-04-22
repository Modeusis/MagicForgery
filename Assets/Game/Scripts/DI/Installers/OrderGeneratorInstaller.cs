using System;
using Environment;
using Game.Scripts.AI;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.Scripts.DI.Installers
{
    public class OrderGeneratorInstaller : MonoInstaller
    {
        private OrderGenerator _orderGenerator;

        [Header("Order UI")]
        [SerializeField] private TMP_Text enchantmentName;
        [SerializeField] private TMP_Text enchantmentRequiredAccuracy;
        [SerializeField] private Image enchantmentProgressBar;
        [SerializeField] private CanvasGroup orderCanvasGroup;
        
        [Header("Order generation")] 
        [SerializeField] private EnchantmentData availableEnchantments;
        [SerializeField] private Vector2 timeRange;
        [SerializeField] private Vector2 accuracyRange;
        
        [Header("Sword spawn")]
        [SerializeField] private float swordScaleOnPlace;
        [SerializeField] private GameObject swordPrefab;
        [SerializeField] private Transform swordParent;
        [SerializeField] private Vector3 positionOnPlace;
        [SerializeField] private Vector3 rotationOnPlace;

        [Header("GameFinish setup")] 
        [SerializeField] private int playersToWin = 5;
        [SerializeField] private TMP_Text clientCounter;
        [SerializeField] private GameFinishScreen gameFinishScreen;
        
        public override void InstallBindings()
        {
            Container.Bind<OrderGenerator>().FromMethod(CreateOrderGenerator).AsSingle().NonLazy();
        }

        private OrderGenerator CreateOrderGenerator()
        {
            return new OrderGenerator(
                this,
                
                enchantmentName,
                enchantmentRequiredAccuracy,
                enchantmentProgressBar,
                orderCanvasGroup,
                clientCounter,
                
                availableEnchantments,
                
                swordPrefab,
                swordParent,
                swordScaleOnPlace,
                positionOnPlace,
                rotationOnPlace,
                
                timeRange,
                accuracyRange,
                gameFinishScreen,
                playersToWin
            );
        }

        private void OnDestroy()
        {
            _orderGenerator?.Dispose();
        }
    }
}