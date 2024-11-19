using UnityEngine;

public class BackgroundParallax : MonoBehaviour
{
    public Rigidbody2D cameraRb;
    public Rigidbody2D backgroundRb;
    public float parallaxAmount;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cameraRb = GameObject.Find("Main Camera").GetComponent<Rigidbody2D>();
        backgroundRb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        backgroundRb.linearVelocityX = parallaxAmount * cameraRb.linearVelocityX;
        backgroundRb.linearVelocityY = parallaxAmount * cameraRb.linearVelocityY;
    }
}
