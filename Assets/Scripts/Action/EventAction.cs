using UnityEngine.Events;

public class EventAction : SceneAction {
    public override string ActionName => "EventAction";

    public UnityEvent ExecuteEvent;

    public override void Execute(string id) {
        dialogId = id;
        ExecuteEvent.Invoke();
        DialogManager.Instance.DialogDone(dialogId);
    }
}