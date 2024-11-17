using TMPro;
using UnityEngine;

public class MenangController : MonoBehaviour
{
    public GameObject teksMenang;
    public GameObject teksHint;
    public Transform gameCamera;
    void Start() {
        gameCamera = GameObject.Find("Main Camera").transform;
    }
    void Update() {
        if (gameCamera.eulerAngles.z != 0) {
            teksHint.SetActive(false);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            teksMenang.SetActive(true);
        }
    }
}
