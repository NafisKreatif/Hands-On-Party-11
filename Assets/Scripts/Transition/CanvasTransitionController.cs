using System;
using UnityEngine;

public class CanvasTransitionController : MonoBehaviour
{
    // Arah transisi dari mana atau ke mana
    [Serializable] public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
    private const float _transitionTime = 1f; // Waktu transisi hingga selesai
    private float _time = 1f; // Total waktu berlangsungnya transisi, 0 -> awal transisi, 1 atau _transitionTime -> akhir transisi
    private Vector3 _initialPosition; // Posisi awal, dari pinggir atau dari tengah
    private Vector3 _targetPosition; // Posisi akhir, ke pinggir atau ke tengah
    private RectTransform _thisTransform; // Untuk diubah posisi UI nya

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _thisTransform = GetComponent<RectTransform>();

        // Kalau nggak ada RectTransform, artinya bukan UI element. Script ini hanya diperuntukkan untuk UI element;
        if (_thisTransform == null)
        {
            Debug.LogError("The GameObject [" + name + "]  is not a UI element!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Masih transisi
        if (_time < _transitionTime)
        {
            Debug.Log("Initial Position : " + _initialPosition.x + ", " + _initialPosition.y + ", " + _initialPosition.z);
            Debug.Log("Target Position : " + _targetPosition.x + ", " + _targetPosition.y + ", " + _targetPosition.z);
            _time += Time.unscaledDeltaTime / _transitionTime;
            SmoothSlideStep(); // Pindahkan posisi selangkah
        }
        // Transisi sudah selesai
        else if (_time > _transitionTime)
        {
            _time = _transitionTime;
            SnapToTargetPosition(); // Memastikan posisi akhir pas dengan targetPosition 
        }
    }
    public void TransitionInFrom(Direction direction)
    {
        _initialPosition = new Vector3(0, 0, _thisTransform.position.z); // Posisi awal nanti disesuaikan dengan arah masuk
        _targetPosition = new Vector3(0, 0, _thisTransform.position.z); // Anggap posisi akhir di (0, 0)
        _time = 0; // Waktu di awal transisi, t = 0

        switch (direction)
        {
            case Direction.Up:
                _initialPosition.y += Camera.main.orthographicSize * 2; // Posisi awal di atas kamera
                break;

            case Direction.Down:
                _initialPosition.y -= Camera.main.orthographicSize * 2; // Posisi awal di bawah kamera
                break;

            case Direction.Right:
                _initialPosition.x += Camera.main.orthographicSize * Camera.main.aspect * 2; // Posisi awal di kanan kamera
                break;

            case Direction.Left:
                _initialPosition.x -= Camera.main.orthographicSize * Camera.main.aspect * 2; // Posisi awal di kiri kamera
                break;

            default:
                break;
        }

        Reveal(); // Pastikan UI element terlihat
    }
    public void TransitionOutTo(Direction direction)
    {
        _initialPosition = new Vector3(0, 0, _thisTransform.position.z); // Anggap posisi awal di (0, 0)
        _targetPosition = new Vector3(0, 0, _thisTransform.position.z); // Posisi akhir nanti disesuaikan dengan arah 
        _time = 0; // Waktu di awal transisi, t = 0

        switch (direction)
        {
            case Direction.Up:
                _targetPosition.y += Camera.main.orthographicSize * 2; // Posisi akhir di atas kamera
                break;

            case Direction.Down:
                _targetPosition.y -= Camera.main.orthographicSize * 2; // Posisi akhir di bawah kamera
                break;

            case Direction.Right:
                _targetPosition.x += Camera.main.orthographicSize * Camera.main.aspect * 2; // Posisi akhir di kanan kamera
                break;

            case Direction.Left:
                _targetPosition.x -= Camera.main.orthographicSize * Camera.main.aspect * 2; // Posisi akhir di kiri kamera
                break;

            default:
                break;
        }

        Reveal(); // Pastikan UI element terlihat
    }

    // Perubahan posisi secara smooth
    private void SmoothSlideStep()
    {
        _thisTransform.position = Vector3.Lerp(_initialPosition, _targetPosition, _time * _time * _time * (_time * (6f * _time - 15f) + 10f));
    }
    // Paskan posisi dengan posisi akhir yang dinginkan
    private void SnapToTargetPosition()
    {
        _thisTransform.position = _targetPosition;
        // Kalau posisi akhir bukan di tengah, alias bukan dari transisi masuk
        if (_targetPosition != new Vector3(0, 0, _thisTransform.position.z))
        {
            Hide();
        }
    }

    // Perlihatkan UI dengan membuat localScale menjadi 1
    public void Reveal()
    {
        _thisTransform.localScale = new Vector3(1, 1, 1);
    }
    // Hilangkan UI dengan membuat localScale menjadi 0
    public void Hide()
    {
        _thisTransform.localScale = new Vector3(0, 0, 0);
    }
}
