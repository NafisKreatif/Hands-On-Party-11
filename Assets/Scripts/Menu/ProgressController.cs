using UnityEngine;

public class ProgressController : MonoBehaviour
{
    public void ResetProgress()
    {
        for (int levelIndex = 1; levelIndex < 20; levelIndex++)
        {
            PlayerPrefs.SetInt("Level" + levelIndex + "BestTime", -1);
            PlayerPrefs.SetInt("Level" + levelIndex + "CollectibleCount", 0);
        }
        PlayerPrefs.Save();
    }
    public void UnlockAll()
    {
        for (int levelIndex = 1; levelIndex < 20; levelIndex++)
        {
            PlayerPrefs.SetInt("Level" + levelIndex + "BestTime", -2);
        }
        PlayerPrefs.Save();
    }
}
