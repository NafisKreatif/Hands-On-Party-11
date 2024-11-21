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
    
        // Periksa apakah SettingsManager sudah ada di scene
        if (SettingsManager.Instance == null)
        {
            Debug.Log("SettingsManager not found. Instantiating from prefab.");
            Instantiate(settingsManagerPrefab); // Buat SettingsManager dari prefab
        }

        // Sinkronkan slider volume
        if (musicSlider != null && myMixer != null)
        {
            musicSlider.value = SettingsManager.Instance.MusicVolume;
            myMixer.SetFloat("music", Mathf.Log10(SettingsManager.Instance.MusicVolume) * 20);

            musicSlider.onValueChanged.AddListener((value) => UpdateMusicVolume(value));
            Debug.Log($"PauseMenu initialized with volume: {SettingsManager.Instance.MusicVolume}");
        }
        else
        {
            Debug.LogError("MusicSlider or AudioMixer is not assigned in the Inspector!");
        }

        // Sinkronkan toggle gyroscope
        if (gyroToggle != null)
        {
            gyroToggle.isOn = SettingsManager.Instance.IsGyroEnabled;
            gyroToggle.onValueChanged.AddListener((value) => UpdateGyroState(value));
        }
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

    // Bagian Audio
    [SerializeField] private AudioMixer myMixer; // Mixer audio untuk mengatur volume
    [SerializeField] private Slider musicSlider; // Slider untuk mengatur volume musik

    // Fungsi untuk memperbarui volume musik
    public void SetMusicVolume()
    {
        if (musicSlider != null && myMixer != null)
        {
            UpdateMusicVolume(musicSlider.value); // Gunakan metode UpdateMusicVolume
        }
    }

    private void UpdateMusicVolume(float value)
    {
        SettingsManager.Instance.UpdateMusicVolume(value); // Panggil metode di SettingsManager
        myMixer.SetFloat("music", Mathf.Log10(value) * 20);
        Debug.Log($"PauseMenu: Music volume updated to {value}");
    }

    // Bagian Gyro
    [SerializeField] private Toggle gyroToggle; // Toggle untuk mengatur gyroscope
    public void ToggleGyroscope(bool isOn)
    {
        UpdateGyroState(isOn); // Gunakan metode UpdateGyroState
    }

    private void UpdateGyroState(bool value)
    {
        SettingsManager.Instance.UpdateGyroState(value); // Panggil metode di SettingsManager
        Debug.Log($"PauseMenu: Gyroscope state updated to {value}");
    }

    // Bagian Singleton
    [SerializeField] private GameObject settingsManagerPrefab;
}
