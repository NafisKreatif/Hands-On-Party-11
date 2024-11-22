using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class mainmenu : MonoBehaviour
{
    // Bagian Main menu
    public void play() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); 
    }

    public void quit() 
    {
        Application.Quit();
        Debug.Log("Player Has quit the game");
    }

    private void Start()
    {
        
        if (SettingsManager.Instance == null)
        {
            
            Instantiate(settingsManagerPrefab); 
        }

        
        if (musicSlider != null)
        {
            musicSlider.value = SettingsManager.Instance.MusicVolume;
            myMixer.SetFloat("music", Mathf.Log10(SettingsManager.Instance.MusicVolume) * 20);

            
            musicSlider.onValueChanged.AddListener(UpdateMusicVolume);
        }

        
        if (gyroToggle != null)
        {
            gyroToggle.isOn = SettingsManager.Instance.IsGyroEnabled;

            
            gyroToggle.onValueChanged.AddListener(UpdateGyroState);
        }

        
        SettingsManager.Instance.OnFloatPropertyChanged.AddListener(OnFloatPropertyChanged);
        SettingsManager.Instance.OnBoolPropertyChanged.AddListener(OnBoolPropertyChanged);
    }

    // Bagian Audio
    [SerializeField] public AudioMixer myMixer; 
    [SerializeField] public Slider musicSlider; 

    public void SetMusicVolume()
    {
        if (musicSlider != null)
        {
            UpdateMusicVolume(musicSlider.value); 
        }
    }

    private void UpdateMusicVolume(float value)
    {
        SettingsManager.Instance.UpdateMusicVolume(value); 
        myMixer.SetFloat("music", Mathf.Log10(value) * 20);
        
    }

    
    [SerializeField] public Toggle gyroToggle; 

    public void ToggleGyroscope(bool isOn)
    {
        UpdateGyroState(isOn); 
    }

    private void UpdateGyroState(bool value)
    {
        SettingsManager.Instance.UpdateGyroState(value); 
        
    }

    
    private void OnFloatPropertyChanged(string propertyName, float value)
    {
        if (propertyName == "MusicVolume" && musicSlider != null)
        {
            musicSlider.value = value;
            myMixer.SetFloat("music", Mathf.Log10(value) * 20);
            
        }
    }

    
    private void OnBoolPropertyChanged(string propertyName, bool value)
    {
        if (propertyName == "IsGyroEnabled" && gyroToggle != null)
        {
            gyroToggle.isOn = value;
            
        }
    }


    [SerializeField] public GameObject settingsManagerPrefab;
}
