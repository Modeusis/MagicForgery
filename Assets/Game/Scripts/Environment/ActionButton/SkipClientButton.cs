using Game.Scripts.AI;
using Game.Scripts.Utilities;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Environment
{
    [RequireComponent(typeof(BoxCollider))]
    public class SkipClientButton : ActionButton
    {
        private EventBus _eventBus;
        
        private OrderGenerator _orderGenerator;
        
        [Inject]
        private void Initialize(EventBus eventBus, OrderGenerator orderGenerator)
        {
            _eventBus = eventBus;
            
            _orderGenerator = orderGenerator;
        }

        private void OnEnable()
        {
            SetButtonColor(false);
            OnPressed += _orderGenerator.SkipOrder;
            _eventBus.Subscribe<bool>(SetButtonColor);
        }

        private void OnDisable()
        {
            OnPressed -= _orderGenerator.SkipOrder;
            _eventBus.Unsubscribe<bool>(SetButtonColor);
        }
        
    }
}