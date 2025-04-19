using System;
using System.Collections.Generic;
using Game.Scripts.Utilities;
using Game.Scripts.Utilities.FSM;
using UnityEngine;

namespace Game.Scripts.TargetMarks
{
    public class MarksSystem : MonoBehaviour
    {
        [SerializeField] private TargetMarksConfig targets;
        
        private FSM _arrowTargetStateMachine;
        
        
        public void Awake()
        {
            
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