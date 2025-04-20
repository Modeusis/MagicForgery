using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Utilities.FSM
{
    public class FSM
    {
        private State _currentState;

        IDictionary<StateType, State> _states;
        
        List<Transition> _transitions;
        
        public FSM(List<Transition> transitions, IDictionary<StateType, State> states, StateType startState)
        {
            _states = states;
            
            _transitions = transitions;
            
            ChangeState(startState);
        }
        
        public void Update()
        {
            _currentState.Update();
            
            for (int i = 0; i < _transitions.Count; i++)
            {
                if (_transitions[i].From == _currentState.StateType && _transitions[i].Condition())
                {
                    ChangeState(_transitions[i].To);
                }
            }
        }

        public void ChangeState(StateType newState)
        {
            if (!_states.TryGetValue(newState, out State state))
            {
                Debug.LogError($"State {newState} not found");
                
                return;
            }
            
            _currentState?.Exit();
            
            _currentState = state;
            
            _currentState?.Enter();
        }
    }
}