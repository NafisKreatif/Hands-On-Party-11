using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance { get; private set; }
    [Header("UI Elements")]
    public Slider[] sliders;
    public Toggle[] toggles;

    [Header("Events")]
    public UnityEvent<string, float> FloatPropertyChangeEvent;
    public UnityEvent<string, int> IntPropertyChangeEvent;
    public UnityEvent<string, bool> BoolPropertyChangeEvent;

    public readonly Dictionary<string, float> FloatSettings = new() {
        { "Master Volume", 1.0f },
    };
    public readonly Dictionary<string, int> Intsettings = new();
    public readonly Dictionary<string, bool> BoolSettings = new() {
        { "Use Gyro", true },
    };

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance.gameObject);
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Initialize events
        FloatPropertyChangeEvent ??= new UnityEvent<string, float>();
        IntPropertyChangeEvent ??= new UnityEvent<string, int>();
        BoolPropertyChangeEvent ??= new UnityEvent<string, bool>();

        // Subscribe to property change events
        FloatPropertyChangeEvent.AddListener(OnFloatPropertyChanged);
        IntPropertyChangeEvent.AddListener(OnIntPropertyChanged);
        BoolPropertyChangeEvent.AddListener(OnBoolPropertyChanged);

        // Load settings   
        foreach (var setting in FloatSettings.ToList()) if (PlayerPrefs.HasKey(setting.Key)) FloatSettings[setting.Key] = PlayerPrefs.GetFloat(setting.Key);
        foreach (var setting in Intsettings.ToList()) if (PlayerPrefs.HasKey(setting.Key)) Intsettings[setting.Key] = PlayerPrefs.GetInt(setting.Key);
        foreach (var setting in BoolSettings.ToList()) if (PlayerPrefs.HasKey(setting.Key)) BoolSettings[setting.Key] = PlayerPrefs.GetInt(setting.Key) == 1;
    }

    private void Start() {
        foreach (var slider in sliders) {
            if (slider != null) {
                slider.value = FloatSettings[slider.name];
                slider.onValueChanged.AddListener((value) => FloatPropertyChangeEvent.Invoke(slider.name, value));
            }
        }

        foreach (var toggle in toggles) {
            if (toggle != null) {
                toggle.isOn = BoolSettings[toggle.name];
                toggle.onValueChanged.AddListener((value) => BoolPropertyChangeEvent.Invoke(toggle.name, value));
            }
        }
    }

    private void OnFloatPropertyChanged(string key, float value)
    {
        FloatSettings[key] = value;
        PlayerPrefs.SetFloat(key, value);
        PlayerPrefs.Save();
    }

    private void OnIntPropertyChanged(string key, int value)
    {
        Intsettings[key] = value;
        PlayerPrefs.SetInt(key, value);
        PlayerPrefs.Save();
    }

    private void OnBoolPropertyChanged(string key, bool value)
    {
        BoolSettings[key] = value;
        PlayerPrefs.SetInt(key, value ? 1 : 0);
        PlayerPrefs.Save();
    }
}
