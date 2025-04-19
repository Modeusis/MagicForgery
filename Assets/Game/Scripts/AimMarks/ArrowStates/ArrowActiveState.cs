using Game.Scripts.Utilities.FSM;
using UnityEngine;

namespace Game.Scripts.TargetMarks.ArrowStates
{
    public class ArrowActiveState : State
    {
        private Transform _arrowTransform;
        
        private TargetMarksConfig _targetsConfig;
        
        public ArrowActiveState(StateType stateType, Transform targetArrow, TargetMarksConfig targets)
        {
            StateType = stateType;
            
            _arrowTransform = targetArrow;
            
            _targetsConfig = targets;
        }
        
        public override void Enter()
        {
            
        }

        public override void Update()
        {
            
        }

        public override void Exit()
        {
            
        }
    }
}