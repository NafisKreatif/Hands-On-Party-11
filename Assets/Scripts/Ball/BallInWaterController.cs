using UnityEngine;

public class BallInWaterController : MonoBehaviour
{
    public ParticleSystem bubbleParticleSystem;
    public bool isInWater = false;
    public float waterLinearDamping = 20f;

    private bool _isWaterPhysicsEnabled = false;
    private Rigidbody2D _thisBody;
    private float _initialDamping;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _thisBody = GetComponent<Rigidbody2D>();
        _initialDamping = _thisBody.linearDamping;
    }

    // Update is called once per frame
    void Update()
    {
        // Kalo di air tapi physicsnya belum diubah
        if (isInWater && !_isWaterPhysicsEnabled)
        {
            IntoWater(); // Masukkan dalam air
        }
        if (!isInWater && _isWaterPhysicsEnabled)
        {
            OutOfWater(); // Keluarkan dari air
        }
        // Kalau dalam air dan cukup cepat untuk membuat bubble
        if (isInWater && _thisBody.linearVelocity.magnitude > 0.1f)
        {
            if (!bubbleParticleSystem.isPlaying)
            {
                bubbleParticleSystem.Play();
            }

            float angle = -Mathf.Atan2(Physics2D.gravity.y, Physics2D.gravity.x) / Mathf.PI * 180 + 90;
            var shape = bubbleParticleSystem.shape;
            shape.rotation = new Vector3(0, 0, angle);
        }
        else // Tidak cukup
        {
            if (bubbleParticleSystem.isPlaying) {
                bubbleParticleSystem.Stop();
            }
        }
    }
    // Kalo masuk air
    public void IntoWater()
    {
        isInWater = true;
        _isWaterPhysicsEnabled = true;
        _thisBody.linearDamping = waterLinearDamping; // Kecepatan cepet berkurang karena ada gaya oleh air
    }
    // Kalo keluar air
    public void OutOfWater()
    {
        isInWater = false;
        _isWaterPhysicsEnabled = false;
        _thisBody.linearDamping = _initialDamping; // Kembali ke gaya gesek dengan udara
    }
}
