using System.Collections;
using UnityEngine;

public class WinController : MonoBehaviour
{
    public GameObject levelCompletedMenu;
    public Camera gameCamera;
    public ParticleSystem winParticle;
    public AudioSource winSound;

    public float winDelay = 1f;
    public float slowMotionTimeScale = 0.1f;
    public float zoomSpeed = 1f;
    private bool _isZooming = false;
    private float _initialSize;
    private Transform _thisTransform;
    void Start()
    {
        _initialSize = gameCamera.orthographicSize;
        _thisTransform = GetComponent<Transform>();
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
        if (winParticle != null)
        {
            winParticle.Play();
        }
        if (winSound != null)
        {
            winSound.Play();
        }
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
