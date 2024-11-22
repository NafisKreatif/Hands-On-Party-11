using UnityEngine;

public class Collectible : MonoBehaviour
{
    public static int collectibleCount = 0; // Static variable to track total collectibles
    public AudioSource collectSound;
    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player collected it
        if (other.CompareTag("Player"))
        {
            collectibleCount++; // Increase count
            collectSound.Play(); // Play audio
            Destroy(gameObject,0.2f); // Remove collectible
        }
    }
}