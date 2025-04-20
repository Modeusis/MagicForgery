using Game.Scripts.Utilities.FSM;

namespace Game.Scripts.TargetMarks.ArrowStates
{
    public class ArrowIdleState : State
    {
        private readonly Arrow _arrow;
        
        public ArrowIdleState(StateType type, Arrow arrow)
        {
            StateType = type;
            
            _arrow = arrow;
        }
        
        public override void Enter()
        {
            _arrow.HideArrow();
        }

        public override void Update()
        {
            
        }

        public override void Exit()
        {
            
        }
    }
}