using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance;
    public Transform _cameraTransform; // 
    public Transform blackScreen;
    public Transform maskTransform;
    public bool transitionOnStart = true; // Apakah perlu ada transisi saat masuk scene ini
    public float transitionTime = 1f; // Dalam detik
    private float _transitionPercentage; // Persen transisi
    private bool _isInTransition = false;
    void Awake()
    {
        // Kalo udah ada transition manager
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // ga usah buat lagi, pake yang lama
        }
        else
        {
            // Set instance
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _cameraTransform = Camera.main.transform;
        if (transitionOnStart)
        {
            TransitionIn();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_cameraTransform == null)
        {
            _cameraTransform = Camera.main.transform;
        }
        // Biar kalo kameranya gerak-gerak dan berotasi masih menutupin
        Vector3 scale = new(Camera.main.orthographicSize * Camera.main.aspect * 2.5f, Camera.main.orthographicSize * 2.5f, 1);
        Vector3 position = new(_cameraTransform.position.x, _cameraTransform.position.y, -9);
        Quaternion rotation = _cameraTransform.rotation;
        blackScreen.localScale = scale;
        blackScreen.SetPositionAndRotation(position, rotation);
        maskTransform.SetPositionAndRotation(position, rotation);
    }
    IEnumerator TransitioningIn()
    {
        float targetScale = new Vector2(blackScreen.localScale.x, blackScreen.localScale.y).magnitude;
        maskTransform.localScale = new(0, 0, maskTransform.localScale.z); // Scale awal
        UIDisablerManager.Instance.DisableUI();
        _isInTransition = true;
        _transitionPercentage = 0;
        while (_transitionPercentage < 1)
        {
            yield return null;
            if (Time.timeScale == 1) _transitionPercentage += Time.deltaTime / transitionTime;
            else _transitionPercentage += Time.unscaledDeltaTime / transitionTime;
            targetScale = new Vector2(blackScreen.localScale.x, blackScreen.localScale.y).magnitude; // Ganti sesuai besar kamera jika berubah
            float currentScale = targetScale * _transitionPercentage;
            maskTransform.localScale = new Vector3(currentScale, currentScale, 1);
        }
        UIDisablerManager.Instance.EnableUI();
        _transitionPercentage = 1;
        _isInTransition = false; // Anggap sudah selesai
        maskTransform.localScale = new Vector3(targetScale, targetScale, 1); // Pas kan dengan hasil akhir yang diinginkan
    }
    IEnumerator TransitioningOut()
    {
        float initialScale = new Vector2(blackScreen.localScale.x, blackScreen.localScale.y).magnitude;
        maskTransform.localScale = new(initialScale, initialScale, maskTransform.localScale.z); // Scale awal
        UIDisablerManager.Instance.DisableUI();
        _isInTransition = true;
        _transitionPercentage = 0;
        while (_transitionPercentage < 1)
        {
            yield return null;
            if (Time.timeScale == 1) _transitionPercentage += Time.deltaTime / transitionTime;
            else _transitionPercentage += Time.unscaledDeltaTime / transitionTime;
            initialScale = new Vector2(blackScreen.localScale.x, blackScreen.localScale.y).magnitude; // Ganti sesuai besar kamera jika berubah
            float currentScale = initialScale * (1 - _transitionPercentage);
            maskTransform.localScale = new Vector3(currentScale, currentScale, 1);
        }
        _transitionPercentage = 1;
        UIDisablerManager.Instance.EnableUI();
        maskTransform.localScale = new Vector3(0, 0, 1); // Pas kan dengan hasil akhir yang diinginkan
    }
    public void TransitionIn()
    {
        StartCoroutine(TransitioningIn());
    }
    public void TransitionOut()
    {
        StartCoroutine(TransitioningOut());
    }
    public void GoToScene(string name)
    {
        if (_isInTransition)
        { // Jangan ganti scene lagi kalo udah lagi transisi
            Debug.LogWarning("Already in transition, can't load scene!");
            return;
        }
        TransitionOut(); // Transisi dulu
        StartCoroutine(LoadScene(name, transitionTime)); // Baru load scene baru

    }
    public void GoToScene(int index)
    {
        if (_isInTransition)
        { // Jangan ganti scene lagi kalo udah lagi transisi
            Debug.LogWarning("Already in transition, can't load scene!");
            return;
        }
        TransitionOut(); // Transisi dulu
        StartCoroutine(LoadScene(index, transitionTime)); // Baru load scene baru

    }
    public void ReloadScene()
    {
        if (_isInTransition)
        { // Jangan ganti scene lagi kalo udah lagi transisi
            Debug.LogWarning("Already in transition, can't load scene!");
            return;
        }
        TransitionOut(); // Transisi dulu
        StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex, transitionTime)); // Baru load scene baru
    }
    public void NextScene()
    {
        if (_isInTransition)
        { // Jangan ganti scene lagi kalo udah lagi transisi
            Debug.LogWarning("Already in transition, can't load scene!");
            return;
        }
        TransitionOut(); // Transisi dulu
        StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex + 1, transitionTime)); // Baru load scene baru
    }
    IEnumerator LoadScene(string name, float timeInSeconds)
    {
        if (Time.timeScale != 1) yield return new WaitForSecondsRealtime(timeInSeconds);
        else yield return new WaitForSeconds(timeInSeconds);
        Time.timeScale = 1;
        yield return SceneManager.LoadSceneAsync(name);
        if (transitionOnStart) TransitionIn(); // Transisi setelah loading
    }
    IEnumerator LoadScene(int index, float timeInSeconds)
    {
        if (Time.timeScale != 1) yield return new WaitForSecondsRealtime(timeInSeconds);
        else yield return new WaitForSeconds(timeInSeconds);
        Time.timeScale = 1;
        yield return SceneManager.LoadSceneAsync(index);
        if (transitionOnStart) TransitionIn(); // Transisi setelah loading
    }
}
