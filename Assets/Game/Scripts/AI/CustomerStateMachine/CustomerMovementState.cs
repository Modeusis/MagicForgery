using System;
using System.Collections.Generic;
using Game.Scripts.Utilities;
using Game.Scripts.Utilities.FSM;
using UI;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Scripts.AI.CustomerStateMachine
{
    public class CustomerMovementState : State, IDisposable
    {
        private readonly EventBus _eventBus;
        
        private readonly Animator _customerAnimator;
        
        private readonly NavMeshAgent _customerAgent;
        
        private readonly List<CustomerDestination> _destinations;
        
        private CustomerDestination _currentDestination;

        public CustomerMovementState(StateType stateType, EventBus eventBus, Animator customerAnimator, NavMeshAgent customerAgent, List<CustomerDestination> currentDestinations, CustomerDestination startDestination)
        {
            StateType = stateType;
            
            _eventBus = eventBus;
            _eventBus.Subscribe<DestinationType>(HandleDestinationTypeChanged);
            
            _customerAnimator = customerAnimator;
            
            _customerAgent = customerAgent;
            
            _destinations = currentDestinations;
            
            _currentDestination = startDestination;
        }
        
        public override void Enter()
        {
            _customerAnimator.SetFloat("Speed", 1f);
            _customerAgent.SetDestination(_currentDestination.DestinationPosition);
        }

        public override void Update()
        {
            if (_customerAgent.remainingDistance <= _customerAgent.stoppingDistance)
            {
                if (_currentDestination.NextDestinationType == DestinationType.None)
                {
                    _eventBus.Publish("FinishOrder");
                    
                    _eventBus.Unsubscribe<DestinationType>(HandleDestinationTypeChanged);
                    
                    return;
                }
                
                _eventBus.Publish(_currentDestination.NextDestinationType);
            }
        }

        public override void Exit()
        {
            
        }
        
        private void HandleDestinationTypeChanged(DestinationType destinationType)
        {
            if (destinationType == DestinationType.None)
            {
                return;
            }
            
            var destination = GetDestinationByType(destinationType);

            if (destination == null)
            {
                Debug.Log("No destination for customer found");
                
                return;
            }
            
            _currentDestination = destination;
        }

        private CustomerDestination GetDestinationByType(DestinationType destinationType)
        {
            var destination = _destinations.Find(dest => dest.CurrentDestination == destinationType);
            
            if (destination == null)
            {
                Debug.LogError("No such destination type in customer destination list");
                
                return null;
            }

            return destination;
        }

        public void Dispose()
        {
            _eventBus.Unsubscribe<DestinationType>(HandleDestinationTypeChanged);
        }
    }
}