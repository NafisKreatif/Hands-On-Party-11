using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public GameObject cameraPos;
    public GameObject target;
    public float distanceX;
    public float distanceY;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        target = GameObject.Find("CameraTarget");
        cameraPos = GameObject.Find("Main Camera");
    }

    // Update is called once per frame
    void Update()
    {
        distanceX = target.transform.position.x - cameraPos.transform.position.x;
        distanceY = target.transform.position.y - cameraPos.transform.position.y;

        rb.linearVelocityX = 3*distanceX;
        rb.linearVelocityY = 3*distanceY;
        
    }
}
