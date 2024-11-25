public class AngleConstraintAction : SceneAction
{
  public override string ActionName => "AngleConstraintAction";

  public float minAngle = 0;
  public float maxAngle = 360;
  private GravityRotationController _gravityRotationController;

  private void Start()
  {
    _gravityRotationController = FindFirstObjectByType<GravityRotationController>();
  }

  public override void Execute(string id)
  {
    dialogId = id;
    if (_gravityRotationController != null)
    {
      _gravityRotationController.minAngle = minAngle;
      _gravityRotationController.maxAngle = maxAngle;
    }
    DialogManager.Instance.DialogDone(dialogId);
  }
}