using UnityEngine;

public class Collectible : MonoBehaviour
{
    public static int collectibleCount = 0; // Static variable to track total collectibles
    public AudioSource collectSound;
    public ParticleSystem boom;
    private SpriteRenderer _renderer;
    private bool _hasBeenCollected = false;
    void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player collected it
        if (other.CompareTag("Player") && !_hasBeenCollected)
        {
            collectibleCount++; // Increase count
            collectSound.Play(); // Play audio
            boom.Play();
            _renderer.enabled = false; // Hilangkan
            _hasBeenCollected = true;
            Destroy(gameObject, collectSound.clip.length); // Remove collectible
        }
    }
}