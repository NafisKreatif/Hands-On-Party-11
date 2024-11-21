using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    void Start() {
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

            musicSlider.onValueChanged.AddListener(delegate { SetMusicVolume(); });
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
            gyroToggle.onValueChanged.AddListener(delegate { ToggleGyroscope(gyroToggle.isOn); });
        }
    }
    // Bagian Audio
    [SerializeField] private AudioMixer myMixer; // Mixer audio untuk mengatur volume
    [SerializeField] private Slider musicSlider; // Slider untuk mengatur volume musik

    // Fungsi untuk mengatur volume musik berdasarkan slider
    public void SetMusicVolume()
    {
        if (musicSlider != null && myMixer != null)
        {
            SettingsManager.Instance.MusicVolume = musicSlider.value;
            myMixer.SetFloat("music", Mathf.Log10(musicSlider.value) * 20);
            Debug.Log($"PauseMenu: Music volume updated to {SettingsManager.Instance.MusicVolume}");
        }
    }

    // Bagian Gyro
    [SerializeField] private Toggle gyroToggle; // Toggle untuk mengatur gyroscope
    public void ToggleGyroscope(bool isOn)
    {
        SettingsManager.Instance.IsGyroEnabled = isOn;
        Debug.Log($"PauseMenu: Gyroscope enabled: {isOn}");
    }
    //Bagian Singleton
    [SerializeField] private GameObject settingsManagerPrefab;
}
