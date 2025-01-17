using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BallCollectibleCount : MonoBehaviour
{
    public float glowIntensity = 0.35f; // Seberapa besar intensitas glow bertambah setiap mendapatkan collectible
    public float glowRadius = 1f;
    public Light2D ballLight;
    void Update()
    {
        ballLight.intensity = glowIntensity * Collectible.collectibleCount; // Besar intensitas sesuai dengan collectible yang telah didapat
        ballLight.pointLightOuterRadius = glowRadius * Mathf.Sqrt(Collectible.collectibleCount); // Besar radius glow sesuai dengan collectible yang telah didapat
    }
}
