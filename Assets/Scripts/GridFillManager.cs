using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridFillManager : MonoBehaviour
{
    private GridManager _gridManager;
    private List<List<Tile>> _regions = new List<List<Tile>>();
    private List<Tile> _currentRegion = new List<Tile>();


    private void Start()
    {
        _gridManager = GetComponent<GridManager>();
        _gridManager.PlayerStop += OnPlayerStop;
    }

    private void OnPlayerStop()
    {
        FindRegions(_gridManager.idleTiles.ToList());
        FillSmallest();
        Clear();
        if (_gridManager.safeTiles.Count == _gridManager.Grid.Length - _gridManager.emptyTiles.Count)
        {
            GameManager.I.NextLevel();
        }
    }


    private void FillSmallest()
    {
        var size = Int32.MaxValue;
        ;
        var smallestRegion = new List<Tile>();
        foreach (var region in _regions)
        {
            if (region.Count < size)
            {
                size = region.Count;
                smallestRegion = region;
            }
        }

        // to prevent instant win. Not really sure about actual algorithm of the game
        if (_regions.Count == 1 && smallestRegion.Count > _gridManager.Grid.Length / 2) return;
        
        foreach (var tile in smallestRegion)
        {
            tile.Status = CellStatus.Safe;
        }
    }

    private void FindRegions(List<Tile> tiles)
    {
        if (tiles.Count == 0) return;
        var tile = tiles[0];

        _currentRegion.Clear();
        FloodFill(tile.coord.x, tile.coord.y);
        var list = _currentRegion.ToList();
        _regions.Add(list);
        foreach (var t in list)
        {
            tiles.Remove(t);
        }

        FindRegions(tiles);
    }

    private void Clear()
    {
        _regions.Clear();
        _currentRegion.Clear();
        foreach (var tiles in _gridManager.idleTiles)
        {
            tiles.isProcessed = false;
        }
    }

    private void FloodFill(int x, int y)
    {
        if ((x < 0) || (x >= _gridManager.gridSize.x)) return;
        if ((y < 0) || (y >= _gridManager.gridSize.y)) return;

        var tile = _gridManager.Grid[x, y];
        if (tile.isProcessed) return;
        if (tile.Status == CellStatus.Idle)
        {
            tile.isProcessed = true;
            _currentRegion.Add(tile);
            FloodFill(x + 1, y);
            FloodFill(x, y + 1);
            FloodFill(x - 1, y);
            FloodFill(x, y - 1);
        }
    }
}