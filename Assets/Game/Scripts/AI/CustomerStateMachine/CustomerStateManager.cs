using System;
using Game.Scripts.AI.CustomerStateMachine;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Scripts.AI.CustomerStateMachine
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class CustomerStateManager : MonoBehaviour
    {
        [Header("Destination points")]
        public Transform customerOrderDestinationPoint;
        public Transform customerFinalDestinationPoint;
        
        [Header("Customer order")]
        [field:SerializeField] public CustomerOrderGenerator OrderGenerator { get; private set; }
        
        CustomerBaseState _customerCurrentState;
        public CustomerMovementState customerMovementState = new();
        public CustomerOrderState customerOrderState = new();
        
        public NavMeshAgent customerAgent;
        public Transform currentDestinationPoint;
        
        public void SwitchState(CustomerBaseState state)
        {
            _customerCurrentState?.ExitState(this);
            _customerCurrentState = state;
            _customerCurrentState.EnterState(this);
        }
        private void Start()
        {
            customerAgent = GetComponent<NavMeshAgent>();
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
    }
}

