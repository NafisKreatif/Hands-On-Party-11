using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour, IPointerClickHandler
{
    public TextMeshProUGUI bestTimeText;
    public int levelIndex = 0;
    private Button _button;
    void Start()
    {
        LevelInfoManager.LevelInfo levelInfo = LevelInfoManager.Instance.GetLevelInfo(levelIndex);
        Debug.Log(levelInfo.bestTime);
        bestTimeText.text = "Best: " + FormatBestTime(levelInfo.bestTime);

        var orbImages = GetComponentsInChildren<RawImage>();
        for (int i = levelInfo.collectibleCount; i < 3; i++) orbImages[i].enabled = false;

        _button = GetComponent<Button>();
    }
    public void OnPointerClick(PointerEventData data)
    {
        if (_button != null && _button.interactable == false) return;
        SceneTransitionManager.Instance.GoToScene(levelIndex);
    }
    private string FormatBestTime(int timeInMiliseconds)
    {
        if (timeInMiliseconds < 0) return "––:––:–––";
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
        return minutes + ":" + seconds + ":" + miliseconds;
    }
}
