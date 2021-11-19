using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using static ValleyInputActions;

public class S_Move : MonoBehaviour
{
    [SerializeField] private float axisSpeed;
    [SerializeField] private Transform cameraTransform;

    [Header("Camera Zoom")]
    [SerializeField] private AnimationCurve zoomCurve;
    [SerializeField] private AnimationCurve accelerationCurve;
    [SerializeField] private float exponentialCoef = -.05f;
    [SerializeField] private float zoomLimitUp = 10f;
    [SerializeField] private float angleLimitUp = -70f;
    [SerializeField] private float angleLimitDown = -10f;
    [SerializeField] private float zoomAccelerationMax = .5f;
    [SerializeField] private float zoomDeceleration = .5f;

    private ValleyInputActions _playerActions;
    private Rigidbody _rbody;
    private Vector3 _moveInput;

    private float zoomAcceleration;
    private float currentZoomLevel = 0;
    private float scrollSpeed;
    private float zoomLimitDown = 0f;


    private void Awake()
    {
        _playerActions = new ValleyInputActions();
        _rbody = GetComponent<Rigidbody>();
        if (_rbody is null)
            Debug.LogError("Rigidbody2D is NULL");
    }

    private void OnEnable()
    {
        _playerActions.Camera.Enable();
    }

    private void OnDisable()
    {
        _playerActions.Camera.Disable();
    }

    private void FixedUpdate()
    {
        UpdateCameraZoom();

        _moveInput = _playerActions.Camera.Move.ReadValue<Vector2>();
        _moveInput.z = _moveInput.y;
        _moveInput.y = 0f;
        _rbody.velocity = _moveInput * axisSpeed;
    }

    private void UpdateCameraZoom()
    {
        //scrollSpeed = _playerActions.Camera.Zoom.ReadValue<Vector2>().y;

        scrollSpeed = Input.mouseScrollDelta.y * 120f;

        Debug.Log(scrollSpeed);
        if (scrollSpeed != 0)
        {
            if(zoomAcceleration > 0 & scrollSpeed < 0 || zoomAcceleration < 0 && scrollSpeed > 0)
            {
                zoomAcceleration = 0;
            }
            zoomAcceleration += (scrollSpeed / 120f) * 10 * accelerationCurve.Evaluate(GetZoomCoef());
        }

        if(zoomAcceleration != 0)
        {
            if (zoomAcceleration > accelerationCurve.Evaluate(GetZoomCoef()) * zoomAccelerationMax)
            {
                zoomAcceleration = accelerationCurve.Evaluate(GetZoomCoef()) * zoomAccelerationMax;
            }
            else if (zoomAcceleration < -accelerationCurve.Evaluate(GetZoomCoef()) * zoomAccelerationMax)
            {
                zoomAcceleration = -accelerationCurve.Evaluate(GetZoomCoef()) * zoomAccelerationMax;
            }

            currentZoomLevel += zoomAcceleration * Time.deltaTime;
            currentZoomLevel = Mathf.Clamp(currentZoomLevel, zoomLimitDown, zoomLimitUp);

            cameraTransform.localPosition = new Vector3(cameraTransform.localPosition.x, zoomLimitUp - currentZoomLevel, cameraTransform.localPosition.z);
            cameraTransform.localEulerAngles = new Vector3(CalculateCameraRotation(), cameraTransform.localEulerAngles.y, cameraTransform.localEulerAngles.z);
        }

        if (Mathf.Abs(zoomAcceleration) > 0)
        {
            zoomAcceleration -= (zoomDeceleration * accelerationCurve.Evaluate(GetZoomCoef())) * Time.deltaTime * (zoomAcceleration / Mathf.Abs(zoomAcceleration));
            if (Mathf.Abs(zoomAcceleration) < 0)
            {
                zoomAcceleration = 0;
            }
        }
    }

    private float CalculateCameraRotation()
    {
        return zoomCurve.Evaluate(GetZoomCoef()) * (angleLimitUp - angleLimitDown) + angleLimitDown;
    }

    private float GetZoomCoef()
    {
        return (currentZoomLevel - zoomLimitDown) / (zoomLimitUp - zoomLimitDown);
    }
}
