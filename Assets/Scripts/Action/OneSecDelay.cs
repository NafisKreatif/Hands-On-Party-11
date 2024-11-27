using System.Collections;
using UnityEngine;

public class OneSecDelay : SceneAction
{
  public override string ActionName => "OneSecDelay";
  public override void Execute(string id)
  {
    dialogId = id;
    StartCoroutine(Delay());
  }

  private IEnumerator Delay()
  {
    Debug.LogWarning("Action start");
    yield return new WaitForSecondsRealtime(1);
    Debug.LogWarning("Action done");

    //* REQUIRED!!!!
    DialogManager.Instance.DialogDone(dialogId);
  }
}