using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    // Bagian Pause Menu
    public static bool paused = false;
    public Canvas PauseMenuCanvas;

    void Start()
    {
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused) Resume();
            else Stop();
        }
    }

    public void Stop()
    {
        PauseMenuCanvas.enabled = true;
        Time.timeScale = 0f;
        paused = true;
    }

    public void Resume()
    {
        PauseMenuCanvas.enabled = false;
        Time.timeScale = 1f;
        paused = false;
    }

    public void ReturnToMainMenu() { SceneManager.LoadScene(0); }
}
