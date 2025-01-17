using UnityEngine;

public class BackgroundParallax : MonoBehaviour
{
    private Rigidbody2D _cameraRb;
    public float parallaxAmount; // Choose the value. x>1: faster than camera (in front), 0<x<1: slower than camera (behind)
    private Vector3 _initialPosition;

    void Start()
    {
        _cameraRb = Camera.main.GetComponent<Rigidbody2D>();
        _initialPosition = new Vector3(_cameraRb.transform.position.x, _cameraRb.transform.position.y, transform.position.z);
        transform.position = new Vector3(_initialPosition.x, _initialPosition.y, transform.position.z); // Paskan dengan posisi awal kamera
    }

    void Update()
    {
        Vector3 deltaPosition = new(_cameraRb.transform.position.x - _initialPosition.x, _cameraRb.transform.position.y - _initialPosition.y, 0);
        transform.position = _initialPosition + deltaPosition * parallaxAmount;
    }
}
