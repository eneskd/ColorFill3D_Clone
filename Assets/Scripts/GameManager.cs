using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class GameManager : MonoBehaviour
{
    public Player Player;

    public static GameManager I;

    // This is just to move between scenes for the demo
    // Not the actual way I would do it

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

        Player.PlayerDeath += RestartLevel;
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex == 2) 
        {
            SceneManager.LoadScene(0); // Return to first scene for now
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
