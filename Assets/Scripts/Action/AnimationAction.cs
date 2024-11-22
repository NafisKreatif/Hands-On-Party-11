using UnityEngine;

public class AnimationAction : SceneAction
{
  public override string ActionName => "AnimationAction";

  public enum AnimationActionType { Play, Bool, Float, Int };

  public Animator animator;
  public AnimationActionType animationActionType;  
  [Tooltip("When animationActionType set to Play, name of the animation to be played, else than Play, the name of the parameter")]
  public string stringValue;
  [Tooltip("Value of the bool to be set. Only used when animationActionType is Bool")]
  public bool boolValue;
  [Tooltip("Value of the float to be set. Only used when animationActionType is Float")]
  public float floatValue;
  [Tooltip("Value of the int to be set. Only used when animationActionType is Int")]
  public int intValue;

  public override void Execute(string id)
  {
    dialogId = id;    
    switch (animationActionType)
    {
      case AnimationActionType.Play:
        animator.Play(stringValue);
        break;
      case AnimationActionType.Bool:
        animator.SetBool(stringValue, boolValue);
        break;
      case AnimationActionType.Float:
        animator.SetFloat(stringValue, floatValue);
        break;
      case AnimationActionType.Int:
        animator.SetInteger(stringValue, intValue);
        break;
    }
    DialogManager.Instance.DialogDone(dialogId);
  }
}