using System;
using DG.Tweening;
using Game.Scripts.Interface;
using Game.Scripts.Utilities;
using UI;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Environment.Hydromat
{
    public class BucketGeneratorScript : MonoBehaviour, IToggle
    {
        [Inject] private EventBus _eventBus;
        
        [Header("Spawner")]
        [SerializeField] private ItemData bucketData;
        [SerializeField] private Transform spawnTransform;
        
        [Header("Animation")]
        [SerializeField] private Animator hydromatAnimator;
        [SerializeField] private string boolParameterName = "isHydromatOpen";

        [Header("View")]
        [SerializeField] private Material flowMaterial;
        [SerializeField] private ParticleSystem hydromatParticles;
        
        private GameObject _bucketInstance;
        
        public bool IsToggled { get; set; }

        private bool _isFocused;
        public bool IsFocused
        {
            get => _isFocused;
            set
            {
                if (_isFocused == value)
                    return;
                
                _isFocused = value;

                if (_bucketInstance != null)
                {
                    _bucketInstance.layer = _isFocused ? LayerMask.NameToLayer("Interactable") : LayerMask.NameToLayer("Default");
                }
            }
        }
        
        private HydromatState HydromatState { get; set; } = HydromatState.Open;
        
        public event Action<bool> IsSpawnAvailable;

        private void OnEnable()
        {
            IsSpawnAvailable += ChangeHydromatState;
        }
        
        private void OnDisable()
        {
            IsSpawnAvailable -= ChangeHydromatState;
        }
        
        public void Toggle()
        {
            TakeBucket();
        }
        
        public void SpawnBucket()
        {
            if (HydromatState == HydromatState.Closed)
            {
                return;
            }
            
            _bucketInstance = Instantiate(bucketData.prefab, spawnTransform.position, Quaternion.identity);

            if (_bucketInstance.TryGetComponent(out Collider bucketCollider))
            {
                Destroy(bucketCollider);
            }
            
            _bucketInstance.transform.SetParent(transform);
            _bucketInstance.transform.localPosition = spawnTransform.localPosition;
            
            hydromatAnimator?.SetBool(boolParameterName, true);
            flowMaterial?.DOFloat(1f, "_FlowPower", 1f);
            hydromatParticles?.Play();
            
            IsSpawnAvailable?.Invoke(false);
        }

        private void TakeBucket()
        {
            if (HydromatState == HydromatState.Open) 
            {
                return;
            }

            if (!Inventory.instance.AddItem(bucketData))
            {
                return;
            }
            
            Destroy(_bucketInstance);

            flowMaterial?.DOFloat(0.2f, "_FlowPower", 1f);
            hydromatAnimator?.SetBool(boolParameterName, false);
            
            IsSpawnAvailable?.Invoke(true);
        }

        private void ChangeHydromatState(bool state)
        {
            HydromatState = state ? HydromatState.Open : HydromatState.Closed;
        }
    }
}