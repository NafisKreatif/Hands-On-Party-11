using UnityEngine;
using UnityEngine.UIElements;

public class BackgroundParallax : MonoBehaviour
{
    public Rigidbody2D cameraRb;
    private Rigidbody2D _backgroundRb;
    private Transform _backgroundTransform;
    private SpriteRenderer _backgroundSprite;
    private float _distanceX;
    private float _distanceY;
    public float parallaxAmount; // Choose the value. x>1: faster than camera (in front), 0<x<1: slower than camera (behind)

    void Start()
    {
        cameraRb = GameObject.Find("Main Camera").GetComponent<Rigidbody2D>();
        _backgroundRb = GetComponent<Rigidbody2D>();
        _backgroundSprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Debug.Log(Camera.main.orthographicSize);
        // Debug.Log(Camera.main.orthographicSize * Camera.main.aspect);
        // Debug.Log(_backgroundSprite.sprite.bounds.size.y);
        // Debug.Log(cameraRb.transform.position.x - transform.position.x);

        _distanceX = cameraRb.transform.position.x - transform.position.x;
        _distanceY = cameraRb.transform.position.y - transform.position.y;

        // if (_distanceX >= _backgroundSprite.sprite.bounds.size.x - Camera.main.orthographicSize * Camera.main.aspect)
        // {
        //     transform.position = new Vector3(cameraRb.transform.position.x - (_backgroundSprite.sprite.bounds.size.x - Camera.main.orthographicSize * Camera.main.aspect), transform.position.y, 0f);
        //     _backgroundRb.linearVelocityX = cameraRb.linearVelocityX;
        // }
        // else if (_distanceX <= -(_backgroundSprite.sprite.bounds.size.x - Camera.main.orthographicSize * Camera.main.aspect))
        // {
        //     transform.position = new Vector3(cameraRb.transform.position.x + (_backgroundSprite.sprite.bounds.size.x - Camera.main.orthographicSize * Camera.main.aspect), transform.position.y, 0f);
        //     _backgroundRb.linearVelocityX = cameraRb.linearVelocityX;
        // }
        // else
        // {
            _backgroundRb.linearVelocityX = parallaxAmount * cameraRb.linearVelocityX;
        // }

        // if (_distanceY >= _backgroundSprite.sprite.bounds.size.y - Camera.main.orthographicSize)
        // {
        //     transform.position = new Vector3(transform.position.x, cameraRb.transform.position.y - (_backgroundSprite.sprite.bounds.size.y - Camera.main.orthographicSize), 0f);
        //     _backgroundRb.linearVelocityY = cameraRb.linearVelocityY;
        // }
        // else if (_distanceY <= -(_backgroundSprite.sprite.bounds.size.y - Camera.main.orthographicSize))
        // {
        //     transform.position = new Vector3(transform.position.x, cameraRb.transform.position.y + (_backgroundSprite.sprite.bounds.size.y - Camera.main.orthographicSize), 0f);
        //     _backgroundRb.linearVelocityY = cameraRb.linearVelocityY;
        // }
        // else
        // {
            _backgroundRb.linearVelocityY = parallaxAmount * cameraRb.linearVelocityY;
        // }
    }
}
