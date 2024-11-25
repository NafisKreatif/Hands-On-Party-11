using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    //Bagian Audio
    [SerializeField] private AudioMixer myMixer; // Mixer audio untuk mengatur volume musik
    [SerializeField] private Slider musicSlider; // Slider untuk mengatur volume musik
    [SerializeField] private Toggle gyroToggle; // Toggle untuk mengatur penggunaan gyro
    [SerializeField] private GravityRotationController gravityRotationController;
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
            float volume = PlayerPrefs.GetFloat("Master Volume");
            musicSlider.value = volume;
            myMixer.SetFloat("music", Mathf.Log10(volume) * 20);
            musicSlider.onValueChanged.AddListener(delegate {SetMusicVolume();});
            Debug.Log($"PauseMenu initialized with volume: {volume}");
        }
        else
        {
            Debug.LogError("MusicSlider or AudioMixer is not assigned in the Inspector!");
        }
        // Sinkronkan slider gyro
        gravityRotationController = Camera.main.GetComponent<GravityRotationController>();
        if (gravityRotationController != null && gyroToggle != null) {
            gyroToggle.onValueChanged.AddListener(delegate {SetGyro();});
        }
    }
    public void SetMusicVolume()
    {
        float volume = musicSlider.value; // Mengambil nilai dari slider
        myMixer.SetFloat("music", Mathf.Log10(volume) * 20); // Mengubah nilai slider menjadi decibel dan mengatur volume musik
    }
    public void SetGyro()
    {
        bool useGyro = PlayerPrefs.GetInt("Use Gyro") == 1; // Mengambil nilai dari slider
        gravityRotationController.useGyro = useGyro; // Mengubah nilai useGyro sesuai dengan pengaturan
    }

    //Bagian Singleton
    [SerializeField] private GameObject settingsManagerPrefab;
}