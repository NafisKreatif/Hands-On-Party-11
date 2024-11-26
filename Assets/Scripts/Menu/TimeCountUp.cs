using UnityEngine;

public class TimeCountUp : MonoBehaviour
{
    public LevelCompletedMenu winMenu;
    public WinController winController;
    public int timeCompleted = 0;
    public int growAmount;
    public int timeCountUp;
    private bool _isEnabled = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        timeCountUp = 0;
        timeCompleted = winController.timeInMiliseconds;
        growAmount = Mathf.RoundToInt(timeCompleted/180);
        _isEnabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(_isEnabled){
            if(timeCountUp <= timeCompleted - growAmount){
                timeCountUp += growAmount;
                winMenu.SetWinTime(timeCountUp);
            }
            else{
                timeCountUp = timeCompleted;
                winMenu.SetWinTime(timeCountUp);
            }
        }
    }
}
