using System;
using Game.Scripts.Utilities;
using Game.Scripts.Utilities.FSM;
using UnityEngine;

namespace Game.Scripts.AI.CustomerStateMachine
{
    public class CustomerOrderState : State
    {
        private readonly EventBus _eventBus;
        
        private readonly Animator _customerAnimator;
        
        private readonly OrderGenerator _orderGenerator;

        public CustomerOrderState(StateType stateType, EventBus eventBus, Animator customerAnimator, OrderGenerator orderGenerator)
        {
            StateType = stateType;
            
            _eventBus = eventBus;
            
            _customerAnimator = customerAnimator;
            
            _orderGenerator = orderGenerator;
        }
        
        public override void Enter()
        {
            _eventBus.Publish(true);
            
            _customerAnimator.SetFloat("Speed", 0f);
            _orderGenerator.StartOrder();
            
            _customerAnimator.transform.LookAt(_orderGenerator.GetOrderPosition());
            _customerAnimator.tag = "Customer";
        }

        public override void Update()
        {
            CastRayFromCustomer(_orderGenerator.FinishOrder);
        }

        public override void Exit()
        {
            _eventBus.Publish(false);
            
            _customerAnimator.tag = "Untagged";
        }
        
        public void CastRayFromCustomer(Action onRaycastHitCustomer)
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