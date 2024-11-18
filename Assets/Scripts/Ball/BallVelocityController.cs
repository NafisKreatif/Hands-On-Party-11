using UnityEngine;

public class BallVelocityController : MonoBehaviour
{
    public float lajuMaksimal = 7f;
    private Rigidbody2D _thisBody;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _thisBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 laju = _thisBody.linearVelocity;
        float besarLaju = laju.magnitude;
        if (besarLaju > lajuMaksimal)
        {
            _thisBody.linearVelocity = laju.normalized * lajuMaksimal;
        }
    }
}
