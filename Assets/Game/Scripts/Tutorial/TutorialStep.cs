using System;
using Game.Scripts.TargetMarks;
using UnityEngine;

namespace Game.Scripts.Tutorial
{
    [Serializable]
    public class TutorialStep
    {
        public int StepId { get; private set; }
        [field: SerializeField] public MarkType TutorialMark { get; private set; }
        
        [field: SerializeField] public bool IsCompleted { get; private set; }

        public event Action<int> OnCompleted;
        
        public void SetId(int stepId)
        {
            StepId = stepId;
        }
        
        public void Reset()
        {
            IsCompleted = false;
        }

        public void Complete(int stepId)
        {
            IsCompleted = true;
            
            OnCompleted?.Invoke(stepId);
        }
    }
}