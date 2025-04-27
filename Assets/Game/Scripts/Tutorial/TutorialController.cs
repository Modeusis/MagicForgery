using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Utilities;
using UnityEngine;

namespace Game.Scripts.Tutorial
{
    public class TutorialController
    {
        private readonly EventBus _eventBus;
        
        private readonly Dictionary<int, TutorialStep> _tutorialStepsOrdered;
        
        private TutorialStep _currentTutorialStep;

        private TutorialStep CurrentTutorialStep
        {
            set
            {
                if (_currentTutorialStep != null)
                {
                    _currentTutorialStep.OnCompleted -= SelectNextStep;
                }
                
                _currentTutorialStep = value;
                _currentTutorialStep.OnCompleted += SelectNextStep;
            }
        }
        
        public TutorialController(List<TutorialStep> tutorialSteps, EventBus eventBus)
        {
            _eventBus = eventBus;
            
            if (tutorialSteps.Count < 1)
            {
                Debug.Log("Unable to initialize tutorial");
                
                return;
            }
            
            _tutorialStepsOrdered = new Dictionary<int, TutorialStep>();
            
            for (int i = 0; i < tutorialSteps.Count; i++)
            {
                tutorialSteps[i].SetId(i);
                _tutorialStepsOrdered.Add(i, tutorialSteps[i]);
            }
        }

        public void StartTutorial()
        {
            foreach (var tutorialStep in _tutorialStepsOrdered)
            {
                tutorialStep.Value.Reset();
            }
            
            CurrentTutorialStep = _tutorialStepsOrdered[0];
            
            _eventBus?.Publish(_currentTutorialStep.TutorialMark);
        }
        
        public void CompleteStep(int stepId)
        {
            if (_tutorialStepsOrdered[stepId].IsCompleted)
            {
                return;
            }
            
            _tutorialStepsOrdered[stepId].Complete(stepId);
        }

        private void SelectNextStep(int stepId)
        {
            if (stepId != _currentTutorialStep.StepId)
                return;
            
            for (int i = _currentTutorialStep.StepId + 1; i < _tutorialStepsOrdered.Count; i++)
            {
                if (_tutorialStepsOrdered[i].IsCompleted) 
                    continue;
                
                CurrentTutorialStep = _tutorialStepsOrdered[i];
                
                break;
            }
            
            _eventBus?.Publish(_currentTutorialStep.TutorialMark);
        }
    }
}