using UnityEngine;

public class TimescaleAction : SceneAction
{
  public float timeScale;
  public override string ActionName => "TimescaleAction";
  public override void Execute(string id)
  {
    dialogId = id;

    Time.timeScale = timeScale;
    DialogManager.Instance.DialogDone(dialogId);
  }
}