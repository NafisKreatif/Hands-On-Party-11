using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsCamera : MonoBehaviour
{
    public float creditsSpeed = 1.0f;
    public RectTransform scrollingContent;
    [SerializeField]
    private BoxCollider2D[] _borderColliders;
    private Camera _camera;
    private Vector2 _cameraPosition;
    private Vector2 _cameraSize;
    private SceneTransitionController _transitionController;
    void Start()
    {
        _camera = Camera.main;
        if (_borderColliders.Length != 4)
        {
            Debug.LogError("Border colliders are not set correctly");
        }
        _cameraSize = new(_camera.orthographicSize * _camera.aspect, _camera.orthographicSize);
        _cameraPosition = _camera.transform.position;
        _transitionController = FindFirstObjectByType<SceneTransitionController>();
    }

    void Update()
    {
        _borderColliders[0].transform.localPosition = _cameraPosition + new Vector2(-_cameraSize.x - 0.5f, 0);
        _borderColliders[1].transform.localPosition = _cameraPosition + new Vector2(_cameraSize.x + 0.5f, 0);
        _borderColliders[2].transform.localPosition = _cameraPosition + new Vector2(0, -_cameraSize.y - 0.5f);
        _borderColliders[3].transform.localPosition = _cameraPosition + new Vector2(0, _cameraSize.y + 0.5f);

        scrollingContent.anchoredPosition += creditsSpeed * Time.deltaTime * Vector2.up;
        if (scrollingContent.anchoredPosition.y > scrollingContent.sizeDelta.y)
        {
            _transitionController.GoToSceneByIndex(0);
        }
    }
}
