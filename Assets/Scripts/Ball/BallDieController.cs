using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;

public class BallDieController : MonoBehaviour
{
    public ParticleSystem dieParticle;
    public AudioSource[] dieSound;
    public SpriteShapeRenderer shapeRenderer;
    public TrailRenderer trailRenderer;
    public SpriteRenderer mataRenderer;
    public float scaleSpeed = -0.2f; // Seberapa cepat bolanya membesar ketika mati
    public bool isDying = false;

    public void Die()
    {
        if (isDying) return;
        isDying = true;
        dieParticle.Play();
        dieSound[Random.Range(0, dieSound.Length)].Play();
        StartCoroutine(DeadIn(SceneTransitionManager.Instance.transitionTime * 0.5f)); // Menghentikan pembesaran bola
        shapeRenderer.enabled = false; // Hilangkan bola dari kamera
        mataRenderer.enabled = false; // Hilangkan mata dari kamera
    }

    // Bola benar-benar menghilang dalam waktu sekian detik
    IEnumerator DeadIn(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        // Hanya particle system yang masih kelihatan, renderer sisanya di-disable
        trailRenderer.enabled = false; // Hilangkan trailnya juga dari kamera
        SceneTransitionManager.Instance.ReloadScene();
    }
}
