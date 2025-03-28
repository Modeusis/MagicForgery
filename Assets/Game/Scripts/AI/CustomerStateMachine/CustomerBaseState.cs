namespace Game.Scripts.AI.CustomerStateMachine
{
    public abstract class CustomerBaseState
    {
        public abstract void EnterState(CustomerStateManager customer);
        public abstract void UpdateState(CustomerStateManager customer);
        public abstract void ExitState(CustomerStateManager customer);
    }
}