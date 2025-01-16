using System;
using UnityEngine;

public class Boss_animation : MonoBehaviour
{
    public Transform leftWing;
    public Transform rightWing;
    public Transform head;
    public Vector3 startHead;
    public float bossSpeed = 0.3f;
    public float startAccelerateDistance = 5f;
    public float actualSpeed = 0.3f;
    public float flapSpeed = 24f;
    public float xFlapAngle = 0.8f;
    public float zFlapAngle = 8f;
    public float headRotationSpeed = 0.8f;
    public float headAngle = -6f;
    private float _xRotation;
    private float _zRotation;
    private Transform _playerTranform;
    void Start()
    {
        _playerTranform = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void Update()
    {
        Vector2 bossPosition = transform.position;
        Vector2 playerPosition = _playerTranform.position;
        Debug.Log("Boss Position: " + bossPosition.x + ", " + bossPosition.y);
        Debug.Log("Player Position: " + playerPosition.x + ", " + playerPosition.y);
        Debug.Log((playerPosition - bossPosition).magnitude);
        actualSpeed = Math.Max(bossSpeed, ((playerPosition - bossPosition).magnitude - startAccelerateDistance + 1) * bossSpeed);
        _xRotation = Mathf.Sin(Time.time * flapSpeed) * xFlapAngle;
        _zRotation = Mathf.Sin(Time.time * flapSpeed) * zFlapAngle;
        head.localRotation = Quaternion.Euler(0, 0, Mathf.Sin(Time.time * headRotationSpeed) * headAngle);
        leftWing.localRotation = Quaternion.Euler(_xRotation, 0, _zRotation);
        rightWing.localRotation = Quaternion.Euler(-_xRotation, 0, -_zRotation);
        transform.position += new Vector3(0, actualSpeed * Time.deltaTime, 0);
    }

    public void SetSpeed(float newSpeed)
    {
        bossSpeed = newSpeed;
    }
    public void SetWalking(bool isWalking) {
        
    }
}
