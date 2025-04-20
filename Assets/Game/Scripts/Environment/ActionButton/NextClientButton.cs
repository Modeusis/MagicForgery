using System;
using DG.Tweening;
using Game.Scripts.AI;
using Game.Scripts.Interface;
using Sounds;
using UI;
using UnityEngine;
using UnityEngine.Events;

namespace Environment
{
    [RequireComponent(typeof(BoxCollider))]
    public class NextClientButton : ActionButton
    {
        [SerializeField] private CustomerGenerator customerSpawner;
        
        private void OnEnable()
        {
            SetButtonColor(true);
            OnPressed += customerSpawner.GenerateCustomer;
            customerSpawner.OnCustomerSpawnChanged += SetButtonColor;
        }

        private void OnDisable()
        {
            OnPressed -= customerSpawner.GenerateCustomer;
            customerSpawner.OnCustomerSpawnChanged -= SetButtonColor;
        }
    }
}