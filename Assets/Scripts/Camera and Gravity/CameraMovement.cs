using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public GameObject target;
    private Transform _cameraTransform;
    private Rigidbody2D _cameraRb;
    private float _distanceX;
    private float _distanceY;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _cameraTransform = GetComponent<Transform>();
        _cameraRb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = target.transform.position;
        pos.z = _cameraTransform.position.z;
        _cameraTransform.position = pos;
        // _distanceX = target.transform.position.x - _cameraTransform.position.x;
        // _distanceY = target.transform.position.y - _cameraTransform.position.y;

        // _cameraRb.linearVelocityX = 3 * _distanceX;
        // _cameraRb.linearVelocityY = 3 * _distanceY;
    }
}
