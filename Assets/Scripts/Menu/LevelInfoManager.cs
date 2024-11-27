using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

// Information to be stored: Number of best collectibles and best time
public class LevelInfoManager : MonoBehaviour
{
    public struct LevelInfo
    {
        public int collectibleCount;
        public int bestTime;
    }

    public static LevelInfoManager Instance;
    public int[] maxCollectibleCounts;
    public UnityEvent<int, int> UpdateCollectibleCount;
    public UnityEvent<int, int> UpdateBestTime;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        UpdateCollectibleCount ??= new UnityEvent<int, int>();
        UpdateBestTime ??= new UnityEvent<int, int>();

        UpdateCollectibleCount.AddListener(OnUpdateCollectible);
        UpdateBestTime.AddListener(OnUpdateBestTime);
    }

    public LevelInfo GetLevelInfo(int levelIndex)
    {
        int collectibleCount = PlayerPrefs.GetInt("Level" + levelIndex + "CollectibleCount", 0);
        int bestTime = PlayerPrefs.GetInt("Level" + levelIndex + "BestTime", -1);
        return new LevelInfo
        {
            collectibleCount = collectibleCount,
            bestTime = bestTime
        };
    }

    private void OnUpdateCollectible(int levelIndex, int count)
    {
        PlayerPrefs.SetInt("Level" + levelIndex + "CollectibleCount", count);
        PlayerPrefs.Save();
    }

    private void OnUpdateBestTime(int levelIndex, int time)
    {
        PlayerPrefs.SetInt("Level" + levelIndex + "BestTime", time);
        PlayerPrefs.Save();
    }
}
