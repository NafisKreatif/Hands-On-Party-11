using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;

public class BallSkin : MonoBehaviour
{
    public float besarTangen = 1f;
    public Transform pusat;
    public Transform[] points;
    private SpriteShapeController spriteShape;
    private const float splineOffset = 0.5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    { 
        spriteShape = GetComponent<SpriteShapeController>();
        UpdateVertices();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateVertices();
    }

    private void UpdateVertices()
    {
        for (int i = 0; i < points.Length; i++)
        {
            Vector2 posisiPoint = points[i].localPosition;
            Vector2 posisiPusat = pusat.localPosition;
            Vector2 arahKePusat = (posisiPusat - posisiPoint).normalized;
            float jariJariCollider = points[i].GetComponent<CircleCollider2D>().radius;
            try {
                spriteShape.spline.SetPosition(i, posisiPoint - arahKePusat * jariJariCollider);
            }
            catch {
                Debug.Log("Spline too close to each other!");
                spriteShape.spline.SetPosition(i, posisiPoint - arahKePusat * (jariJariCollider));
            }
            spriteShape.spline.SetLeftTangent(i, arahKePusat.Perpendicular1() * besarTangen);
            spriteShape.spline.SetRightTangent(i, arahKePusat.Perpendicular2() * besarTangen);
        }
    }
}
