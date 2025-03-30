using System.Collections.Generic;
using Game.Scripts.AI.CustomerStateMachine;
using UnityEngine;

namespace Game.Scripts.AI
{
    public class CustomerGenerator : MonoBehaviour
    {
        [Header("General Settings")]
        [SerializeField] private List<Transform> spawnPoints;
        [SerializeField] private GameObject customerPrefab;
        
        [Header("Destination points")]
        [SerializeField] private Transform customerOrderDestinationPoint;
        [SerializeField] private Transform customerFinalDestinationPoint;
        
        [Header("Customer order")]
        [field:SerializeField] public CustomerOrderGenerator OrderGenerator { get; private set; }

        private bool _isCustomerSpawned;
        
        private void GenerateCustomer()
        {
            if (_isCustomerSpawned)
                return;
            
            _isCustomerSpawned = true;
            
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
            }
        }

        private void ActiveCustomerGenerator()
        {
            _isCustomerSpawned = false;
        }
    }
}