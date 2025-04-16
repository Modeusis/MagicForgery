using Game.Scripts.AI;
using Game.Scripts.Utilities;
using UnityEngine;

namespace Environment
{
    [RequireComponent(typeof(BoxCollider))]
    public class SkipClientButton : ActionButton
    {
        [SerializeField] private CustomerOrderGenerator customerOrderGenerator;

        private void OnEnable()
        {
            SetButtonColor(false);
            OnPressed += customerOrderGenerator.SkipOrder;
            EventBus.Instance.Subscribe<bool>(SetButtonColor);
        }

        private void OnDisable()
        {
            OnPressed -= customerOrderGenerator.SkipOrder;
            EventBus.Instance.Unsubscribe<bool>(SetButtonColor);
        }
    }
}