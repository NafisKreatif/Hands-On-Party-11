using UnityEngine;

public class TimeCountUp : MonoBehaviour
{
    public LevelCompletedMenu winMenu;
    public int timeCompleted = 0;
    private int _growAmount = 0;
    private int _timeCountUp = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        timeCompleted = Mathf.RoundToInt(Time.timeSinceLevelLoad * 1000f);
    }

    // Update is called once per frame
    void Update()
    {
        if(_timeCountUp < timeCompleted - _growAmount){
            _timeCountUp += _growAmount;
        }
        else{
            _timeCountUp = timeCompleted;
        }
        winMenu.SetWinTime(_timeCountUp);
    }
}
