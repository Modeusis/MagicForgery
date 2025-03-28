using System;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Scripts.AI
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class CustomerMovement : MonoBehaviour
    {
        [SerializeField] private Transform customerDestination;
        [SerializeField] private Transform finalDestination;
        
        private bool _isMoving;
        private Vector3 _destination;

        public Vector3 Destination
        {
            get => _destination;
            set
            {
                if (_destination == value)
                {
                    return;   
                }
                
                _destination = value;
                _navAgent.SetDestination(_destination);
                _isMoving = true;
            }
        }
        private NavMeshAgent _navAgent;
        
        private void Start()
        {
            _navAgent = GetComponent<NavMeshAgent>();
        }

        private void OnEnable()
        {
            _navAgent.SetDestination(customerDestination.position);
        }

        void Update()
        {
            if (_navAgent.remainingDistance == 0 && _isMoving)
            {
                OnCustomerDestinationReached?.Invoke();
            }
        }

        public event Action OnCustomerDestinationReached;
    }
}