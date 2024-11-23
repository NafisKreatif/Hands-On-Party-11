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
    GameObject tempTarget = _cameraMovement.target;
    _cameraMovement.target = targetObject;
    float originalOrthographicSize = _camera.orthographicSize;
    float time = 0;
    while (time <= duration / 2)
    {
      _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, originalOrthographicSize * zoomFactor, time / (duration / 2));      
      time += Time.deltaTime;
      yield return null;
    }

    _cameraMovement.target = tempTarget;
    time = 0;
    while (time <= duration / 2)
    {
      _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, originalOrthographicSize, time / (duration / 2));
      time += Time.deltaTime;
      yield return null;
    }


    yield return new WaitForSeconds(duration / 2);
    DialogManager.Instance.DialogDone(dialogId);
  }
}