using UnityEngine;

public class TimeCountUp : MonoBehaviour
{
    public LevelCompletedMenu winMenu;
    public int timeCompleted = 0;
    public int growAmount = 1;
    public int timeCountUp;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        timeCountUp = 0;
        timeCompleted = Mathf.RoundToInt(Time.timeSinceLevelLoad * 1000f);
        while(timeCountUp <= timeCompleted - growAmount){
            timeCountUp += growAmount;
            winMenu.SetWinTime(timeCountUp);
        }
        timeCountUp = timeCompleted;
        winMenu.SetWinTime(timeCountUp);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
