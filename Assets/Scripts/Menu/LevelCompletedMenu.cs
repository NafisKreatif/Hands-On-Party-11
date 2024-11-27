using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelCompletedMenu : MonoBehaviour
{
    public TransitionController transitionController;
    public TMP_Text timeText; // Untuk menampilkan waktu penyelesaian
    public void GoToNextLevel(int level)
    {
        transitionController.GoToSceneByIndex(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void GoToMainMenu()
    {
        transitionController.GoToSceneByIndex(0);
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
        while (miliseconds.Length < 2)
        {
            miliseconds = "0" + miliseconds;
        }
        timeText.text = "Time: " + minutes + ":" + seconds + ":" + miliseconds;
    }
}
