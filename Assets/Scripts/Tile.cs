using System;
using Interfaces;
using UnityEngine;

public enum CellStatus
{
    Idle,
    Active,
    Safe,
    Empty
}

public class Tile : MonoBehaviour
{
    public Vector2Int coord;
    public bool isProcessed = false;

    private bool _hasPlayerLeft = false;

    public CellStatus Status
    {
        get => _cellStatus;
        set
        {
            switch (_cellStatus)
            {
                case CellStatus.Idle:
                    GridManager.I.idleTiles.Remove(this);
                    break;
                case CellStatus.Active:
                    GridManager.I.activeTiles.Remove(this);
                    break;
                case CellStatus.Safe:
                    GridManager.I.safeTiles.Remove(this);
                    break;
                case CellStatus.Empty:
                    GridManager.I.emptyTiles.Remove(this);
                    break;
            }

            _cellStatus = value;

            switch (_cellStatus)
            {
                case CellStatus.Idle:
                    GridManager.I.idleTiles.Add(this);
                    rend.material = GridManager.I.defaultMat;
                    break;
                case CellStatus.Active:
                    GridManager.I.activeTiles.Add(this);
                    rend.material = GridManager.I.activeMat;
                    break;
                case CellStatus.Safe:
                    GridManager.I.safeTiles.Add(this);
                    rend.material = GridManager.I.safeMat;
                    break;
                case CellStatus.Empty:
                    GridManager.I.safeTiles.Add(this);
                    break;
            }
        }
    }

    private CellStatus _cellStatus = CellStatus.Idle;

    private Renderer rend;

    private void Start()
    {
        GridManager.I.PlayerStop += OnPlayerStop;
        rend = GetComponent<Renderer>();

        if (!Physics.Raycast(transform.position, Vector3.up, out RaycastHit hit,
            GridManager.cellSize * 0.51f + GridManager.margin)) return;
        if (hit.transform.TryGetComponent(out Wall wall))
        {
            Status = CellStatus.Empty;
        }
    }

    public void ActivateTile()
    {
        if (Status == CellStatus.Idle)
        {
            Status = CellStatus.Active;
        }
    }


    private void OnTriggerStay(Collider other)
    {
        Enemy enemy;

        switch (Status)
        {
            case CellStatus.Active:
                if (other.TryGetComponent(out enemy))
                {
                    // Can reset level here
                }

                if (other.TryGetComponent(out Player player))
                {
                    if (_hasPlayerLeft)
                    {
                        player.Die();
                    }
                }

                break;
            case CellStatus.Safe:
                if (other.TryGetComponent(out enemy))
                {
                    enemy.Die();
                }

                break;
        }
    }


    private void OnPlayerStop()
    {
        if (Status == CellStatus.Active)
        {
            Status = CellStatus.Safe;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (Status == CellStatus.Active)
        {
            if (other.TryGetComponent(out Player player))
            {
                _hasPlayerLeft = true;
            }
        }
    }
}