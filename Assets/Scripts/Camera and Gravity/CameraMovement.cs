using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform targetTransform;
    public float cameraSpeed = 3f;
    private Transform _cameraTransform;
    private Rigidbody2D _cameraRb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _cameraTransform = GetComponent<Transform>();
        _cameraRb = GetComponent<Rigidbody2D>();

        // Kalau target tranfromnya tidak ada
        // Ambil player sebagai defaultnya
        if (targetTransform == null) {
            targetTransform = GameObject.FindWithTag("Player").transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (targetTransform == null) return;

        float _distanceX = targetTransform.position.x - _cameraTransform.position.x;
        float _distanceY = targetTransform.position.y - _cameraTransform.position.y;

        // Fungsi posisi eksponensial tergantung jarak
        _cameraRb.linearVelocityX = cameraSpeed * _distanceX;
        _cameraRb.linearVelocityY = cameraSpeed * _distanceY;
    }
}
