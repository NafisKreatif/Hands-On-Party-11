using System.Collections;
using UnityEngine;

public class CameraPeekAction : SceneAction
{
  public override string ActionName => "CameraPeekAction";

  [Tooltip("Duration of the peeking in seconds.")]
  public float duration = 1.0f;
  public GameObject targetObject;

  private CameraMovement _cameraMovement;

  public void Start()
  {
    _cameraMovement = FindFirstObjectByType<CameraMovement>();
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

    yield return new WaitForSeconds(duration / 2);
    _cameraMovement.target = tempTarget;

    yield return new WaitForSeconds(duration / 2);
    DialogManager.Instance.DialogDone(dialogId);
  }
}