using UnityEngine.Events;

public class EventAction : SceneAction {
    public override string ActionName => "EventAction";

    public UnityEvent unityEvent;

    public override void Execute(string id) {
        dialogId = id;
        unityEvent.Invoke();
        DialogManager.Instance.DialogDone(dialogId);
    }
}