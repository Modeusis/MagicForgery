using UnityEngine;

namespace Game.Scripts.AI.CustomerStateMachine
{
    public class CustomerOrderState : CustomerBaseState
    {
        public override void EnterState(CustomerStateManager customer)
        {
            Debug.Log("Enter Customer Order State");
            customer.OrderGenerator.StartOrder();
        }

        public override void UpdateState(CustomerStateManager customer)
        {
            customer.CustomerRaycaster(customer.OrderGenerator.FinishOrder);
            
            if (customer.OrderGenerator.isFinished)
            {
                customer.SwitchState(customer.customerMovementState);
            }
        }

        public override void ExitState(CustomerStateManager customer)
        {
            customer.currentDestinationPoint = customer.customerFinalDestinationPoint;
        }
    }
}