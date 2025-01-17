using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class DirectionHintController : MonoBehaviour
{
    private Camera _mainCamera;
    public Transform target;
    private Transform _thisTranform;
    private Vector3 _initialScale;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _mainCamera = Camera.main;
        _thisTranform = GetComponent<Transform>();
        _initialScale = _thisTranform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 positionOnViewport = _mainCamera.WorldToViewportPoint(target.position);
        // Jika di target di dalam kamera
        if (positionOnViewport.x >= 0 && positionOnViewport.x <= 1 && positionOnViewport.y >= 0 && positionOnViewport.y <= 1)
        {
            _thisTranform.localScale = new(0, 0, 1);
        }
        else
        {
            // Kecilkan scale jika dekat dengan kamera
            float newScale = 1;
            newScale *= Math.Max(Mathf.Abs(positionOnViewport.x - 0.5f) - 0.5f, Mathf.Abs(positionOnViewport.y - 0.5f) - 0.5f);
            newScale = Mathf.Sqrt(newScale); // Supaya pengecilannya kuadratik
            _thisTranform.localScale = _initialScale * Mathf.Min(1, newScale); // Jangan sampai memperbesar

            // Harus dirotate dengan kamera vektornya
            float angle = _mainCamera.transform.eulerAngles.z / 180 * Mathf.PI;
            Vector3 rotatedScale = _thisTranform.lossyScale;
            rotatedScale.x = Mathf.Cos(angle) * _thisTranform.lossyScale.x - Mathf.Sin(angle) * _thisTranform.lossyScale.y;
            rotatedScale.y = Mathf.Sin(angle) * _thisTranform.lossyScale.x + Mathf.Cos(angle) * _thisTranform.lossyScale.y;

            // Offset posisi di kamera
            Vector2 scaleOnViewport = _mainCamera.WorldToViewportPoint(rotatedScale + _mainCamera.transform.position);
            scaleOnViewport -= new Vector2(0.5f, 0.5f);

            // Normalisasi posisi
            if (positionOnViewport.x < 0)
            {
                positionOnViewport.x = 0 + scaleOnViewport.x;
            }
            if (positionOnViewport.x > 1)
            {
                positionOnViewport.x = 1 - scaleOnViewport.x;
            }
            if (positionOnViewport.y < 0)
            {
                positionOnViewport.y = 0 + scaleOnViewport.y;
            }
            if (positionOnViewport.y > 1)
            {
                positionOnViewport.y = 1 - scaleOnViewport.y;
            }
            Vector3 worldPointPos = _mainCamera.ViewportToWorldPoint(positionOnViewport);
            _thisTranform.position = new Vector3(worldPointPos.x, worldPointPos.y, _thisTranform.position.z);
            // Rotasikan petunjuk sesuai arah target
            Vector2 directionTowardTarget = target.position - _thisTranform.position;
            float targetAngle = Mathf.Atan2(directionTowardTarget.y, directionTowardTarget.x) / Mathf.PI * 180;
            targetAngle -= 90;
            _thisTranform.rotation = Quaternion.Euler(0, 0, targetAngle);
        }
    }
}
