using UnityEngine;
using UnityEngine.UIElements;

public class BackgroundParallax : MonoBehaviour
{
    public Rigidbody2D cameraRb;
    public float parallaxAmount; // Choose the value. x>1: faster than camera (in front), 0<x<1: slower than camera (behind)

    void Start()
    {
        cameraRb = GameObject.Find("Main Camera").GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        transform.position = new Vector3(cameraRb.transform.position.x, cameraRb.transform.position.y, transform.position.z);
    }
}
