using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    // Bagian Pause Menu
    public static bool paused = false; // Melacak apakah permainan sedang pause
    public GameObject PauseMenuCanvas; // Objek Canvas untuk menampilkan menu pause

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

            // Tambahkan listener untuk sinkronisasi perubahan
            musicSlider.onValueChanged.AddListener((value) => UpdateMusicVolume(value));
        }

        // Sinkronkan toggle gyroscope
        if (gyroToggle != null)
        {
            gyroToggle.isOn = SettingsManager.Instance.IsGyroEnabled;

            // Tambahkan listener untuk sinkronisasi perubahan
            gyroToggle.onValueChanged.AddListener((value) => UpdateGyroState(value));
        }

        // Daftarkan listener untuk UnityEvent di SettingsManager
        SettingsManager.Instance.OnFloatPropertyChanged.AddListener(OnFloatPropertyChanged);
        SettingsManager.Instance.OnBoolPropertyChanged.AddListener(OnBoolPropertyChanged);
    }

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
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1); // Memuat scene sebelumnya (main menu)
    }

    // Bagian Audio
    [SerializeField] private AudioMixer myMixer; // Mixer audio untuk mengatur volume
    [SerializeField] private Slider musicSlider; // Slider volume musik

    public void SetMusicVolume()
    {
        if (musicSlider != null)
        {
            UpdateMusicVolume(musicSlider.value); // Gunakan metode UpdateMusicVolume
        }
    }

    private void UpdateMusicVolume(float value)
    {
        SettingsManager.Instance.UpdateMusicVolume(value); // Gunakan metode di SettingsManager
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
        SettingsManager.Instance.UpdateGyroState(value); // Gunakan metode di SettingsManager
        Debug.Log($"PauseMenu: Gyroscope state updated to {value}");
    }

    // Listener untuk UnityEvent (float)
    private void OnFloatPropertyChanged(string propertyName, float value)
    {
        if (propertyName == "MusicVolume" && musicSlider != null)
        {
            musicSlider.value = value;
            myMixer.SetFloat("music", Mathf.Log10(value) * 20);
            Debug.Log($"MusicVolume synced to slider: {value}");
        }
    }

    // Listener untuk UnityEvent (bool)
    private void OnBoolPropertyChanged(string propertyName, bool value)
    {
        if (propertyName == "IsGyroEnabled" && gyroToggle != null)
        {
            gyroToggle.isOn = value;
            Debug.Log($"IsGyroEnabled synced to toggle: {value}");
        }
    }

    // Bagian Singleton
    [SerializeField] private GameObject settingsManagerPrefab;
}
