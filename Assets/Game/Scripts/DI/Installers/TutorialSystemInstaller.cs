using System.Collections.Generic;
using Game.Scripts.Tutorial;
using UnityEngine;
using Zenject;

namespace Game.Scripts.DI.Installers
{
    public class TutorialSystemInstaller : MonoInstaller
    {
        [SerializeField] private List<TutorialStep> tutorialSteps;
        
        public override void InstallBindings()
        {
            Container.Bind<TutorialController>().FromNew().AsSingle().WithArguments(tutorialSteps).NonLazy();
        }
    }
}