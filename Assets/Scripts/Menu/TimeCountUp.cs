using UnityEngine;

public class TimeCountUp : MonoBehaviour
{
    public LevelCompletedMenu winMenu;
    public WinController winController;
    public AudioSource timeCountingSound;
    public float countingTime = 5f;
    public float countingDelay = 0.5f;
    private int _timeCompleted = 0;
    private int _timeCountUp;
    private float _delay;
    private bool _isEnabled = false;
    private float _time = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        _timeCountUp = 0;
        _timeCompleted = winController.timeInMiliseconds;
        _delay = countingDelay;
        _isEnabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isEnabled)
        {
            if (_delay > 0) {
                _delay -= Time.unscaledDeltaTime;
            }
            else if (_timeCountUp < _timeCompleted)
            {
                if (!timeCountingSound.isPlaying) {
                    timeCountingSound.Play();
                }
                _timeCountUp = Mathf.RoundToInt(Mathf.Lerp(0, _timeCompleted, _time * _time / countingTime));
                _time += Time.unscaledDeltaTime;
                winMenu.SetWinTime(_timeCountUp);
            }
            else
            {
                _timeCountUp = _timeCompleted;
                winMenu.SetWinTime(_timeCountUp);
                timeCountingSound.loop = false;
            }
        }
    }
}
