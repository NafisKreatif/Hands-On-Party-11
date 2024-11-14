using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class GravityRotationController : MonoBehaviour
{
  public Transform cameraTransform;
  public bool mobileUseGyro = true;

  private float _initialGravity;
  private float _startAngle; // Angle before rotation gesture starts in degrees.
  private float _originAngle; // Angle of the mouse position relative to the center of the screen before rotation gesture starts in degrees.
  [SerializeField]
  private float _currentAngle; // Angle during rotation gesture in degrees.
  private float _currentAngleRad; // Angle during rotation gesture in radians.
  private Vector2 _screenCenter;
  private Vector2? _mousePosStart; // Mouse position when rotation gesture starts.
  private Quaternion _deviceRotation;

  public void Start()
  {
    _initialGravity = Physics2D.gravity.magnitude;
    _screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);

    if (SystemInfo.supportsGyroscope && mobileUseGyro) Input.gyro.enabled = true;
  }

  public void Update()
  {
    if (Application.isMobilePlatform || UnityEditor.EditorApplication.isRemoteConnected)
    {
      if (SystemInfo.supportsGyroscope && mobileUseGyro) {
        _deviceRotation = new Quaternion(Input.gyro.attitude.x, Input.gyro.attitude.y, -Input.gyro.attitude.z, -Input.gyro.attitude.w);
        _currentAngle = _deviceRotation.eulerAngles.z;      
      }

      // Update camera rotation and gravity
      cameraTransform.rotation = Quaternion.AngleAxis(_currentAngle, Vector3.forward);
      Physics2D.gravity = new Vector2(Mathf.Cos(_currentAngleRad - Mathf.PI / 2), Mathf.Sin(_currentAngleRad - Mathf.PI / 2)) * _initialGravity;
    }
    else
    {
      if (Input.GetButtonDown("Fire1"))
      {
        _mousePosStart = Input.mousePosition;
        _startAngle = _currentAngle;
        _originAngle = Mathf.Atan2(Input.mousePosition.y - _screenCenter.y, Input.mousePosition.x - _screenCenter.x) * Mathf.Rad2Deg;
      }
      else if (Input.GetButtonUp("Fire1"))
      {
        _mousePosStart = null;
      }
      else if (_mousePosStart != null)
      {
        _currentAngle = _startAngle + Mathf.Atan2(Input.mousePosition.y - _screenCenter.y, Input.mousePosition.x - _screenCenter.x) * Mathf.Rad2Deg - _originAngle;
        _currentAngleRad = _currentAngle * Mathf.Deg2Rad;

        // Update camera rotation and gravity
        cameraTransform.rotation = Quaternion.AngleAxis(_currentAngle, Vector3.forward);
        Physics2D.gravity = new Vector2(Mathf.Cos(_currentAngleRad - Mathf.PI / 2), Mathf.Sin(_currentAngleRad - Mathf.PI / 2)) * _initialGravity;
      }
    }
  }
}