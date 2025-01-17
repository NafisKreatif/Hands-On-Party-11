using UnityEngine;
using UnityEngine.Tilemaps;

public class FlickingTrap : MonoBehaviour
{
 
    public Tilemap spikeTilemap; // Reference to the spike tilemap
    public float appearDuration = 2f; // Time the spikes are active
    public float disappearDuration = 2f; // Time the spikes are inactive

    private bool _spikesActive = true; // Whether spikes are currently active
    private float _timer = 0f; // timer to track durations

    void Update()
    {
        // Increment the timer
        _timer += Time.deltaTime;

        // Toggle spikes based on timer and current state
        if (_spikesActive && _timer >= appearDuration)
        {
            SetSpikesActive(false); // Deactivate spikes
            _timer = 0f; // Reset timer
        }
        else if (!_spikesActive && _timer >= disappearDuration)
        {
            SetSpikesActive(true); // Reactivate spikes
            _timer = 0f; // Reset timer
        }
    }

    void SetSpikesActive(bool active)
    {
        _spikesActive = active;

        // Enable or disable the tilemap's collider
        var collider = spikeTilemap.GetComponent<TilemapCollider2D>();
        if (collider != null)
        {
            collider.enabled = active;
        }

        // Toggle visibility (optional)
        spikeTilemap.color = active ? Color.white : new Color(1f, 1f, 1f, 0.3f); // Fades out inactive spikes
    }

}
