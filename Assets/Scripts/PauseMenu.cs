using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool paused = false; 
    public GameObject PauseMenuCanvas;

    
    void Start()
    {
        Time.timeScale = 1f; 
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            if (paused)
            {
                Play(); 
            }
            else
            {
                Stop(); 
            }
        }
    }

    public void Stop()
    {
        PauseMenuCanvas.SetActive(true); 
        Time.timeScale = 0f; 
        paused = true;
    }

    public void Play() 
    {
        PauseMenuCanvas.SetActive(false); 
        Time.timeScale = 1f; 
        paused = false;
        Debug.Log("PPPPPPPPPPPPP");
    }

    public void MainMenuButton()
    {
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}

