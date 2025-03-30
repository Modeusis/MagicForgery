using System;
using Game.Scripts.AI.CustomerStateMachine;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Game.Scripts.AI.CustomerStateMachine
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CustomerFaceChanger))]
    public class CustomerStateManager : MonoBehaviour
    {
        public Transform spawnPoint;
        public Transform customerOrderDestinationPoint;
        public Transform customerFinalDestinationPoint;
        public CustomerOrderGenerator orderGenerator;
        
        CustomerBaseState _customerCurrentState;
        public CustomerMovementState customerMovementState = new();
        public CustomerOrderState customerOrderState = new();
        
        public NavMeshAgent customerAgent;
        public Animator customerAnimator;
        
        public Transform currentDestinationPoint;

        public UnityEvent onCustomerExit;
        public void SwitchState(CustomerBaseState state)
        {
            _customerCurrentState?.ExitState(this);
            _customerCurrentState = state;
            _customerCurrentState.EnterState(this);
        }
        private void Start()
        {
            transform.position = spawnPoint.position;
            customerAgent = GetComponent<NavMeshAgent>();
            customerAnimator = GetComponent<Animator>();
            currentDestinationPoint = customerOrderDestinationPoint;
            SwitchState(customerMovementState);
        }

        private void Update()
        {
            _customerCurrentState?.UpdateState(this);
        }

        public void CustomerRaycaster(Action onRaycastHitCustomer)
        {
            Ray ray = Player.Player.instance.mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 4f, LayerMask.GetMask("Customer")) && hit.collider.CompareTag("Customer"))
            {
                if (Input.GetKeyDown(Player.Player.instance.InteractKey))
                {
                    onRaycastHitCustomer?.Invoke();
                }
            }
        }

        public void DestroyCustomerAndOpenGenerator()
        {
            onCustomerExit?.Invoke();
            Destroy(gameObject);
        }
    }
}

