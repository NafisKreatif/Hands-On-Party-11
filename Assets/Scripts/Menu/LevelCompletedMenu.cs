using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelCompletedMenu : MonoBehaviour
{
    public TMP_Text timeText; // Untuk menampilkan waktu penyelesaian
    public bool hasOrb = true;
    public AudioSource[] orbSounds;
    public void GoToNextLevel(int level)
    {
        SceneTransitionManager.Instance.GoToScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void GoToMainMenu()
    {
        SceneTransitionManager.Instance.GoToScene(0);
    }
    public void SetWinTime(int timeInMiliseconds)
    {
        string minutes = (timeInMiliseconds / 60000).ToString();
        string seconds = (timeInMiliseconds / 1000).ToString();
        string miliseconds = (timeInMiliseconds % 1000).ToString();
        while (minutes.Length < 2)
        {
            minutes = "0" + minutes;
        }
        while (seconds.Length < 2)
        {
            seconds = "0" + seconds;
        }
        while (miliseconds.Length < 3)
        {
            miliseconds = "0" + miliseconds;
        }
        timeText.text = "Time: " + minutes + ":" + seconds + ":" + miliseconds;
    }
    public void SetOrbSound(int total)
    {
        Debug.Log("TOTAL: " + total);
        for (int i = 0; i < orbSounds.Length; i++)
        {
            if (i < total)
            {
                orbSounds[i].playOnAwake = true;
            }
            else
            {
                orbSounds[i].playOnAwake = false;
            }
        }
    }
}
