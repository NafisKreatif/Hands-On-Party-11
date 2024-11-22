using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class WinController : MonoBehaviour
{
    public static WinController Instance;
    public GameObject levelCompletedMenu;
    public Camera gameCamera;
    public ParticleSystem winParticle;
    public AudioSource winSound;
    public LevelCompletedMenu winMenu;
    public float winDelay = 1f;
    public float slowMotionTimeScale = 0.1f;
    public float zoomSpeed = 1f;
    public UnityEvent WinEvent;
    private bool _isZooming = false;
    private float _initialSize;
    private Transform _thisTransform;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;        
    }
    void Start()
    {
        _initialSize = gameCamera.orthographicSize;
        _thisTransform = GetComponent<Transform>();

        WinEvent ??= new UnityEvent();
    }
    void Update()
    {
        if (_isZooming && gameCamera.orthographicSize > 0.5f * _initialSize)
        {
            Vector3 positionChange = _thisTransform.position - gameCamera.transform.position;
            positionChange.z = 0; // Jangan ubah z indexnya
            gameCamera.transform.position += Time.deltaTime / slowMotionTimeScale * positionChange; // Gerakkan kamera ke arah goal
            gameCamera.orthographicSize -= zoomSpeed * Time.deltaTime / slowMotionTimeScale; // Zoom kameranya
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Win();
        }
    }
    public void Win()
    {
        WinEvent.Invoke();
        if (winParticle != null)
        {
            winParticle.Play();
        }
        if (winSound != null)
        {
            winSound.Play();
        }
        winMenu.SetWinTime(Mathf.RoundToInt(Time.timeSinceLevelLoad * 1000f));
        Time.timeScale = slowMotionTimeScale; // Slow motion saat menang waktu dalam game
        Time.fixedDeltaTime = 0.02f * Time.timeScale; // Biar nggak choppy
        _isZooming = true;
        StartCoroutine(Delay(winDelay * slowMotionTimeScale));
    }
    IEnumerator Delay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        levelCompletedMenu.SetActive(true);
        _isZooming = false;
        Time.timeScale = 0; // Berhentikan waktu
        Time.fixedDeltaTime = 0.02f; // Kembalikan waktu seperti semula
    }
}
