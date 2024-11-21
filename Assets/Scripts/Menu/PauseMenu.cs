using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public TransitionController transitionController;
    // Bagian Pause Menu
    // Variabel untuk melacak apakah permainan sedang pause
    public static bool paused = false;
    // Objek Canvas untuk menampilkan menu pause
    public GameObject PauseMenuCanvas;

    // Fungsi yang dipanggil sekali saat game dimulai
    void Start()
    {
        Time.timeScale = 1f; // Mengatur waktu menjadi normal saat game dimulai
    }

    // Fungsi yang dipanggil setiap frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // Mengecek apakah tombol Escape ditekan
        {
            if (paused)
            {
                Play(); // Melanjutkan permainan jika sedang pause
            }
            else
            {
                Stop(); // Mem-pause permainan jika tidak sedang pause
            }
        }
    }

    // Fungsi untuk mem-pause permainan
    public void Stop()
    {
        PauseMenuCanvas.SetActive(true); // Menampilkan Canvas menu pause
        Time.timeScale = 0f; // Menghentikan waktu dalam game
        paused = true; // Menandai bahwa permainan sedang pause
    }

    public void Play()
    {
        PauseMenuCanvas.SetActive(false); // Menyembunyikan Canvas menu pause
        Time.timeScale = 1f; // Melanjutkan waktu dalam game
        paused = false; // Menandai bahwa permainan tidak lagi pause
        // Debug.Log("Game resumed"); // Log untuk debugging
    }

    // Fungsi untuk berpindah ke menu utama
    public void MainMenuButton()
    {
        Time.timeScale = 1f; // Supaya transisinya bisa jalan
        transitionController.GoToSceneByIndex(0);
    }
}
