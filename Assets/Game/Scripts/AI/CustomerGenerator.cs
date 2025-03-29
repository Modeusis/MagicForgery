using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.AI
{
    public class CustomerGenerator : MonoBehaviour
    {
        [SerializeField] private List<Transform> spawnPoints;
        [SerializeField] private GameObject customerPrefab;
        [SerializeField] private Transform customerParentObject;
        
        private GameObject _customerInstance;
        private void GenerateCustomer()
        {
            _customerInstance = Instantiate(customerPrefab, customerParentObject);
            if (spawnPoints.Count > 0)
            {
                transform.position = spawnPoints[Random.Range(0, spawnPoints.Count)].position;
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                GenerateCustomer();
            }
        }
    }
}