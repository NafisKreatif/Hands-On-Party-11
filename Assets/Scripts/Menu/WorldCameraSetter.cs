using UnityEngine;

public class WorldCameraSetter : MonoBehaviour
{
    private Canvas _thisCanvas;

    void Start()
    {
        _thisCanvas = GetComponent<Canvas>();
        _thisCanvas.worldCamera = Camera.main;
    }

    void Update()
    {
        // Kalo worldCameranya hilang, biasanya karena unload scene
        if (_thisCanvas.worldCamera == null)
        {
            _thisCanvas.worldCamera = Camera.main;
        }
    }
}