using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;

public class BallSkinController : MonoBehaviour
{
    public Transform[] titikSudut; // Sudut-sudut pada bola
    private SpriteShapeController _spriteShape; // Untuk bentuk bolanya
    private Transform _spriteTransform; // This transform (padahal ga perlu getComponent, but anyway)
    private const float SPLINEOFFSET = 1.2f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _spriteShape = GetComponent<SpriteShapeController>();
        _spriteTransform = GetComponent<Transform>();
        UpdateVertices();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateVertices();
    }

    // Update bentuk dari bola menyesuaikan titik-titik sudut pada soft bodynya
    private void UpdateVertices()
    {
        for (int i = 0; i < titikSudut.Length; i++)
        {
            Vector2 posisiSudut = titikSudut[i].localPosition;
            Vector2 posisiSebenarnya = new(0, 0);

            // Hitung posisi titik sudut spline yang disesuaikan dengan rotasi teksturnya
            float sudut = -_spriteTransform.eulerAngles.z / 180 * Mathf.PI; // Pake minus buat dapat inversi rotasi yang terjadi
            posisiSebenarnya.x = Mathf.Cos(sudut) * posisiSudut.x - Mathf.Sin(sudut) * posisiSudut.y;
            posisiSebenarnya.y = Mathf.Sin(sudut) * posisiSudut.x + Mathf.Cos(sudut) * posisiSudut.y;
            
            // Hitung vector arah ke pusat dengan besar 1
            Vector2 posisiPusat = new(0, 0);
            Vector2 arahKePusat = (posisiPusat - posisiSebenarnya).normalized;

            float jariJariCollider = titikSudut[i].GetComponent<CircleCollider2D>().radius;
            try {
                // Taroh titik sudut di posisi yang sesuai
                _spriteShape.spline.SetPosition(i, posisiSebenarnya - arahKePusat * jariJariCollider);
            }
            catch {
                // Ga tahu ini berhasil apa nggak. Buat jaga-jaga saja. Kalo eror, titik sudut pada spline dijauhkan dengan menambah offset
                Debug.Log("Spline too close to each other!");
                _spriteShape.spline.SetPosition(i, posisiSebenarnya - arahKePusat * (jariJariCollider * SPLINEOFFSET));
            }

            // Sesuaikan kurva sudut dengan mengedit arah tangen kiri dan kanan
            _spriteShape.spline.SetLeftTangent(i, -Vector2.Perpendicular(arahKePusat) * jariJariCollider);
            _spriteShape.spline.SetRightTangent(i, Vector2.Perpendicular(arahKePusat) * jariJariCollider);
        }
    }
}
