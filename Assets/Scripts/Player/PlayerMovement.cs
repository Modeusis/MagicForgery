using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float gravity = 5f;
    
    [Header("Keys")]
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    
    private float _horizontal;
    private float _vertical;
    private Vector3 _movement;
    
    private CharacterController _controller;

    private bool _charEnabled = true;
    private bool _charIsGrounded;
    void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (_charEnabled)
        {
            HandleMovement();
            _charIsGrounded = _controller.isGrounded;
        }
        
    }

    void HandleMovement()
    {
        _horizontal = Input.GetAxisRaw("Horizontal") * speed;
        _vertical = Input.GetAxisRaw("Vertical") * speed;
        _movement = new Vector3(_horizontal, _charIsGrounded ? 0.0f : -gravity, _vertical);
        
        _controller.Move(_movement * Time.deltaTime);
    }

    void HandleJump()
    {
        if (_controller.isGrounded && Input.GetKey(jumpKey))
        {
            _movement.y = 1.0f;
        }
    }
}
