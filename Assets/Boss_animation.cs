using UnityEngine;

public class Boss_animation : MonoBehaviour
{
    public Transform leftWing;
    public Transform rightWing;
    public Transform Head;
    public Vector3 StartHead;
    public float Boss_speed = 0.3f;
    public float flapSpeed;
    public float flapAngle;
    public float ZflapAngle;
    public float HeadRotationSpeed;
    public float HeadAngle;
    private float Rotation;
    private float ZRotation;
    void Start(){
        flapAngle = 24;
        flapSpeed = 0.8f;
        ZflapAngle = 8;
        HeadRotationSpeed = 0.8f;
        HeadAngle = -6f;
        Boss_speed = 0.3f; //Change the speed if its too fast / slow
    }
    void Update()
    {
        Rotation = Mathf.Sin(Time.time * flapSpeed) * flapAngle;
        ZRotation = Mathf.Sin(Time.time * flapSpeed) * ZflapAngle;
        Head.localRotation= Quaternion.Euler(0,0,Mathf.Sin(Time.time*HeadRotationSpeed) * HeadAngle);
        leftWing.localRotation = Quaternion.Euler(Rotation, 0, ZRotation);
        rightWing.localRotation = Quaternion.Euler(-Rotation, 0, -ZRotation);
        transform.position += new Vector3(0,Boss_speed*Time.deltaTime,0);
    }
}
