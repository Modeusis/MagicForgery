using DG.Tweening;
using TMPro;
using UI;
using UnityEngine;

namespace Environment
{
    public class DoorTrigger : MonoBehaviour
    {
        [SerializeField] private Animator door;
        [SerializeField] private TMP_Text tooltipText;
        [SerializeField] private float raycastLenght = 6;
    
        [Header("Sounds")]
        [SerializeField] private AudioClip doorOpenSound;
        [SerializeField] private AudioClip doorCloseSound;
        // [SerializeField] private AudioSource doorSound;
        
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
                tooltipText.text = $"{enteractKey.ToString()} to {(_isDoorOpen ? "close" : "open")}";
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
                SoundManager.instance.PlaySfx(_isDoorOpen ? doorOpenSound : doorCloseSound);
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
            if (Player.Player.instance.IsPlayerEnabled && _isTriggered)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
                if (Physics.Raycast(ray,out var hit, raycastLenght ))
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
            if (other.CompareTag("Player"))
            {
                _isTriggered = false;
                IsDoorInFocus = false;
            }
        }
    }
}
