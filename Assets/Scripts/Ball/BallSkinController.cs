using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;

public class BallSkinController : MonoBehaviour
{
    public Transform[] ballVertices; // Sudut-sudut pada bola
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
        for (int i = 0; i < ballVertices.Length; i++)
        {
            Vector2 vertexPosition = ballVertices[i].localPosition;
            Vector2 realPosition = new(0, 0);

            // Hitung posisi titik sudut spline yang disesuaikan dengan rotasi teksturnya
            float sudut = -_spriteTransform.eulerAngles.z / 180 * Mathf.PI; // Pake minus buat dapat inversi rotasi yang terjadi
            realPosition.x = Mathf.Cos(sudut) * vertexPosition.x - Mathf.Sin(sudut) * vertexPosition.y;
            realPosition.y = Mathf.Sin(sudut) * vertexPosition.x + Mathf.Cos(sudut) * vertexPosition.y;

            // Hitung vector arah ke pusat dengan besar 1
            Vector2 centerPosition = new(0, 0);
            Vector2 VectorToCenter = (centerPosition - realPosition).normalized;

            float colliderRadius = ballVertices[i].GetComponent<CircleCollider2D>().radius;
            try
            {
                // Taroh titik sudut di posisi yang sesuai
                _spriteShape.spline.SetPosition(i, realPosition - VectorToCenter * colliderRadius);
            }
            catch
            {
                // Ga tahu ini berhasil apa nggak. Buat jaga-jaga saja. Kalo eror, titik sudut pada spline dijauhkan dengan menambah offset
                Debug.Log("Spline too close to each other!");
                _spriteShape.spline.SetPosition(i, realPosition - VectorToCenter * (colliderRadius * SPLINEOFFSET));
            }

            // Sesuaikan kurva sudut dengan mengedit arah tangen kiri dan kanan
            _spriteShape.spline.SetLeftTangent(i, -Vector2.Perpendicular(VectorToCenter) * colliderRadius);
            _spriteShape.spline.SetRightTangent(i, Vector2.Perpendicular(VectorToCenter) * colliderRadius);
        }
    }
}
