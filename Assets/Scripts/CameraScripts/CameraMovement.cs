using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Rigidbody2D cameraRb;
    public GameObject targetCamera;
    public float distanceX;
    public float distanceY;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        targetCamera = GameObject.Find("CameraTarget");
        cameraRb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        distanceX = targetCamera.transform.position.x - transform.position.x;
        distanceY = targetCamera.transform.position.y - transform.position.y;

        cameraRb.linearVelocityX = 3*distanceX;
        cameraRb.linearVelocityY = 3*distanceY;
        
    }
}
