using System;
using DG.Tweening;
using Game.Scripts.AI;
using Game.Scripts.Interface;
using Game.Scripts.Tutorial;
using Sounds;
using UI;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Environment
{
    [RequireComponent(typeof(BoxCollider))]
    public class NextClientButton : ActionButton
    {
        [Inject] private TutorialController _tutorialController;
        
        [Header("Tutorial")]
        [SerializeField] private int stepId = 5;
        
        [SerializeField] private CustomerGenerator customerSpawner;
        
        private void OnEnable()
        {
            SetButtonColor(true);
            OnPressed += HandleButtonClick;
            customerSpawner.OnCustomerSpawnChanged += SetButtonColor;
        }

        private void OnDisable()
        {
            OnPressed -= HandleButtonClick;
            customerSpawner.OnCustomerSpawnChanged -= SetButtonColor;
        }

        private void HandleButtonClick()
        {
            _tutorialController.CompleteStep(stepId);
            
            customerSpawner.GenerateCustomer();
        }
    }
}