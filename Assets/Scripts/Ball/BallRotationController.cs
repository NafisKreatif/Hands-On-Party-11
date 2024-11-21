using UnityEngine;

public class BallRotationController : MonoBehaviour
{
    public Rigidbody2D[] ballVertices; // Sudut-sudut pada bola
    public float angleSpeedRatio = 1f;
    private Transform _bolaTransform;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _bolaTransform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        float angleMomentum = 0;
        foreach (var vertex in ballVertices)
        {
            angleMomentum += vertex.angularVelocity * angleSpeedRatio;
        }
        angleMomentum /= ballVertices.Length; // Jadiin rata-rata
        angleMomentum *= Time.timeScale; // Jaga-jaga kalo lagi pause
        _bolaTransform.rotation = Quaternion.Euler(0, 0, _bolaTransform.eulerAngles.z + angleMomentum * Time.deltaTime);
    }
}
