using UnityEngine;

namespace Game.Scripts.AI.CustomerStateMachine
{
    public class CustomerMovementState : CustomerBaseState
    {
        public override void EnterState(CustomerStateManager customer)
        {
            customer.customerAnimator.SetFloat("Speed", 1f);
            customer.customerAgent.SetDestination(customer.currentDestinationPoint.position);
        }

        public override void UpdateState(CustomerStateManager customer)
        {
            if (customer.customerAgent.remainingDistance <= customer.customerAgent.stoppingDistance)
            {
                if (customer.currentDestinationPoint == customer.customerOrderDestinationPoint)
                {
                    customer.SwitchState(customer.customerOrderState);
                    customer.transform.LookAt(customer.orderGenerator.transform);
                }
                else
                {
                    customer.DestroyCustomerAndOpenGenerator();
                }
            }
        }

        public override void ExitState(CustomerStateManager customer)
        {
            
        }
    }
}