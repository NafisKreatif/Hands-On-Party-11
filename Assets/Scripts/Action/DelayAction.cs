using System.Collections;
using UnityEngine;

public class DelayAction : SceneAction
{
  public override string ActionName => "DelayAction";

  public float delay = 1.0f;
  public override void Execute(string id)
  {
    dialogId = id;
    StartCoroutine(Delay());
  }

  private IEnumerator Delay()
  {
    Debug.LogWarning("Action start");
    yield return new WaitForSeconds(delay);
    Debug.LogWarning("Action done");

    DialogManager.Instance.DialogDone(dialogId);
  }
}