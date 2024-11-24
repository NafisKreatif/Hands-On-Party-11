using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ScreenShakeAction : SceneAction
{
    public float duration = 1.0f;
    public float magnitude = 0.25f;
    public float dampingSpeed = 1.0f;
    private Transform _cameraTransform;
    private Vector3 _initialPos;

    private void Start() {
        _cameraTransform = Camera.main.transform;
        _initialPos = _cameraTransform.localPosition;
    }

    public override string ActionName => "ScreenShakeAction";
    public override void Execute(string id)
    {
        dialogId = id;

        StartCoroutine(Shake());
    }

    IEnumerator Shake() {
        float time = 0.0f;
        while (time <= duration) {
            _cameraTransform.localPosition += Random.insideUnitSphere * magnitude;
            time += Time.unscaledDeltaTime;
            yield return null;
        }
        transform.localPosition = _initialPos;

        DialogManager.Instance.DialogDone(dialogId);
    }
}
