using System;
using Game.Scripts.Tutorial;
using Zenject;

namespace Environment
{
    public class StartTutorialActionButton : ActionButton
    {
        [Inject] private TutorialController _tutorialController;
        private void OnEnable()
        {
            SetButtonColor(true);
            OnPressed += _tutorialController.StartTutorial;
        }

        private void OnDisable()
        {
            OnPressed -= _tutorialController.StartTutorial;
        }
    }
}