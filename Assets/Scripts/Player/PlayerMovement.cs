using System;
using UnityEngine;

namespace Player
{ 
[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float gravity;
    [SerializeField] private float jumpStrength;
    
    
    [Header("Keys")]
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode runKey = KeyCode.LeftShift;
    
    
    
    private float _horizontal;
    private float _vertical;
    private Vector3 _movement;
    private Vector2 _inertia;
    
    private float _speed;
    private float _yVelocity;
    private CharacterController _controller;
    
    private SpeedState _speedState = SpeedState.Walking;
    private enum SpeedState
    {
        Walking,
        Running
    }
    private bool _charIsGrounded;
    void Awake()
    {
        _controller = GetComponent<CharacterController>();
        
    }

    private void Update()
    {
        if (Player.PlayerEnabled)
        {
            HandleRunning();
            HandleMovement();
            _charIsGrounded = _controller.isGrounded;
        }
        
    }

    void HandleMovement()
    {
        if (_speedState == SpeedState.Walking)
        {
            _speed = walkSpeed;
        }
        else if (_speedState == SpeedState.Running)
        {
            _speed = runSpeed;
        }
        
        _horizontal = Input.GetAxisRaw("Horizontal") * _speed;
        _vertical = Input.GetAxisRaw("Vertical") * _speed;
        
        _yVelocity = _movement.y;
        
        _movement = transform.right * _horizontal + transform.forward * _vertical;
        
        HandleJump();
        
        if (!_charIsGrounded)
        {
            if (_movement is { x: 0, z: 0 })
            {
                _movement.x = _inertia.x;
                _movement.z = _inertia.y;
            }
            _yVelocity -= gravity * Time.deltaTime;
        }
        _movement.y = _yVelocity;
        _controller.Move(_movement * Time.deltaTime);
    }

    void HandleJump()
    {
        if (_charIsGrounded && Input.GetKey(jumpKey))
        {
            _inertia.x = _movement.x; 
            _inertia.y = _movement.z; 
            _yVelocity = jumpStrength;
        }
    }

    void HandleRunning()
    {
        if (_charIsGrounded && Input.GetKey(runKey))
        {
            _speedState = SpeedState.Running;
        }
        else
        {
            _speedState = SpeedState.Walking;
        }
    }
}
}
