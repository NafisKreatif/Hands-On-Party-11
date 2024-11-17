using UnityEngine;

public class BallRotateController : MonoBehaviour
{
    public Rigidbody2D[] titikSudut; // Sudut-sudut pada bola
    public float damping = 1000f;
    private Transform _bolaTransform;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _bolaTransform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        float totalMomentumSudut = 0;
        foreach (var sudut in titikSudut) {
            totalMomentumSudut += sudut.angularVelocity / damping;
        }
        Debug.Log(totalMomentumSudut / titikSudut.Length);
        _bolaTransform.rotation = Quaternion.Euler(0, 0, _bolaTransform.eulerAngles.z + totalMomentumSudut / titikSudut.Length);
    }
}
