using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class GravityRotationController : MonoBehaviour
{
  public Transform cameraTransform;
  public bool useGyro = true;

  private float _initialGravity; // Initial gravity magnitude.
  private float _startAngle; // Angle before rotation gesture starts in degrees.
  private float? _originAngle; // Angle of the mouse position relative to the center of the screen before rotation gesture starts in degrees.    
  private float _offsetAngle = 0; // Angle offset calculated by Cursor rotation.
  private float _gyroAngle = 0; // Angle calculated by Gyroscope rotation.
  [SerializeField]
  private float _currentAngle = 0; // Angle during rotation gesture in degrees.  
  private float _currentAngleRad; // Angle during rotation gesture in radians.
  private Vector2 _screenCenter; // Center of the screen.
  private Vector2? _mousePosStart; // Mouse position when rotation gesture starts.
  private Quaternion _deviceRotation; // Quaternion of the device rotation when gyroscope is enabled.
  private Touch _touchA; // First touch input.
  private Touch _touchB; // Second touch input.

  private void Start()
  {
    _initialGravity = Physics2D.gravity.magnitude;
    _screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);

    if (SystemInfo.supportsGyroscope) Input.gyro.enabled = true;
  }

  // Updates the rotation of the camera and the direction of gravity based on user input.  
  // On mobile platforms, it uses the gyroscope or touch input to determine the rotation.
  // On non-mobile platforms, it uses mouse input to determine the rotation.  
  // With Gyro, the rotation is based on the axis where the device is facing (supposedly).
  // With Cursor, the rotation is calculated from the angle change between a single cursor (mouse or touch input) to the center of the screen or between two cursors.  
  private void FixedUpdate()
  {
    // Gyro rotation
    if (SystemInfo.supportsGyroscope && useGyro && _originAngle == null)
    {
      _deviceRotation = new Quaternion(0, 0, Input.gyro.attitude.z, -Input.gyro.attitude.w);
      _gyroAngle = _deviceRotation.eulerAngles.z;
    }
    
    // Cursor rotation
    if (Input.touchCount >= 2)
    {
      _touchA = Input.GetTouch(0);
      _touchB = Input.GetTouch(1);

      if (_touchB.phase == TouchPhase.Began)
      {
        _startAngle = _offsetAngle;
        _originAngle = Mathf.Atan2(_touchB.position.y - _touchA.position.y, _touchB.position.x - _touchA.position.x) * Mathf.Rad2Deg;
      }
      else if (_originAngle != null && (_touchA.phase == TouchPhase.Moved || _touchB.phase == TouchPhase.Moved))
      {
        _offsetAngle = _startAngle - (Mathf.Atan2(_touchB.position.y - _touchA.position.y, _touchB.position.x - _touchA.position.x) * Mathf.Rad2Deg - _originAngle.Value);
      }
      else if (_touchA.phase == TouchPhase.Ended || _touchB.phase == TouchPhase.Ended || _touchA.phase == TouchPhase.Canceled || _touchB.phase == TouchPhase.Canceled)
      {
        _originAngle = null;
      }
    }
    else if (Input.touchCount == 1 || Input.mousePresent)
    {
      if (Input.GetButtonDown("Fire1"))
      {
        _mousePosStart = Input.mousePosition;
        _startAngle = _offsetAngle;
        _originAngle = Mathf.Atan2(Input.mousePosition.y - _screenCenter.y, Input.mousePosition.x - _screenCenter.x) * Mathf.Rad2Deg;
      }
      else if (Input.GetButtonUp("Fire1"))
      {
        _mousePosStart = null;
        _originAngle = null;
      }
      else if (_mousePosStart != null && _originAngle != null)
      {
        _offsetAngle = _startAngle - (Mathf.Atan2(Input.mousePosition.y - _screenCenter.y, Input.mousePosition.x - _screenCenter.x) * Mathf.Rad2Deg - _originAngle.Value);
      }
    }

    UpdateRotation();
  }

  // Updates the center of the screen when the screen size changes.
  private void OnRectTransformDimensionsChange() {
    _screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
  }

  // Resets the rotation of the camera and the direction of gravity to the initial state.
  public void Reset()
  {
    _offsetAngle = 0;
    UpdateRotation();
  }

  // Updates the rotation of the camera and the direction of gravity based on the current angle.
  private void UpdateRotation()
  {
    _currentAngle = _offsetAngle + _gyroAngle;
    _currentAngleRad = _currentAngle * Mathf.Deg2Rad;
    cameraTransform.rotation = Quaternion.AngleAxis(_currentAngle, Vector3.forward);
    Physics2D.gravity = new Vector2(Mathf.Cos(_currentAngleRad - Mathf.PI / 2), Mathf.Sin(_currentAngleRad - Mathf.PI / 2)) * _initialGravity;
  }
}