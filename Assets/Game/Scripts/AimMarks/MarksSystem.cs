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
        
        [SerializeField] private float _disappearTime = 0.5f;
        [SerializeField] private float _appearTime = 0.5f;
        [SerializeField] private float _rotationTime = 0.5f;
        
        [SerializeField] private int wordsOnOneRow = 8;
        [SerializeField] private float hideGuidLineDistance = 2f;
        
        [SerializeField] private TargetMarksConfig _targets;
        
        [SerializeField] private Transform _arrowTransform;
        
        [SerializeField] private TMP_Text _stepTextField;
        
        private FSM _arrowTargetStateMachine;
        
        [Inject]
        private void Initialize(EventBus eventBus)
        {
            _eventBus = eventBus;
            
            var arrow = new Arrow(_arrowTransform, _appearTime, _disappearTime, _rotationTime);

            var idleState = new ArrowIdleState(StateType.Idle, arrow);
            var hideState = new ArrowIdleState(StateType.Hide, arrow);
            var activeState = new ArrowActiveState(StateType.Active, _targets, arrow, _stepTextField,
                _eventBus, wordsOnOneRow, hideGuidLineDistance);
            
            var transitions = new List<Transition>()
            {
                new Transition(StateType.Idle, StateType.Active, () => _eventBus.WasInvokedThisFrame<MarkType>()),
                new Transition(StateType.Active, StateType.Active, () => _eventBus.WasInvokedThisFrame<MarkType>()),
                new Transition(StateType.Active, StateType.Hide, () => _eventBus.WasInvokedThisFrame<TagCloseToAim>()),
                new Transition(StateType.Hide, StateType.Active, () => _eventBus.WasInvokedThisFrame<MarkType>())
                
            };

            var states = new Dictionary<StateType, State>()
            {
                { StateType.Idle, idleState },
                { StateType.Active, activeState },
                { StateType.Hide, hideState },
            };
            
            _arrowTargetStateMachine = new FSM(transitions, states, StateType.Idle);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                _eventBus.Publish(MarkType.Engine);
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                _eventBus.Publish(MarkType.DrawWell);
            }

            if (Input.GetKeyDown(KeyCode.K))
            {
                _eventBus.Publish(MarkType.EnchantmentBook);
            }
            
            _arrowTargetStateMachine?.Update();
        }
    }
}