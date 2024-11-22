using UnityEngine;

public class BallKillerController : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.SendMessage("Die");
        }
    }
}
