using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;

public class BallDieController : MonoBehaviour
{
    public TransitionController transitionController;
    public ParticleSystem dieParticle;
    public AudioSource[] dieSound;
    public SpriteShapeRenderer shapeRenderer;
    public TrailRenderer trailRenderer;
    public float scaleSpeed = -0.2f; // Seberapa cepat bolanya membesar ketika mati
    private Transform _bolaTransform;
    public bool isDying = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _bolaTransform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDying)
        {
            // Kalo lagi mati, bolanya membesar
            shapeRenderer.enabled = false; // Hilangkan bola dari kamera
            //_bolaTransform.localScale = new Vector3(_bolaTransform.localScale.x + scaleSpeed * Time.deltaTime, _bolaTransform.localScale.y + scaleSpeed * Time.deltaTime, _bolaTransform.localScale.z);
        }
    }
    public void Die()
    {
        if (isDying) return;
        isDying = true;
        dieParticle.Play();
        dieSound[Random.Range(0, dieSound.Length)].Play();
        StartCoroutine(DeadIn(transitionController.transitionTime * 0.5f)); // Menghentikan pembesaran bola
        StartCoroutine(ResetSceneIn(transitionController.transitionTime * 1.5f)); // Reset Scene
    }

    // Bola benar-benar menghilang dalam waktu sekian detik
    IEnumerator DeadIn(float seconds)
    {
        yield return new WaitForSeconds(seconds);
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
