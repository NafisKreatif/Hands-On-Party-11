using UnityEngine;
using UnityEngine.Events;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance { get; private set; }

    public UnityEvent<string, float> OnFloatPropertyChanged;
    public UnityEvent<string, bool> OnBoolPropertyChanged;

    [SerializeField] 
    public float MusicVolume { get; private set; } = 1.0f;

    
    [SerializeField]
    public bool IsGyroEnabled { get; private set; } = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); 
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); 
        

        
        OnFloatPropertyChanged ??= new UnityEvent<string, float>();
        OnBoolPropertyChanged ??= new UnityEvent<string, bool>();

        
        LoadSettings();
    }

    
    private void LoadSettings()
    {
        MusicVolume = PlayerPrefs.GetFloat("MusicVolume", 1.0f); 
        IsGyroEnabled = PlayerPrefs.GetInt("IsGyroEnabled", 0) == 1; 

        
    }

    
    private void SaveSettings()
    {
        PlayerPrefs.SetFloat("MusicVolume", MusicVolume);
        PlayerPrefs.SetInt("IsGyroEnabled", IsGyroEnabled ? 1 : 0);
        PlayerPrefs.Save();
        Debug.Log("Settings saved.");
    }

    
    public void UpdateMusicVolume(float newVolume)
    {
        if (Mathf.Approximately(MusicVolume, newVolume)) return; 
        MusicVolume = newVolume;
        SaveSettings(); 
        Debug.Log($"MusicVolume updated to {MusicVolume}");

        
        OnFloatPropertyChanged?.Invoke("MusicVolume", MusicVolume);
    }

    
    public void UpdateGyroState(bool newState)
    {
        if (IsGyroEnabled == newState) return; 
        IsGyroEnabled = newState;
        SaveSettings(); 
        Debug.Log($"IsGyroEnabled updated to {IsGyroEnabled}");

        
        OnBoolPropertyChanged?.Invoke("IsGyroEnabled", IsGyroEnabled);
    }
}
