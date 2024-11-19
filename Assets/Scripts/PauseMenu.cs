using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
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
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1); // Memuat scene sebelumnya (main menu)
    }

    //Bagian Audio
    [SerializeField] private AudioMixer myMixer; // Mixer audio untuk mengatur volume musik
    [SerializeField] private Slider musicSlider; // Slider untuk mengatur volume musik

    // Fungsi untuk mengatur volume musik berdasarkan slider
    public void SetMusicVloume()
    {
        float volume = musicSlider.value; // Mengambil nilai dari slider
        myMixer.SetFloat("music", Mathf.Log10(volume)*20); // Mengubah nilai slider menjadi decibel dan mengatur volume musik
    }

    //Bagian Gyroscope
    private Gyroscope gyro; // Objek untuk mengakses gyroscope
    private bool isGyroEnabled = false; // Menandai apakah gyroscope sedang aktif

    // Fungsi untuk mengaktifkan atau menonaktifkan gyroscope menggunakan toggle
    public void ToggleGyroscope(bool isOn)
{
    if (gyro == null) // Mengecek apakah perangkat mendukung gyroscope
    {
        Debug.LogWarning("Gyroscope is not supported, cannot toggle.");

        // Gyroscope tidak didukung, tapi tidak ada elemen status untuk diperbarui
        return;
    }

    if (isOn)
    {
        EnableGyroscope(); // Mengaktifkan gyroscope
    }
    else
    {
        DisableGyroscope(); // Menonaktifkan gyroscope
    }
}

// Fungsi untuk mengaktifkan gyroscope
private void EnableGyroscope()
{
    if (!isGyroEnabled) // Mengecek apakah gyroscope belum aktif
    {
        gyro.enabled = true; // Mengaktifkan gyroscope
        isGyroEnabled = true; // Menandai bahwa gyroscope aktif
        // Tidak ada teks untuk diperbarui
    }
}

// Fungsi untuk menonaktifkan gyroscope
private void DisableGyroscope()
{
    if (isGyroEnabled) // Mengecek apakah gyroscope sedang aktif
    {
        gyro.enabled = false; // Menonaktifkan gyroscope
        isGyroEnabled = false; // Menandai bahwa gyroscope tidak aktif
        // Tidak ada teks untuk diperbarui
    }
}

}

