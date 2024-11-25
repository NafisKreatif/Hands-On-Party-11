using System.Collections;
using UnityEngine;

public class CameraPeekAction : SceneAction
{
  public override string ActionName => "CameraPeekAction";

  [Tooltip("Duration of the peeking in seconds.")]
  public float stayDuration = 1.0f;
  public float travelDuration = 1.0f;
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
    float targetOrthographicSize = originalOrthographicSize * zoomFactor;
    float time = 0;
    while (time <= travelDuration)
    {
      _camera.orthographicSize = Mathf.Lerp(originalOrthographicSize, targetOrthographicSize, time * time / travelDuration);
      _camera.transform.position = Vector3.Lerp(originalPosition, targetPosition, time * time / travelDuration);
      time += Time.unscaledDeltaTime;
      yield return null;
    }
    
    time = 0;
    _camera.orthographicSize = targetOrthographicSize;
    while (time <= stayDuration)
    {
      _camera.transform.position = targetPosition;
      time += Time.unscaledDeltaTime;
      yield return null;
    }

    time = 0;
    while (time <= travelDuration)
    {
      _camera.orthographicSize = Mathf.Lerp(targetOrthographicSize, originalOrthographicSize, time * time / travelDuration);
      _camera.transform.position = Vector3.Lerp(targetPosition, originalPosition, time * time / travelDuration);
      time += Time.unscaledDeltaTime;
      yield return null;
    }

    _cameraMovement.enabled = true;    
    _camera.orthographicSize = originalOrthographicSize;
    DialogManager.Instance.DialogDone(dialogId);
  }
}