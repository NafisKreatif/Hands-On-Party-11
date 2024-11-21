using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance { get; private set; }

    public float MusicVolume { get; set; } = 1.0f; // Default volume penuh
    public bool IsGyroEnabled { get; set; } = false; // Default gyroscope tidak aktif

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
    }
}
