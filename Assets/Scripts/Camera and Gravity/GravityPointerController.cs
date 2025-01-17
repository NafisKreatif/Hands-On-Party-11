using UnityEngine;

class GravityPointerController : MonoBehaviour
{
    void Update()
    {
        transform.localRotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(Physics2D.gravity.y, Physics2D.gravity.x));
    }
}