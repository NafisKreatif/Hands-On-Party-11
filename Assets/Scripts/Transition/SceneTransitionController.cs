using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionController : MonoBehaviour
{
    public static SceneTransitionController Instance;
    private Transform _cameraTransform; // 
    public Transform blackScreen;
    public Transform maskTransform;
    public bool transitionOnStart = true; // Apakah perlu ada transisi saat masuk scene ini
    public float maskScale = 1f; // Besar mask relatif terhadap screen
    public float transitionTime = 1f; // Dalam detik
    private float _transitionSpeed; // Kecepatan transisi
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
        if (_isInTransition)
        {
            // Biar kalo kameranya gerak-gerak dan berotasi masih menutupin
            Vector3 position = new(_cameraTransform.position.x, _cameraTransform.position.y, -9);
            Quaternion rotation = _cameraTransform.rotation;
            blackScreen.SetPositionAndRotation(position, rotation);
            maskTransform.SetPositionAndRotation(position, rotation);
            // Update scale dari masknya
            if (Time.timeScale != 1) // Lagi pause pake unscaleDeltaTime biar tidak terpengaruh
            {
                maskTransform.localScale += new Vector3(_transitionSpeed * Time.unscaledDeltaTime, _transitionSpeed * Time.unscaledDeltaTime, 0);
            }
            else // Nggak lagi pause, pake deltaTime biar kalo ganti scene bener transisinya
            {
                maskTransform.localScale += new Vector3(_transitionSpeed * Time.deltaTime, _transitionSpeed * Time.deltaTime, 0);
            }
        }
    }
    IEnumerator FinishTransitionIn(float seconds, Vector3 maskScaleTarget)
    {
        if (Time.timeScale != 1) yield return new WaitForSecondsRealtime(seconds);
        else yield return new WaitForSeconds(seconds);
        _isInTransition = false; // Anggap sudah selesai
        maskTransform.localScale = maskScaleTarget; // Pas kan dengan hasil akhir yang diinginkan
    }
    public void TransitionIn()
    {
        if (_isInTransition)
        { // Jangan transisi lagi kalo lagi transisi
            Debug.Log("Can't set another transition!");
            return;
        }
        float scale = new Vector2(blackScreen.localScale.x, blackScreen.localScale.y).magnitude * maskScale;
        maskTransform.localScale = new(0, 0, maskTransform.localScale.z); // Scale awal
        _isInTransition = true;
        _transitionSpeed = scale / transitionTime; // Scale membesar
        StartCoroutine(FinishTransitionIn(transitionTime, new Vector3(scale, scale, maskTransform.localScale.z)));
    }
    public void TransitionOut()
    {
        if (_isInTransition)
        { // Jangan transisi lagi kalo lagi transisi
            Debug.Log("Can't set another transition!");
            return;
        }
        float scale = new Vector2(blackScreen.localScale.x, blackScreen.localScale.y).magnitude * maskScale;
        maskTransform.localScale = new(scale, scale, maskTransform.localScale.z); // Scale awal
        _isInTransition = true;
        _transitionSpeed = -scale / transitionTime; // Scale mengecil
        StartCoroutine(FinishTransitionIn(transitionTime, new Vector3(0, 0, maskTransform.localScale.z)));
    }
    public void GoToSceneByName(string name)
    {
        TransitionOut(); // Transisi dulu
        StartCoroutine(LoadSceneByName(name, transitionTime)); // Baru load scene baru
    }
    public void GoToSceneByIndex(int index)
    {
        TransitionOut(); // Transisi dulu
        StartCoroutine(LoadSceneByIndex(index, transitionTime)); // Baru load scene baru
    }
    public void ReloadScene()
    {
        TransitionOut(); // Transisi dulu
        StartCoroutine(LoadSceneByIndex(SceneManager.GetActiveScene().buildIndex, transitionTime)); // Baru load scene baru
    }
    IEnumerator LoadSceneByName(string name, float timeInSeconds)
    {
        if (Time.timeScale != 1) yield return new WaitForSecondsRealtime(timeInSeconds);
        else yield return new WaitForSeconds(timeInSeconds);
        Time.timeScale = 1;
        SceneManager.LoadScene(name);
    }
    IEnumerator LoadSceneByIndex(int index, float timeInSeconds)
    {
        if (Time.timeScale != 1) yield return new WaitForSecondsRealtime(timeInSeconds);
        else yield return new WaitForSeconds(timeInSeconds);
        Time.timeScale = 1;
        SceneManager.LoadScene(index);
    }
}
