using System;
using System.Collections;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;


public class PlayerMovement : MonoBehaviour
{
    public bool CanTurn
    {
        get => _canTurn;
        set
        {
            _canTurn = value;
            if (_playerDirection.Equals(_turnDirection)) return;
            if (!_canTurn) return;
            if (!_isMoving) return;
            Turn();
        }
    }

    public bool overrideTurn = false;

    [SerializeField] private float moveSpeed = 2f;


    private Vector2Int _playerDirection = Vector2Int.zero;
    private Vector2Int _turnDirection = Vector2Int.zero;

    private bool _isMoving = false;
    private bool _canTurn = false;

    private PlayerInput _playerInput;
    private Rigidbody _rb;
    private PlayerLocationTracker _locationTracker;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _rb = GetComponent<Rigidbody>();
        _locationTracker = GetComponent<PlayerLocationTracker>();
    }

    private void Update()
    {
        CheckInput(_playerInput.InputVector);
    }

    private void FixedUpdate()
    {
        CheckFront();
    }

    private void CheckInput(Vector2 input)
    {
        if (input.magnitude < 0.9f) return;
        if (_isMoving && Vector3.Cross(input, (Vector2) _playerDirection).magnitude < 0.5) return; // check the angle 
        UpdateTurnDirection(input);
    }

    private void UpdateTurnDirection(Vector2 input)
    {
        if (!_isMoving)
        {
            _turnDirection = Mathf.Abs(input.y) > 0.8f
                ? new Vector2Int(0, Math.Sign(input.y))
                : new Vector2Int(Math.Sign(input.x), 0);
            Turn();
        }
        else
        {
            _turnDirection = Math.Abs(_playerDirection.x) == 1
                ? new Vector2Int(0, Math.Sign(input.y))
                : new Vector2Int(Math.Sign(input.x), 0);
        }
    }


    private void Turn()
    {
        Snap();
        _playerDirection = _turnDirection;
        _canTurn = false;
        overrideTurn = true;
        Move();
    }

    private void Move()
    {
        _rb.velocity = _playerDirection.ToXZ() * (moveSpeed * GridManager.cellSize);
        _isMoving = true;
    }

    private void Stop()
    {
        _isMoving = false;
        Snap();
        _rb.velocity = Vector3.zero;
        _playerDirection = Vector2Int.zero;
        var coord = _locationTracker.playerCoord;
        GridManager.I.Grid[coord.x, coord.y].ActivateTile();
        GridManager.I.OnPlayerStop();
    }

    private void Snap()
    {
        var gridManager = GridManager.I;
        var x = _locationTracker.playerCoord.x * GridManager.cellSize + gridManager.transform.position.x;
        var z = _locationTracker.playerCoord.y * GridManager.cellSize + gridManager.transform.position.z;

        transform.position = new Vector3(x, transform.position.y, z);
    }

    private void CheckFront()
    {
        // Check for Walls
        if (!_isMoving) return;
        if (!Physics.Raycast(transform.position, _playerDirection.ToXZ(), out RaycastHit hit,
            GridManager.cellSize * 0.51f + GridManager.margin)) return;
        if (!hit.transform.TryGetComponent(out IBlocker wall)) return;
        if (!_isMoving) return;

        Stop();
    }
}