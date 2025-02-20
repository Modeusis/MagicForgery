using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UI;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    //Переделать под корутины
    [SerializeField] private GameObject door;
    [SerializeField] private TMP_Text tooltipText;
    
    [Header("Keycodes")]
    [SerializeField] private KeyCode enteractKey = KeyCode.E;
    
    private bool _isDoorInFocus;

    

    public bool IsDoorInFocus
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
            door.GetComponent<Animator>().SetBool("IsDoorOpened", _isDoorOpen);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        IsDoorInFocus = true;
        Debug.Log("Enter");
    }

    void OnTriggerStay(Collider other)
    {
        if (Player.Player.IsPlayerEnabled)
        {
            if (Input.GetKeyDown(enteractKey))
            {
                Debug.Log("DoorOpen");
                Debug.Log(IsDoorOpen);
                IsDoorOpen = !IsDoorOpen;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        IsDoorInFocus = false;
        Debug.Log("Exit");
    }
}
