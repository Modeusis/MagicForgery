using System;
using Game.Scripts.Utilities;
using Zenject;

namespace Game.Scripts.DI.Installers
{
    public class EventSystemInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<EventBus>().AsSingle().NonLazy();
        }
    }
}