using UnityEngine;
using UnityEngine.UI;

public class SettingsButtonSetter : MonoBehaviour
{
    [SerializeField] private Button _thisButton;
    [SerializeField] private Button _backButton;
    [SerializeField] private Canvas _settingsCanvas;
    [SerializeField] public Canvas _backToThisCanvas;
    void Start()
    {
        _thisButton = GetComponent<Button>();
        _settingsCanvas = SettingsManager.Instance.GetComponent<Canvas>();
        _backButton = SettingsManager.Instance.GetComponentInChildren<Button>().gameObject.GetComponentInChildren<Button>();
        SetCanvas();
    }
    void SetCanvas()
    {
        _thisButton.onClick.AddListener(() => EnableCanvas(_settingsCanvas));
        _backButton.onClick.AddListener(() => EnableCanvas(_backToThisCanvas));
    }
    void EnableCanvas(Canvas canvas)
    {
        canvas.enabled = true;
    }
}
