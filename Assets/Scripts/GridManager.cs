using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GridManager : MonoBehaviour
{
    public delegate void PlayerStopHandler();
    public event PlayerStopHandler PlayerStop;
    
    public static float cellSize = 1f;
    public static float margin = 0.05f;

    public Tile[,] Grid;
    public List<Tile> activeTiles = new List<Tile>();
    public List<Tile> safeTiles = new List<Tile>();
    public List<Tile> idleTiles = new List<Tile>();
    public List<Tile> emptyTiles = new List<Tile>();
    public Material defaultMat;
    public Material activeMat;
    public Material safeMat;

        
    public Vector2Int gridSize = new Vector2Int(10, 10);
    
    [SerializeField] private GameObject gridObject;
    [SerializeField] private float yPos = -1;
    
    
    public static GridManager I { get; private set; }

    private void Awake()
    {
        if (I != null && I != this)
        {
            Destroy(gameObject);
        }
        else
        {
            I = this;
        }
        FillGrid();
    }


    public void FillGrid()
    {
        ClearGrid();
        Grid = new Tile[gridSize.x, gridSize.y];

        for (var i = 0; i < gridSize.x; i++)
        {
            for (var j = 0; j < gridSize.y; j++)
            {
                var pos = transform.position + new Vector3(i, 0, j) * cellSize;
                pos.y = yPos;
                var gridObj = Instantiate(gridObject, pos, Quaternion.identity);
                gridObj.transform.localScale -= Vector3.one * margin;
                gridObj.transform.SetParent(this.gameObject.transform);
                var tile = gridObj.GetComponent<Tile>();
                tile.coord = new Vector2Int(i, j);
                Grid[i,j] = tile;
                idleTiles.Add(tile);
            }
        }
    }

    public void ClearGrid()
    {
        List<GameObject> children = new List<GameObject>();

        foreach (Transform child in transform)
        {
            children.Add(child.gameObject);
        }

        for (int i = 0; i < children.Count; i++)
        {
            DestroyImmediate(children[i]);
        }

        Grid = null;
        idleTiles.Clear();
        activeTiles.Clear();
        safeTiles.Clear();
    }

    public void Snap()
    {
        transform.position = transform.position.Round();
    }

    public virtual void OnPlayerStop()
    {
        PlayerStop?.Invoke();
    }
}