using Game.Scripts.Tutorial;
using Zenject;

namespace Game.Scripts.DI.Installers
{
    public class TutorialSystemInstaller : MonoInstaller
    {
        private TutorialController _tutorialController;
        
        public override void InstallBindings()
        {
            
            
            Container.Bind<TutorialController>().FromInstance(_tutorialController).AsSingle().NonLazy();
        }
    }
}