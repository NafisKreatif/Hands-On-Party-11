using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BallCollectibleCount : MonoBehaviour
{
    private float _collectibleCount = 0f;
    public float glowIntensity = 1f; // Seberapa besar intensitas glow bertambah setiap mendapatkan collectible
    public Light2D ballLight;

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player collected it
        if (other.CompareTag("Collectible"))
        {
            Debug.Log("Collected");
            _collectibleCount += 1f;
            ballLight.intensity = glowIntensity * _collectibleCount; // Besar intensitas sesuai dengan collectible yang telah didapat
        }
    }
}
