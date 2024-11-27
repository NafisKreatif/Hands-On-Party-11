using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    public TextMeshProUGUI bestTimeText;
    public int levelIndex = 0;
    void Start()
    {
        LevelInfoManager.LevelInfo levelInfo = LevelInfoManager.Instance.GetLevelInfo(levelIndex);
        Debug.Log(levelInfo.bestTime);
        bestTimeText.text = "Best: " + FormatBestTime(levelInfo.bestTime);

        var orbImages = gameObject.GetComponentsInChildren<RawImage>();
        for (int i = levelInfo.collectibleCount; i < 3; i++) orbImages[i].enabled = false;
    }

    private string FormatBestTime(int timeInMiliseconds)
    {
        if (timeInMiliseconds == -1) return "––:––:–––";
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
        return minutes + ":" + seconds + ":" + miliseconds;
    }
}
