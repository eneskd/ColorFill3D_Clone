using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public void Die()
    {
        Destroy(gameObject);
    }

    public void OnCollisionEnter(Collision other)
    {
        if (other.transform.TryGetComponent(out Player player))
        {
            player.Die();
        }
    }
}
