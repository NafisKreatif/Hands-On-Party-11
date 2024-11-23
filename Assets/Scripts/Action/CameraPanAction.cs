using System.Collections;
using UnityEngine;

public class CameraPanAction : SceneAction
{
    public float duration = 2.0f;
    public Transform targetTransform;

    public override string ActionName => "CameraPanAction";

    private CameraMovement _cameraMovement;
    private Transform _cameraTransform;

    private void Start()
    {
        _cameraMovement = FindFirstObjectByType<CameraMovement>();
        _cameraTransform = _cameraMovement.transform;
    }

    public override void Execute(string id)
    {
        dialogId = id;

        StartCoroutine(Pan());
    }

    private IEnumerator Pan()
    {
        _cameraMovement.enabled = false;
        Vector3 originalPosition = _cameraTransform.position;

        float time = 0;
        while (time <= duration)
        {
            _cameraTransform.position = Vector3.Lerp(originalPosition, targetTransform.position, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        _cameraMovement.enabled = true;
        DialogManager.Instance.DialogDone(dialogId);
    }
}
