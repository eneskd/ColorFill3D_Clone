using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerInput : MonoBehaviour
{
    public Vector2 InputVector => _inputVector;

    private Vector2 _inputVector = Vector2.zero;
    
    private void Update()
    {
        _inputVector.x = Input.GetAxisRaw("Horizontal");
        _inputVector.y = Input.GetAxisRaw("Vertical");
    }
    
}
