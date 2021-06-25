using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocationTracker : MonoBehaviour
{
    public Vector2Int playerCoord;
    [SerializeField] private float turnThreshold = 0.1f;
    
    private Vector2Int _prevCoord;
    private PlayerMovement _playerMovement;
    private float _cellSize;
    
    
    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
    }

    private void Start()
    {
        _cellSize = GridManager.cellSize;
    }

    private void FixedUpdate()
    {
        var pos = transform.position - GridManager.I.transform.position;
        var currentCord = new Vector2Int((int) Math.Round(pos.x / _cellSize), (int) Math.Round(pos.z / _cellSize));

        if (!playerCoord.Equals(currentCord))
        {
            _prevCoord = playerCoord;
            playerCoord = currentCord;
            GridManager.I.Grid[_prevCoord.x, _prevCoord.y].ActivateTile();
            _playerMovement.overrideTurn = false;
        }

        if (playerCoord.x * _cellSize - pos.x < turnThreshold && playerCoord.y * _cellSize - pos.z < turnThreshold && !_playerMovement.overrideTurn)
        {
            _playerMovement.CanTurn = true;
        }
        else
        {
            _playerMovement.CanTurn = false;
        }
        
        

    }
}
