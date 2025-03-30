using System;
using Game.Scripts.Interface;
using UnityEngine;
using UnityEngine.Events;

namespace Environment
{
    [RequireComponent(typeof(BoxCollider))]
    public class NextClientButton : MonoBehaviour, IPressable
    {
        [SerializeField] private UnityEvent OnPressed;

        public void Press()
        {
            OnPressed?.Invoke();
        }
    }
}