using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class WinController : MonoBehaviour
{
    public GameObject levelCompletedMenu;
    public Camera gameCamera;
    public ParticleSystem winParticle;
    public AudioSource winSound;
    public LevelCompletedMenu winMenu;
    public TMP_Text bestText;
    public float winDelay = 1f;
    public float slowMotionTimeScale = 0.1f;
    public float zoomSpeed = 1f;
    public bool isCutsceneLevel = false;
    public UnityEvent WinningEvent;
    public UnityEvent HasWonEvent;
    private bool _isZooming = false;
    private float _initialSize;
    private Transform _thisTransform;
    private GameObject _player;
    public int timeInMiliseconds;
    private SceneTransitionController _transitionController;

    void Start()
    {
        _initialSize = gameCamera.orthographicSize;
        _thisTransform = GetComponent<Transform>();

        // Kalau levelnya tidak ada orb, jangan tampilin orb (bikin transparan)
        if (!winMenu.hasOrb)
        {
            var orbImages = levelCompletedMenu.GetComponentsInChildren<RawImage>();
            for (int i = 0; i < orbImages.Length; i++)
            {
                Debug.Log("Orb: " + orbImages[i].name);
                orbImages[i].color = new Color(0, 0, 0, 0);
            }
        }

        WinningEvent ??= new UnityEvent();
        HasWonEvent ??= new UnityEvent();
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
        _transitionController = FindFirstObjectByType<SceneTransitionController>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _player = other.gameObject;
            if (_player.GetComponent<BallDieController>().isDying == false)
            {
                Win();
            }
        }
    }
    public void Win()
    {
        if (isCutsceneLevel)
        {
            _transitionController.GoToSceneByIndex(SceneManager.GetActiveScene().buildIndex + 1);
            return;
        }
        WinningEvent.Invoke();
        if (winParticle != null)
        {
            winParticle.Play();
        }
        if (winSound != null)
        {
            winSound.Play();
        }
        timeInMiliseconds = Mathf.RoundToInt(Time.timeSinceLevelLoad * 1000f);
        Time.timeScale = slowMotionTimeScale; // Slow motion saat menang waktu dalam game
        Time.fixedDeltaTime = 0.02f * Time.timeScale; // Biar nggak choppy
        _isZooming = true;
        StartCoroutine(Delay(winDelay * slowMotionTimeScale));

        // Update best time and collectible count
        int levelIndex = SceneManager.GetActiveScene().buildIndex;
        int lastBestTime = PlayerPrefs.GetInt("Level" + levelIndex + "BestTime", -1);
        if (timeInMiliseconds < lastBestTime || lastBestTime < 0)
        {
            bestText.text = "Best!";
            LevelInfoManager.Instance?.UpdateBestTime.Invoke(levelIndex, timeInMiliseconds);
        }
        else
        {
            bestText.text = "";
        }
        int bestCollectible = PlayerPrefs.GetInt("Level" + levelIndex + "CollectibleCount", 0);
        if (Collectible.collectibleCount > bestCollectible)
        {
            LevelInfoManager.Instance?.UpdateCollectibleCount.Invoke(levelIndex, Collectible.collectibleCount);
        }

        // Display how many orb was collected
        var orbImages = levelCompletedMenu.GetComponentsInChildren<RawImage>();
        for (int i = 0; i < Collectible.collectibleCount; i++)
        {
            Debug.Log("Orb: " + orbImages[i].name);
            orbImages[i].color = Color.white;
        }
        winMenu.SetOrbSound(Collectible.collectibleCount);
    }
    IEnumerator Delay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        HasWonEvent.Invoke();
        //_player.SetActive(false);
        levelCompletedMenu.SetActive(true);
        _isZooming = false;
        Time.timeScale = 0; // Berhentikan waktu
        Time.fixedDeltaTime = 0.02f; // Kembalikan waktu seperti semula
    }
}
