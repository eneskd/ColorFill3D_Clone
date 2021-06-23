using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;

    private Vector2Int _playerMovement = Vector2Int.zero;
    private bool _isMoving = false; 

    private PlayerInput _playerInput;
    private Rigidbody _rb;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        CheckInput(_playerInput.InputVector);
    }

    private void CheckInput(Vector2 input)
    {
        if(input.magnitude < 0.9) return;
        if(_playerMovement.magnitude > 0 && Vector3.Cross(input,(Vector2)_playerMovement).magnitude < 0.5) return;
        Move(input);
    }

    private void Move(Vector2 input)
    {
        _playerMovement = Math.Abs(_playerMovement.x) == 1 ? new Vector2Int(0, Math.Sign(input.y)) : new Vector2Int(Math.Sign(input.x), 0);
        _rb.velocity = _playerMovement.ToXZ() * moveSpeed;
    }
    
    
}