using System.Collections;
using UnityEngine;

public class TransformAction : SceneAction
{
  public override string ActionName => "TransformAction";

  public enum EasingFunction { Linear, EaseIn, EaseOut, EaseInOut }

  [Tooltip("Duration of the transformation in seconds.")]
  public float duration = 1.0f;
  [Tooltip("Easing function for the transformation. Linear = t, EaseIn = 1 - cos(t * π/2), EaseOut = sin(t * π/2), EaseInOut = t^3(t(6t - 15) + 10)")]
  public EasingFunction[] easingFunctions;
  [Tooltip("Transform states to be transformed to, in order. Please make the positions fixed")]
  public Transform[] transformStates;
  public GameObject targetObject;
  public GameObject targetObjectPrefab;

  public override void Execute(string id)
  {
    dialogId = id;
    StartCoroutine(Transform());
  }

  private IEnumerator Transform()
  {
    if (targetObjectPrefab != null) targetObject = Instantiate(targetObjectPrefab, transformStates[0].position, transformStates[0].rotation);
    for (int i = 0; i < transformStates.Length; i++)
    {
      Transform target = transformStates[i];
      yield return StartCoroutine(TransformTo(target, easingFunctions[i]));
    }
    if (targetObjectPrefab != null) Destroy(targetObject);

    DialogManager.Instance.DialogDone(dialogId);
  }

  // Coroutine for transforming the target object to the target transform.
  private IEnumerator TransformTo(Transform target, EasingFunction easingFunction = EasingFunction.Linear)
  {
    float time = 0;
    float partialDuration = duration / transformStates.Length;
    Vector3 endPosition = target.position;
    Quaternion endRotation = target.rotation;
    Vector3 endScale = target.localScale;
    Vector3 startPosition = targetObject.transform.position;
    Quaternion startRotation = targetObject.transform.rotation;
    Vector3 startScale = targetObject.transform.localScale;

    while (time < partialDuration)
    {
      targetObject.transform.position = Vector3Lerp(startPosition, endPosition, time / partialDuration, easingFunction);
      targetObject.transform.rotation = QuaternionLerp(startRotation, endRotation, time / partialDuration, easingFunction);
      targetObject.transform.localScale = Vector3Lerp(startScale, endScale, time / partialDuration, easingFunction);
      time += Time.unscaledDeltaTime;
      yield return null;
    }

    targetObject.transform.position = endPosition;
    targetObject.transform.rotation = endRotation;
    targetObject.transform.localScale = endScale;
  }

  private Vector3 Vector3Lerp(Vector3 start, Vector3 end, float time, EasingFunction easingFunction = EasingFunction.Linear)
  {
    return easingFunction switch
    {
      EasingFunction.Linear => Vector3.Lerp(start, end, time),
      EasingFunction.EaseIn => Vector3.Lerp(start, end, 1f - Mathf.Cos(time * Mathf.PI * 0.5f)),
      EasingFunction.EaseOut => Vector3.Lerp(start, end, Mathf.Sin(time * Mathf.PI * 0.5f)),
      EasingFunction.EaseInOut => Vector3.Lerp(start, end, time * time * time * (time * (6f * time - 15f) + 10f)),
      _ => Vector3.zero,
    };
  }

  private Quaternion QuaternionLerp(Quaternion start, Quaternion end, float time, EasingFunction easingFunction = EasingFunction.Linear)
  {
    return easingFunction switch
    {
      EasingFunction.Linear => Quaternion.Lerp(start, end, time),
      EasingFunction.EaseIn => Quaternion.Lerp(start, end, 1f - Mathf.Cos(time * Mathf.PI * 0.5f)),
      EasingFunction.EaseOut => Quaternion.Lerp(start, end, Mathf.Sin(time * Mathf.PI * 0.5f)),
      EasingFunction.EaseInOut => Quaternion.Lerp(start, end, time * time * time * (time * (6f * time - 15f) + 10f)),
      _ => Quaternion.identity,
    };
  }
}