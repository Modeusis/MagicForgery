using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI
{
    [RequireComponent(typeof(Animator))]
    public class InventoryItem : MonoBehaviour
    {
        [SerializeField] private ItemData itemData;
        
        private Animator _animator;

        private bool _isSelected;

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected == value)
                    return;
                _isSelected = value;
                if (_isSelected)
                {
                    Select();
                }
                else
                {
                    Deselect();
                }
            }
        }
        
        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Select()
        {
            gameObject.SetActive(true);
        }

        private void Deselect()
        {
            gameObject.SetActive(false);
            _animator.SetBool("Selected", false);
        }

        private void OnEnable()
        {
            _animator.SetBool("Selected", true);
        }
    }
}