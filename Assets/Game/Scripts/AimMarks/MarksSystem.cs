using System.Collections.Generic;
using Game.Scripts.TargetMarks.ArrowStates;
using Game.Scripts.Utilities;
using Game.Scripts.Utilities.FSM;
using TMPro;
using UnityEngine;
using Zenject;

namespace Game.Scripts.TargetMarks
{
    public class MarksSystem : MonoBehaviour
    {
        private EventBus _eventBus;
        
        private TargetMarksConfig _targets;
        
        private Transform _arrowTransform;
        
        private TMP_Text _stepTextField;
        
        private float _disappearTime = 0.5f;
        private float _appearTime = 0.5f;
        private float _rotationTime = 0.5f;
        
        private FSM _arrowTargetStateMachine;
        
        [Inject]
        private void Initialize(EventBus eventBus)
        {
            _eventBus = eventBus;
            
            var arrow = new Arrow(_arrowTransform, _appearTime, _disappearTime, _rotationTime);

            var idleState = new ArrowIdleState(StateType.Idle, arrow);
            var activeState = new ArrowActiveState(StateType.Active, _targets, arrow, _stepTextField, _eventBus);
            
            var transitions = new List<Transition>()
            {
                new Transition(StateType.Idle, StateType.Active, () => _eventBus.WasInvokedThisFrame<MarkType>()),
                new Transition(StateType.Active, StateType.Active, () => _eventBus.WasInvokedThisFrame<MarkType>()),
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
            _eventBus.Publish(mark);   
        }
    }
}