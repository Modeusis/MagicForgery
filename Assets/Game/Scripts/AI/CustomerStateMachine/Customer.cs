using System;
using System.Collections.Generic;
using Game.Scripts.Utilities;
using Game.Scripts.Utilities.FSM;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Zenject;

namespace Game.Scripts.AI.CustomerStateMachine
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CustomerFaceChanger))]
    public class Customer : MonoBehaviour
    {
        private EventBus _eventBus;
        
        private FSM _customerStateMachine;
        
        [SerializeField] private List<CustomerDestination> destinations;
        
        private CustomerDestination _startDestination;
        
        private OrderGenerator _orderGenerator;
        
        private NavMeshAgent _customerAgent;
        private Animator _customerAnimator;

        public UnityEvent onCustomerExit;
        
        private Transform _spawnPoint;
        
        public void Initialize(EventBus eventBus, OrderGenerator orderGenerator, Transform spawnPoint, DestinationType destinationType)
        {
            _eventBus = eventBus;
            
            _eventBus.Subscribe<string>(HandleLastDestinationReached);
            
            _orderGenerator = orderGenerator;

            _startDestination = GetDestinationByType(destinationType);
            
            _spawnPoint = spawnPoint;
            
            transform.position = _spawnPoint.position;
            
            _customerAgent = GetComponent<NavMeshAgent>();
            _customerAnimator = GetComponent<Animator>();

            var transitions = new List<Transition>()
            {
                new Transition(StateType.Movement, StateType.Order, () => _eventBus.WasInvokedThisFrame<DestinationType>()),
                new Transition(StateType.Order, StateType.Movement, () => _orderGenerator.IsOrderFinished()),
            };

            var states = new Dictionary<StateType, State>()
            {
                { StateType.Movement , new CustomerMovementState(StateType.Movement, _eventBus, _customerAnimator, _customerAgent, destinations, _startDestination)},
                { StateType.Order , new CustomerOrderState(StateType.Order, _eventBus, _customerAnimator, _orderGenerator)}
            };
            
            _customerStateMachine = new FSM(transitions, states, StateType.Movement);
        }

        private void Update()
        {
            _customerStateMachine?.Update();
        }

        private void LateUpdate()
        {
            _customerStateMachine?.LateUpdate();
        }

        private void OnDestroy()
        {
            _eventBus.Unsubscribe<string>(HandleLastDestinationReached);
        }

        private void HandleLastDestinationReached(string finishCall)
        {
            if (finishCall == "FinishOrder")
            {
                DestroyCustomerAndOpenGenerator();
            }
        }

        private CustomerDestination GetDestinationByType(DestinationType destinationType)
        {
            var destination = destinations.Find(dest => dest.CurrentDestination == destinationType);
            
            if (destination == null)
            {
                Debug.LogError("No such destination type in customer destination list");
                
                return null;
            }

            return destination;
        }
        
        private void DestroyCustomerAndOpenGenerator()
        {
            onCustomerExit?.Invoke();
            
            Destroy(gameObject);
        }
    }
}

