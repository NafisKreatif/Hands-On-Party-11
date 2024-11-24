using System.Collections;
using UnityEngine;

public class CameraPeekAction : SceneAction
{
  public override string ActionName => "CameraPeekAction";

  [Tooltip("Duration of the peeking in seconds.")]
  public float duration = 1.0f;
  public float zoomFactor = 1.0f;
  public GameObject targetObject;

  private CameraMovement _cameraMovement;
  private Camera _camera;

  public void Start()
  {
    _cameraMovement = FindFirstObjectByType<CameraMovement>();
    _camera = _cameraMovement.GetComponent<Camera>();
  }

  public override void Execute(string id)
  {
    dialogId = id;

    StartCoroutine(Peek());
  }

  private IEnumerator Peek()
  {
    _cameraMovement.enabled = false;
    Vector3 originalPosition = _camera.transform.position;
    Vector3 targetPosition = new(targetObject.transform.position.x, targetObject.transform.position.y, originalPosition.z);
    float originalOrthographicSize = _camera.orthographicSize;
    float time = 0;
    while (time <= duration / 2)
    {
      _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, originalOrthographicSize * zoomFactor, time / (duration / 2));      
      _camera.transform.position = Vector3.Lerp(_camera.transform.position, targetPosition, time / (duration / 2));
      time += Time.unscaledDeltaTime;
      yield return null;
    }
    
    time = 0;    
    while (time <= duration / 2)
    {
      _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, originalOrthographicSize, time / (duration / 2));
      _camera.transform.position = Vector3.Lerp(_camera.transform.position, originalPosition, time / (duration / 2));
      time += Time.unscaledDeltaTime;
      yield return null;
    }

    _cameraMovement.enabled = true;
    yield return new WaitForSecondsRealtime(duration / 2);
    DialogManager.Instance.DialogDone(dialogId);
  }
}