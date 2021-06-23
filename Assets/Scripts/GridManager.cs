using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GridManager : MonoBehaviour
{
    [SerializeField] private Vector2Int gridSize = new Vector2Int(10, 10);
    [SerializeField] private GameObject gridObject;
    [SerializeField] private float margin = 0.2f;
    [SerializeField] private float cellLength = 1f;
    [SerializeField] private float yPos = -1;
    private float ObjectSize => cellLength - margin;

    private List<GameObject> _objects = new List<GameObject>();

    public static GridManager I { get; private set; }

    private void Awake()
    {
        if (I != null && I != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            I = this;
        }

        print(ObjectSize);
    }


    public void FillGrid()
    {
        ClearGrid();

        for (var i = 0; i < gridSize.x; i++)
        {
            for (var j = 0; j < gridSize.y; j++)
            {
                var pos = transform.position + new Vector3(i, yPos, j) * cellLength +
                          new Vector3(0.5f, yPos, 0.5f) * cellLength;
                var gridObj = Instantiate(gridObject, pos, Quaternion.identity);
                gridObj.transform.localScale -= Vector3.one * margin;
                gridObj.transform.SetParent(this.gameObject.transform);
                _objects.Add(gridObj);
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

        _objects.Clear();
    }

    public void Snap()
    {
        transform.position = transform.position.Round();
    }
}