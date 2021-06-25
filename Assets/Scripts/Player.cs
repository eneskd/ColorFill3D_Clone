using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public delegate void PlayerDeathHandler();

    public event PlayerDeathHandler PlayerDeath;

    
    private PlayerLocationTracker _locationTracker;

    private void Awake()
    {
        _locationTracker = GetComponent<PlayerLocationTracker>();
    }

    public void Die()
    {
        var coord = _locationTracker.playerCoord;
        if (GridManager.I.Grid[coord.x, coord.y].Status == CellStatus.Safe) return;
        Destroy(gameObject);
        OnPlayerDeath();
    }

    protected virtual void OnPlayerDeath()
    {
        PlayerDeath?.Invoke();
    }
}