using System;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Animator))]
    public class StaffScript : MonoBehaviour
    {
        [Header("Light")]
        [SerializeField] private Light magicFlashlight;
        [SerializeField] private GameObject magicCrystal;
        
        [Header("Key bindings")]
        [SerializeField] private KeyCode flashLight  = KeyCode.F;
        
        private bool _isFlashLightEnabled;
        private Animator _animator;
        
        
        
        void Start()
        {
            _animator = GetComponent<Animator>();
        }
        
        private void Update()
        {
            if (Player.IsPlayerEnabled)
            {
                HandleFlashLight();
            }
            magicCrystal.transform.Rotate(Vector3.forward * (Time.deltaTime * 100));
        }

        void HandleFlashLight()
        {
            if (Input.GetKeyDown(flashLight))
            {
                _isFlashLightEnabled = !_isFlashLightEnabled;
                magicFlashlight.enabled = _isFlashLightEnabled;
            }
        }

        void HandleAnimations()
        {
            
        }
    }
}

