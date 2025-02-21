using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Environment
{
    public class DoorTrigger : MonoBehaviour
    {
        [SerializeField] private Animator door;
        [SerializeField] private TMP_Text tooltipText;
        [SerializeField] private int layerNumber = 6;
        [SerializeField] private float raycastLenght = 6;
    
        [Header("Keycodes")]
        [SerializeField] private KeyCode enteractKey = KeyCode.E;
    
        private Animator _animator;
        private bool _isTriggered;
        private bool _isDoorInFocus;

        private bool IsDoorInFocus
        {
            get => _isDoorInFocus;
            set
            {
                if (_isDoorInFocus == value)
                    return;
                _isDoorInFocus = value;
                tooltipText.text = $"Press {enteractKey.ToString()} to {(_isDoorOpen ? "close" : "open")}";
                tooltipText.DOFade(value ? 1 : 0, 1f);
            }
        }

        private bool _isDoorOpen;

        private bool IsDoorOpen
        {
            get => _isDoorOpen;
            set
            {
                if (value == _isDoorOpen)
                    return;
                _isDoorOpen = value;
                door.SetBool("IsDoorOpened", _isDoorOpen);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _isTriggered = true;
            }
        }

        private void Update()
        {
            if (Player.Player.IsPlayerEnabled && _isTriggered)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
                if (Physics.Raycast(ray,out var hit, raycastLenght, 1 << layerNumber ))
                {
                    if (hit.collider.CompareTag("Door"))
                    {
                        IsDoorInFocus = true;
                    }
                    if (Input.GetKeyDown(enteractKey) && IsDoorInFocus)
                    {
                        IsDoorOpen = !IsDoorOpen;
                    } 
                }
                else
                {
                    IsDoorInFocus = false;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            _isTriggered = false;
            IsDoorInFocus = false;
        }
    }
}
