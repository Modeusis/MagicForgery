using Game.Scripts.Utilities;
using UnityEngine;

namespace Game.Scripts.AI.CustomerStateMachine
{
    public class CustomerOrderState : CustomerBaseState
    {
        public override void EnterState(CustomerStateManager customer)
        {
            EventBus.Instance.Publish(true);
            
            customer.customerAnimator.SetFloat("Speed", 0f);
            customer.orderGenerator.StartOrder();
            customer.tag = "Customer";
        }

        public override void UpdateState(CustomerStateManager customer)
        {
            customer.CustomerRaycaster(customer.orderGenerator.FinishOrder);
            
            if (customer.orderGenerator.isFinished)
            {
                customer.SwitchState(customer.customerMovementState);
            }
        }

        public override void ExitState(CustomerStateManager customer)
        {
            EventBus.Instance.Publish(false);
            
            customer.currentDestinationPoint = customer.customerFinalDestinationPoint;
            customer.orderGenerator.isFinished = false;
            customer.tag = "Untagged";
        }
    }
}