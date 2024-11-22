using UnityEngine;

public class BallVelocityController : MonoBehaviour
{
    public float maxVelocity = 7f;
    private Rigidbody2D _thisBody;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _thisBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 velocity = _thisBody.linearVelocity;
        float magnitude = velocity.magnitude;
        // Kalau besar vektor kecepatan terlalu besar, maka dipendekin menjadi besar maksimal
        if (magnitude > maxVelocity)
        {
            _thisBody.linearVelocity = velocity.normalized * maxVelocity;
        }
    }
}
