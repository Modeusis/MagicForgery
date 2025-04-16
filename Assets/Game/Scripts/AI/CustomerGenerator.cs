using System;
using System.Collections.Generic;
using Game.Scripts.AI.CustomerStateMachine;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Game.Scripts.AI
{
    public class CustomerGenerator : MonoBehaviour
    {
        [Header("General Settings")]
        [SerializeField] private List<Transform> spawnPoints;
        [SerializeField] private GameObject customerPrefab;
        [SerializeField] private Material customerHeadMaterial;
        
        [Header("Destination points")]
        [SerializeField] private Transform customerOrderDestinationPoint;
        [SerializeField] private Transform customerFinalDestinationPoint;
        
        [Header("Customer order")]
        [field:SerializeField] public CustomerOrderGenerator OrderGenerator { get; private set; }

        public event Action<bool> OnCustomerSpawnChanged;
        public event Action<bool> OnCustomerSpawned;

        private bool _isCustomerSpawned;
        private bool IsCustomerSpawned
        {
            get => _isCustomerSpawned;
            set
            {
                _isCustomerSpawned = value;
                OnCustomerSpawnChanged?.Invoke(!_isCustomerSpawned);
                OnCustomerSpawned?.Invoke(_isCustomerSpawned);
            }
        }
        
        public void GenerateCustomer()
        {
            if (IsCustomerSpawned)
                return;
            
            IsCustomerSpawned = true;
            
            if (!customerPrefab.TryGetComponent(out CustomerStateManager customer))
                return;
            
            customer.customerOrderDestinationPoint = customerOrderDestinationPoint;
            customer.customerFinalDestinationPoint = customerFinalDestinationPoint;
            customer.orderGenerator = OrderGenerator;
            
            if (customerPrefab.TryGetComponent(out CustomerFaceChanger customerFace))
            {
                customer.orderGenerator.faceChanger = customerFace;
                customer.orderGenerator.faceChanger.SetIdleFace();
            }

            
            
            if (spawnPoints.Count > 0)
            {
                customer.spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
            }
            
            var customerInstance = Instantiate(customerPrefab);

            if (customerInstance.TryGetComponent(out CustomerStateManager instancedCustomer))
            {
                instancedCustomer.onCustomerExit.AddListener(ActiveCustomerGenerator);
                if (!customerHeadMaterial)
                    return;
                customerHeadMaterial.color = Random.ColorHSV();
            }
        }

        private void ActiveCustomerGenerator()
        {
            IsCustomerSpawned = false;
        }
    }
}