using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class mainmenu : MonoBehaviour
{

    // Bagian Main menu
    public void play() // Fungsi mengatur tombol play
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); //Lanjut ke scene selanjutnya setelah mencet play
    }
    public void quit() //Fungsi untuk mengatur quit game
    {
        Application.Quit();
        Debug.Log("Player Has quit the game");
    }

    void Start()
    {
        if (SettingsManager.Instance == null)
        {
            Debug.Log("SettingsManager not found. Instantiating from prefab.");
            Instantiate(settingsManagerPrefab); // Buat SettingsManager dari prefab
        }

        // Sinkronisasi slider dengan SettingsManager
        if (musicSlider != null)
        {
            musicSlider.value = SettingsManager.Instance.MusicVolume;
            myMixer.SetFloat("music", Mathf.Log10(SettingsManager.Instance.MusicVolume) * 20);
            musicSlider.onValueChanged.AddListener((value) => UpdateMusicVolume(value));
        }

        // Sinkronisasi toggle gyroscope dengan SettingsManager
        if (gyroToggle != null)
        {
            gyroToggle.isOn = SettingsManager.Instance.IsGyroEnabled;
            gyroToggle.onValueChanged.AddListener((value) => UpdateGyroState(value));
        }
    }

    // Bagian Audio
    [SerializeField] private AudioMixer myMixer; // Audio Mixer untuk volume
    [SerializeField] private Slider musicSlider; // Slider volume musik
    public void SetMusicVolume()
    {
        if (musicSlider != null)
        {
            UpdateMusicVolume(musicSlider.value); // Gunakan metode UpdateMusicVolume untuk menyimpan perubahan
        }
    }

    private void UpdateMusicVolume(float value)
    {
        SettingsManager.Instance.UpdateMusicVolume(value); // Gunakan metode di SettingsManager
        myMixer.SetFloat("music", Mathf.Log10(value) * 20);
        Debug.Log($"Music volume updated to: {value}");
    }

    // Bagian Gyro
    [SerializeField] private Toggle gyroToggle; // Tambahkan toggle gyroscope di Main Menu
    public void ToggleGyroscope(bool isOn)
    {
        UpdateGyroState(isOn); // Gunakan metode UpdateGyroState untuk menyimpan perubahan
    }

    private void UpdateGyroState(bool value)
    {
        SettingsManager.Instance.UpdateGyroState(value); // Gunakan metode di SettingsManager
        Debug.Log($"Gyroscope state updated to: {value}");
    }

    // Bagian singleton
    [SerializeField] private GameObject settingsManagerPrefab;
}
