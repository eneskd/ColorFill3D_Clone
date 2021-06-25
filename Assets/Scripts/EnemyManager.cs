using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{



    [SerializeField] private GameObject enemyObject;
    [SerializeField] private Vector2Int enemyGridSize = new Vector2Int(5, 5);
    [SerializeField] private Vector2Int moveDirection = new Vector2Int(0, -1);
    [SerializeField] private float moveSpeed = 0.1f;
    [SerializeField] private float maxDistance = 4f;
    [SerializeField] private float yPos = 0;
    
    
    private bool _flipped = false;
    private float _movedDistance = 0;
    private Vector3 _direction;
    
    
    
    public static EnemyManager I;
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
        _direction = moveDirection.ToXZ();
    }

    private void Update()
    {
        Move();
    }


    public void FillGrid()
    {
        ClearGrid();

        for (var i = 0; i < enemyGridSize.x; i++)
        {
            for (var j = 0; j < enemyGridSize.y; j++)
            {
                var pos = transform.position + new Vector3(i, 0, j) * GridManager.cellSize;
                pos.y = yPos;
                var enemy = Instantiate(enemyObject, pos, Quaternion.identity);
                enemy.transform.localScale -= Vector3.one * GridManager.margin;
                enemy.transform.SetParent(this.gameObject.transform);
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

    }

    public void Snap()
    {
        transform.position = transform.position.Round();
    }

    private void Move()
    {
        // Not best way but fast
        
        var distance = moveSpeed * Time.deltaTime;
        transform.position += _direction * distance;
        _movedDistance += distance;

        // To prevent a bug
        if (_flipped && _movedDistance < 0)
        {
            Flip();
        }
        else if (!_flipped && _movedDistance > maxDistance)
        {
            Flip();
        }

    }

    private void Flip()
    {
        moveSpeed = -moveSpeed;
        _flipped = !_flipped;
    }
}
