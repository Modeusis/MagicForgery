using System;
using System.Collections.Generic;
using Game.Scripts.AI.CustomerStateMachine;
using Game.Scripts.Utilities;
using Sounds;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;
using Random = UnityEngine.Random;

namespace Game.Scripts.AI
{
    public class CustomerGenerator : MonoBehaviour
    {
        private EventBus _eventBus;
        
        private OrderGenerator _orderGenerator;
        
        private SoundService _soundService;
        
        [Header("General Settings")]
        [SerializeField] private List<Transform> spawnPoints;
        [SerializeField] private GameObject customerPrefab;
        [SerializeField] private Material customerHeadMaterial;
        
        [Header("Destination points")]
        [SerializeField] private DestinationType startDestination;

        public event Action<bool> OnCustomerSpawnChanged;

        private bool _isCustomerSpawned;
        private bool IsCustomerSpawned
        {
            get => _isCustomerSpawned;
            set
            {
                _isCustomerSpawned = value;
                OnCustomerSpawnChanged?.Invoke(!_isCustomerSpawned);
            }
        }
        
        [Inject]
        private void Initialize(EventBus eventBus, SoundService soundService,OrderGenerator orderGenerator)
        {
            _eventBus = eventBus;
            
            _soundService = soundService;
            
            _orderGenerator = orderGenerator;
        }
        
        public void GenerateCustomer()
        {
            if (IsCustomerSpawned)
                return;

            if (spawnPoints.Count == 0)
            {
                Debug.LogError("No spawn points for customer.");
                
                return;
            }
            
            IsCustomerSpawned = true;

            var customerInstance = Instantiate(customerPrefab);
            // customerInstance.SetActive(false);
            customerInstance.SetActive(true);
            
            if (!customerInstance.TryGetComponent(out StepSoundScript stepSoundScript))
                return;

            stepSoundScript.Initialize(_soundService);
            
            if (!customerInstance.TryGetComponent(out Customer customer))
                return;
            
            customer.Initialize(_eventBus, _orderGenerator, spawnPoints[Random.Range(0, spawnPoints.Count)], startDestination);
            customer.onCustomerExit.AddListener(ActiveCustomerGenerator);
            
            if (customerInstance.TryGetComponent(out CustomerFaceChanger customerFace))
            {
                _orderGenerator.faceChanger = customerFace;
                _orderGenerator.faceChanger.SetIdleFace();
            }
            
            customerInstance.SetActive(true);

            // if (customerInstance.TryGetComponent(out Customer instancedCustomer))
            // {
            //     instancedCustomer.onCustomerExit.AddListener(ActiveCustomerGenerator);
            //     if (!customerHeadMaterial)
            //         return;
            //     customerHeadMaterial.color = Random.ColorHSV();
            // }
        }

        private void ActiveCustomerGenerator()
        {
            IsCustomerSpawned = false;
        }
    }
}