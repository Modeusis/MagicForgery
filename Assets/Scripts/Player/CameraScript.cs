using System;
using UnityEngine;

namespace Player
{
    public class CameraScript : MonoBehaviour
    {
        [Header("Camera Settings")]
        [SerializeField] private float sensX;
        [SerializeField] private float sensY;

        private float _mouseX;
        private float _mouseY;
        
        private float _cameraVertical;
        private float _cameraHorizontal;
        
        [Header("Player Settings")]
        [SerializeField] private Transform playerTransform;

        private void Awake()
        {
            _cameraHorizontal = playerTransform.rotation.x;
            _cameraVertical = transform.rotation.y;
            
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            if (Player.PlayerEnabled)
            {
                HandleCameraMovement();
            }
            
        }

        void HandleCameraMovement()
        {
            _mouseX = Input.GetAxisRaw("Mouse X") * sensX;
            _mouseY = Input.GetAxisRaw("Mouse Y") * sensY;
            
            _cameraHorizontal += _mouseX * Time.deltaTime;
            
            _cameraVertical -= _mouseY * Time.deltaTime;
            _cameraVertical = Mathf.Clamp(_cameraVertical, -90f, 90f);
            
            playerTransform.rotation = Quaternion.Euler(0, _cameraHorizontal, 0);
            transform.localRotation = Quaternion.Euler(_cameraVertical, 0, 0);
        }
    }
}

