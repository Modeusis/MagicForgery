using UnityEngine;

namespace Game.Scripts.AI.CustomerStateMachine
{
    public class CustomerMovementState : CustomerBaseState
    {
        public override void EnterState(CustomerStateManager customer)
        {
            Debug.Log("Enter Customer Movement State");
            customer.customerAgent.SetDestination(customer.currentDestinationPoint.position);
        }

        public override void UpdateState(CustomerStateManager customer)
        {
            if (customer.customerAgent.remainingDistance <= customer.customerAgent.stoppingDistance)
            {
                if (customer.currentDestinationPoint == customer.customerOrderDestinationPoint)
                {
                    customer.SwitchState(customer.customerOrderState);
                }
            }
        }

        public override void ExitState(CustomerStateManager customer)
        {
            
        }
    }
}