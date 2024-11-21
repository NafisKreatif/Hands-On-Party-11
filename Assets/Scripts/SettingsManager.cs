using UnityEngine;
using UnityEngine.Events;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance { get; private set; }

    // UnityEvents untuk abstraksi setiap tipe variabel
    public UnityEvent<string, float> OnFloatPropertyChanged;
    public UnityEvent<string, bool> OnBoolPropertyChanged;

    // Properti Volume Musik
    [field: SerializeField] // Tampil di Inspector dan tetap private untuk akses
    public float MusicVolume { get; private set; } = 1.0f;

    // Properti Gyroscope
    [field: SerializeField]
    public bool IsGyroEnabled { get; private set; } = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Hapus duplikat
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Tetap hidup di semua scene
        Debug.Log("SettingsManager initialized.");

        // Inisialisasi UnityEvent jika null
        OnFloatPropertyChanged ??= new UnityEvent<string, float>();
        OnBoolPropertyChanged ??= new UnityEvent<string, bool>();

        // Muat pengaturan dari PlayerPrefs
        LoadSettings();
    }

    // Memuat pengaturan dari PlayerPrefs
    private void LoadSettings()
    {
        MusicVolume = PlayerPrefs.GetFloat("MusicVolume", 1.0f); // Default ke 1.0 jika tidak ada nilai tersimpan
        IsGyroEnabled = PlayerPrefs.GetInt("IsGyroEnabled", 0) == 1; // Default ke false jika tidak ada nilai tersimpan

        Debug.Log($"Settings loaded: MusicVolume = {MusicVolume}, IsGyroEnabled = {IsGyroEnabled}");
    }

    // Menyimpan pengaturan ke PlayerPrefs
    private void SaveSettings()
    {
        PlayerPrefs.SetFloat("MusicVolume", MusicVolume);
        PlayerPrefs.SetInt("IsGyroEnabled", IsGyroEnabled ? 1 : 0);
        PlayerPrefs.Save();
        Debug.Log("Settings saved.");
    }

    // Metode untuk memperbarui volume musik
    public void UpdateMusicVolume(float newVolume)
    {
        if (Mathf.Approximately(MusicVolume, newVolume)) return; // Hindari update jika nilai sama
        MusicVolume = newVolume;
        SaveSettings(); // Simpan ke PlayerPrefs
        Debug.Log($"MusicVolume updated to {MusicVolume}");

        // Panggil UnityEvent
        OnFloatPropertyChanged?.Invoke("MusicVolume", MusicVolume);
    }

    // Metode untuk memperbarui state gyroscope
    public void UpdateGyroState(bool newState)
    {
        if (IsGyroEnabled == newState) return; // Hindari update jika nilai sama
        IsGyroEnabled = newState;
        SaveSettings(); // Simpan ke PlayerPrefs
        Debug.Log($"IsGyroEnabled updated to {IsGyroEnabled}");

        // Panggil UnityEvent
        OnBoolPropertyChanged?.Invoke("IsGyroEnabled", IsGyroEnabled);
    }
}
