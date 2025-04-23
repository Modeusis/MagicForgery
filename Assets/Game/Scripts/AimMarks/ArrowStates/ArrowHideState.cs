using Game.Scripts.Utilities.FSM;
using UnityEngine;

namespace Game.Scripts.TargetMarks.ArrowStates
{
    public class ArrowHideState : State
    {
        private readonly Arrow _arrow;
        
        public ArrowHideState(StateType type, Arrow arrow)
        {
            StateType = type;
            
            _arrow = arrow;
        }
        
        public override void Enter()
        {
            Debug.Log("arrow hide state entered");
            
            _arrow.ResetRotation();
        }

        public override void Update()
        {
            
        }

        public override void Exit()
        {
            
        }
    }
}