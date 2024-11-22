using System.Collections.Generic;

public class CompositeAction : SceneAction
{
  public override string ActionName => "CompositeAction";

  public SceneAction[] actions;
  private readonly Dictionary<string, bool> _actionDoneState = new();
  private int _actionDoneCounter = 0;

  public override void Execute(string id)
  {
    dialogId = id;

    DialogManager.Instance.DialogDoneEvent.AddListener(OnDialogDone);

    for (int i = 0; i < actions.Length; i++)
    {
      string subActionId = dialogId + i;
      _actionDoneState[subActionId] = false;
      DialogManager.Instance.dialogDoneState[subActionId] = false;
      actions[i].Execute(subActionId);
    }
  }

  private void OnDialogDone(string id)
  {
    if (_actionDoneState.ContainsKey(id))
    {
      _actionDoneState[id] = true;
      _actionDoneCounter++;

      if (_actionDoneCounter == actions.Length)
      {
        DialogManager.Instance.DialogDoneEvent.RemoveListener(OnDialogDone);
        DialogManager.Instance.DialogDone(dialogId);
      }
    }
  }
}