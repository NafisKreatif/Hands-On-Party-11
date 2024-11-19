using UnityEngine;

public class Hostile_mob_movements : MonoBehaviour
{
    // function LeftRightMovement menggerakkan gameobj ke kiri-kanan-kiri-kanan... parameternya 
    // time dan distance, time = waktu yang dibutuhkan untuk mencapai distance, distance = jarak yang ingin ditempuh saat ke kiri maupun kanan
    // UpDownMovemenet juga sama aja cuma gerakannya atas bwah
    // kecepatannya angular, kalau mau linear bisa ganti speednya jadi linear-sinusoidal function (aku gatau itu ada ato gak tp you get the idea)
    public float speed;
    public Rigidbody2D rb;
    private Vector3 startPosition; 
    void Start()
    {
        startPosition = transform.position;
    }
    void Update()
    {
        float xOffset = LeftRightMovement(5,5);
        float yOffset = UpDownMovement(5,5);   
        transform.position = new Vector3(startPosition.x + xOffset, startPosition.y + yOffset, startPosition.z);
    }
    float LeftRightMovement(float time,float distance)
    {
        float speed = 2 * Mathf.PI / time;
        float offset = Mathf.Sin(Time.time * speed) * distance;
        return offset;
    }
    float UpDownMovement(float time,float distance)
    {
        float speed = 2 * Mathf.PI / time; 
        float offset = Mathf.Sin(Time.time * speed) * distance;
        return offset;
    }
    }