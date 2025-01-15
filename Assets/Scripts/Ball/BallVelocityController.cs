using System.Collections;
using UnityEngine;

public class BallVelocityController : MonoBehaviour
{
    public float maxVelocity = 7f;
    public AudioSource[] wallHitSound;
    public AudioSource wooshSound;
    private Rigidbody2D _thisBody;
    private BallInWaterController _ballInWaterController;
    private float _lastFrameVelocityMagnitude;
    private bool _canPlayWallHitSound = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _thisBody = GetComponent<Rigidbody2D>();
        _ballInWaterController = GetComponent<BallInWaterController>();
        wooshSound.Play();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 velocity = _thisBody.linearVelocity;
        // Kalau besar vektor kecepatan terlalu besar, maka dipendekin menjadi besar maksimal
        if (velocity.magnitude > maxVelocity)
        {
            _thisBody.linearVelocity = velocity.normalized * maxVelocity;
        }

        // Keluarkan suara angin sesuai kecepatan bola
        float magnitude = _thisBody.linearVelocity.magnitude;
        if (!_ballInWaterController.isInWater) // Tidak di air
        {
            // Suara gesekan dengan udara, dikali Time.timeScale supaya berhenti jika pause
            wooshSound.volume = magnitude / maxVelocity / Mathf.Sqrt(maxVelocity) * Time.timeScale;
            wooshSound.pitch = 1.2f;

            // Kalo tiba-tiba nabrak keluarkan suara nabrak
            if (_lastFrameVelocityMagnitude - magnitude > 1f && _canPlayWallHitSound)
            {
                int randomIndex = Random.Range(0, wallHitSound.Length);
                wallHitSound[randomIndex].volume = (_lastFrameVelocityMagnitude - magnitude) / maxVelocity;
                wallHitSound[randomIndex].pitch = 1;
                wallHitSound[randomIndex].Play();
                _canPlayWallHitSound = false;
                StartCoroutine(WallHitSoundDelay(wallHitSound[randomIndex].clip.length));
            }
        }
        else // Di air 
        {
            // Suara gesekan dengan air, dikali Time.timeScale supaya berhenti jika pause
            wooshSound.volume = magnitude / maxVelocity * Time.timeScale;
            wooshSound.pitch = 0.2f;

            // Kalo tiba-tiba nabrak keluarkan suara nabrak
            if (_lastFrameVelocityMagnitude - magnitude > 1f && _canPlayWallHitSound)
            {
                int randomIndex = Random.Range(0, wallHitSound.Length);
                wallHitSound[randomIndex].volume = (_lastFrameVelocityMagnitude - magnitude) / maxVelocity / maxVelocity;
                wallHitSound[randomIndex].pitch = 0.5f;
                wallHitSound[randomIndex].Play();
                _canPlayWallHitSound = false;
                StartCoroutine(WallHitSoundDelay(wallHitSound[randomIndex].clip.length));
            }
        }

        _lastFrameVelocityMagnitude = magnitude;
    }

    IEnumerator WallHitSoundDelay(float seconds) {
        yield return new WaitForSeconds(seconds);
        _canPlayWallHitSound = true;
    }
}
