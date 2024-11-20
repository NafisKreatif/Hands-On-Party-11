using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;

public class BallDieController : MonoBehaviour
{
    public TransitionController transitionController;
    public ParticleSystem dieParticle;
    public SpriteShapeRenderer shapeRenderer;
    public TrailRenderer trailRenderer;
    public float scaleSpeed = -0.2f; // Seberapa cepat bolanya membesar ketika mati
    private Transform _bolaTransform;
    private bool _isDying = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _bolaTransform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            Die();
        }
        if (_isDying)
        {
            // Kalo lagi mati, bolanya membesar
            _bolaTransform.localScale = new Vector3(_bolaTransform.localScale.x + scaleSpeed * Time.deltaTime, _bolaTransform.localScale.y + scaleSpeed * Time.deltaTime, _bolaTransform.localScale.z);
        }
    }
    void Die()
    {
        _isDying = true;
        dieParticle.Play();
        StartCoroutine(DeadIn(transitionController.transitionTime * 0.5f)); // Menghentikan pembesaran bola
        StartCoroutine(ResetSceneIn(transitionController.transitionTime * 1.5f)); // Reset Scene
    }

    // Bola benar-benar menghilang dalam waktu sekian detik
    IEnumerator DeadIn(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        _isDying = false; // Menghentikan pembesaran bola
        // Hanya particle system yang masih kelihatan, renderer sisanya di-disable
        shapeRenderer.enabled = false; // Hilangkan bola dari kamera
        trailRenderer.enabled = false; // Hilangkan trailnya juga dari kamera
        transitionController.TransitionOut();
    }
    IEnumerator ResetSceneIn(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Load scene yang sama
    }
}
