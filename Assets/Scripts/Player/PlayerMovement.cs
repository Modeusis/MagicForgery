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
    
    private float _speed;
    private float _yVelocity;
    private CharacterController _controller;
    

    private bool _charIsGrounded;
    void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (Player.IsPlayerEnabled)
        {
            HandleMovement();
            _charIsGrounded = _controller.isGrounded;
        }
    }

    void HandleMovement()
    {
        _yVelocity = _movement.y;
        
        if (!_charIsGrounded)
        {
            float tempH = Input.GetAxisRaw("Horizontal");
            float tempV = Input.GetAxisRaw("Vertical");

            if (tempH != 0 || tempV != 0)
            {
                _horizontal = tempH;
                _vertical = tempV;
            }
            
            _yVelocity -= gravity * Time.deltaTime;
        }
        else
        {
            _horizontal = Input.GetAxisRaw("Horizontal");
            _vertical = Input.GetAxisRaw("Vertical");
            
            if (_horizontal == 0 && _vertical == 0)
            {
                Player.State = Player.PlayerState.Standing;
            }
            else
            {
                HandleRunning();
            }
            
            if (Player.State == Player.PlayerState.Walking)
            {
                _speed = walkSpeed;
            }
            else if (Player.State == Player.PlayerState.Running)
            {
                _speed = runSpeed;
            }

        }
        HandleJump();
        
        _movement = transform.right * (_horizontal * _speed) + transform.forward * (_vertical  * _speed);
        _movement.y = _yVelocity;
        _controller.Move(_movement * Time.deltaTime);
    }

    void HandleJump()
    {
        if (_charIsGrounded && Input.GetKey(jumpKey))
        {
            Player.State = Player.PlayerState.Jumping; 
            _yVelocity = jumpStrength;
        }
    }

    void HandleRunning()
    {
        if (_charIsGrounded && Input.GetKey(runKey))
        {
            Player.State = Player.PlayerState.Running;
        }
        else
        {
            Player.State = Player.PlayerState.Walking;
        }
    }
}
}
