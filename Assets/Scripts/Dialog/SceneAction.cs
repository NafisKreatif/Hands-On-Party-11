using UnityEditor.ShortcutManagement;
using UnityEngine;

public abstract class SceneAction : MonoBehaviour
{
  public abstract string ActionName { get; }    
  [HideInInspector]
  public string dialogId;

  // Within the end of Execute, the scene action should call DialogManager.Instance.DialogDone(actionName) to notify the dialog manager that the action has been executed.
  public abstract void Execute(string id);
}