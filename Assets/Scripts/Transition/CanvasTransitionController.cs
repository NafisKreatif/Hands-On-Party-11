using UnityEngine;

public class CanvasTransitionController : MonoBehaviour
{
    public float transitionTime = 1f;
    private float _time = 1f;
    private Vector3 _initialPosition;
    private Vector3 _targetPosition;
    private RectTransform _thisTransform;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _thisTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_time < transitionTime)
        {
            Debug.Log("Initial Position : " + _initialPosition.x + ", " + _initialPosition.y + ", " + _initialPosition.z);
            Debug.Log("Target Position : " + _targetPosition.x + ", " + _targetPosition.y + ", " + _targetPosition.z);
            _time += Time.deltaTime / transitionTime;
            SmoothSlideStep();
        }
        else if (_time > transitionTime)
        {
            _time = transitionTime;
            SnapToPosition();
        }
    }
    public void TransitionInFromUp()
    {
        _initialPosition = new Vector3(0, 0, _thisTransform.position.z);
        _targetPosition = new Vector3(0, 0, _thisTransform.position.z); // target position nanti disesuaikan dengan arah 
        _time = 0;

        _initialPosition.y += Camera.main.orthographicSize * 2;
        Reveal();
    }
    public void TransitionInFromDown()
    {
        _initialPosition = new Vector3(0, 0, _thisTransform.position.z); ;
        _targetPosition = new Vector3(0, 0, _thisTransform.position.z); // target position nanti disesuaikan dengan arah 
        _time = 0;

        _initialPosition.y -= Camera.main.orthographicSize * 2;
        Reveal();
    }
    public void TransitionInFromRight()
    {
        _initialPosition = new Vector3(0, 0, _thisTransform.position.z);
        _targetPosition = new Vector3(0, 0, _thisTransform.position.z); // target position nanti disesuaikan dengan arah 
        _time = 0;

        _initialPosition.x += Camera.main.orthographicSize * Camera.main.aspect * 2;
        Reveal();
    }
    public void TransitionInFromLeft()
    {
        _initialPosition = new Vector3(0, 0, _thisTransform.position.z);
        _targetPosition = new Vector3(0, 0, _thisTransform.position.z); // target position nanti disesuaikan dengan arah 
        _time = 0;

        _initialPosition.x -= Camera.main.orthographicSize * Camera.main.aspect * 2;
        Reveal();
    }

    public void TransitionOutToUp()
    {
        _initialPosition = new Vector3(0, 0, _thisTransform.position.z);
        _targetPosition = new Vector3(0, 0, _thisTransform.position.z); // target position nanti disesuaikan dengan arah 
        _time = 0;

        _targetPosition.y += Camera.main.orthographicSize * 2;
    }
    public void TransitionOutToDown()
    {
        _initialPosition = new Vector3(0, 0, _thisTransform.position.z);
        _targetPosition = new Vector3(0, 0, _thisTransform.position.z); // target position nanti disesuaikan dengan arah 
        _time = 0;

        _targetPosition.y -= Camera.main.orthographicSize * 2;
    }
    public void TransitionOutToRight()
    {
        _initialPosition = new Vector3(0, 0, _thisTransform.position.z);
        _targetPosition = new Vector3(0, 0, _thisTransform.position.z); // target position nanti disesuaikan dengan arah 
        _time = 0;

        _targetPosition.x += Camera.main.orthographicSize * Camera.main.aspect * 2;
    }
    public void TransitionOutToLeft()
    {
        _initialPosition = new Vector3(0, 0, _thisTransform.position.z);
        _targetPosition = new Vector3(0, 0, _thisTransform.position.z); // target position nanti disesuaikan dengan arah 
        _time = 0;

        _targetPosition.x -= Camera.main.orthographicSize * Camera.main.aspect * 2;
    }
    private void SmoothSlideStep()
    {
        _thisTransform.position = Vector3.Lerp(_initialPosition, _targetPosition, _time * _time * _time * (_time * (6f * _time - 15f) + 10f));
    }
    private void SnapToPosition()
    {
        _thisTransform.position = _targetPosition;
        if (_targetPosition != new Vector3(0, 0, _thisTransform.position.z))
        {
            Hide();
        }
    }
    public void Reveal()
    {
        _thisTransform.localScale = new Vector3(1, 1, 1);
    }
    public void Hide()
    {
        _thisTransform.localScale = new Vector3(0, 0, 0);
    }
}
