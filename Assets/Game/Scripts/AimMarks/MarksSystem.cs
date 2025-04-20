using System.Collections.Generic;
using Game.Scripts.TargetMarks.ArrowStates;
using Game.Scripts.Utilities;
using Game.Scripts.Utilities.FSM;
using TMPro;
using UnityEngine;

namespace Game.Scripts.TargetMarks
{
    public class MarksSystem : MonoBehaviour
    {
        [SerializeField] private TargetMarksConfig targets;
        
        [SerializeField] private Transform arrowTransform;
        
        [SerializeField] private TMP_Text stepTextField;
        
        [SerializeField] private float disappearTime = 0.5f;
        [SerializeField] private float appearTime = 0.5f;
        [SerializeField] private float rotationTime = 0.5f;
        
        private FSM _arrowTargetStateMachine;
        
        public void Awake()
        {
            var arrow = new Arrow(arrowTransform, appearTime, disappearTime, rotationTime);

            var idleState = new ArrowIdleState(StateType.Idle, arrow);
            var activeState = new ArrowActiveState(StateType.Active, targets, arrow, stepTextField);
            
            var transitions = new List<Transition>()
            {
                new Transition(StateType.Idle, StateType.Active, () => EventBus.Instance.WasInvokedThisFrame<MarkType>()),
                new Transition(StateType.Active, StateType.Active, () => EventBus.Instance.WasInvokedThisFrame<MarkType>()),
                // new Transition(StateType.Active, StateType.Idle, () => EventBus.Instance.WasInvokedThisFrame<MarkType>()),
                
            };

            var states = new Dictionary<StateType, State>()
            {
                { StateType.Idle, idleState },
                { StateType.Active, activeState },
            };
            
            _arrowTargetStateMachine = new FSM(transitions, states, StateType.Idle);
        }

        private void Update()
        {
            _arrowTargetStateMachine?.Update();
        }

        public void ChangeTarget(MarkType mark)
        {
            EventBus.Instance.Publish(mark);   
        }
    }
}