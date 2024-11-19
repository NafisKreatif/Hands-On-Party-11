using UnityEngine;

public class BackgroundParallax : MonoBehaviour
{
    public Rigidbody2D cameraRb;
    public Rigidbody2D backgroundRb;
    public float parallaxAmount; // Choose the value. x>1: faster than camera (in front), 0<x<1: slower than camera (behind)

    void Start()
    {
        cameraRb = GameObject.Find("Main Camera").GetComponent<Rigidbody2D>();
        backgroundRb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        backgroundRb.linearVelocityX = parallaxAmount * cameraRb.linearVelocityX;
        backgroundRb.linearVelocityY = parallaxAmount * cameraRb.linearVelocityY;
    }
}
