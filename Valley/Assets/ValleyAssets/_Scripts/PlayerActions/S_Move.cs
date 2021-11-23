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
    [SerializeField] private AnimationCurve decelerationCurve;
    [SerializeField] private float decelerationSpeed = 0.3f;
    [SerializeField] private float angleByScroll = 10f;
    [SerializeField] private float angleLimitUp = -70f;
    [SerializeField] private float angleLimitDown = -10f;
    [SerializeField] private float positionLimitUp = 40f;


    private ValleyInputActions _playerActions;
    private Rigidbody _rbody;
    private Vector3 _moveInput;

    private float zoomAcceleration;
    private float currentAngle = 70f;
    private float scrollSpeed;
    private float currentDeceleration = 0;

    private float lerpTarget = 0;
    private float startLerp = 0;

    private void Awake()
    {
        _playerActions = new ValleyInputActions();
        _rbody = GetComponent<Rigidbody>();
        if (_rbody is null)
            Debug.LogError("Rigidbody2D is NULL");

        currentAngle = angleLimitUp;
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
        scrollSpeed = _playerActions.Camera.Zoom.ReadValue<float>();

        //scrollSpeed = Input.GetAxis("Mouse ScrollWheel"); //= Input.mouseScrollDelta.y * 120f;

        if (scrollSpeed != 0)
        {
            currentDeceleration = 1;

            startLerp = currentAngle;
            if (scrollSpeed < 0)
            {
                lerpTarget = angleByScroll + startLerp;
                if(lerpTarget > angleLimitUp)
                {
                    lerpTarget = angleLimitUp;
                }
            }
            else if(scrollSpeed > 0)
            {
                lerpTarget = -angleByScroll + startLerp;
                if (lerpTarget < angleLimitDown)
                {
                    lerpTarget = angleLimitDown;
                }
            }
        }

        if(currentDeceleration != 0)
        {
            zoomAcceleration = decelerationCurve.Evaluate(1 - currentDeceleration);
            currentAngle = Mathf.Lerp(startLerp, lerpTarget, zoomAcceleration);

            cameraTransform.localPosition = new Vector3(cameraTransform.localPosition.x, CalculatePosition(), cameraTransform.localPosition.z);
            cameraTransform.localEulerAngles = new Vector3(currentAngle, cameraTransform.localEulerAngles.y, cameraTransform.localEulerAngles.z);

            currentDeceleration -= decelerationSpeed * Time.deltaTime;
            if(currentDeceleration < 0)
            {
                currentDeceleration = 0;
            }
        }
    }

    private float CalculatePosition()
    {
        return ((currentAngle + angleLimitDown) / (angleLimitUp + angleLimitDown)) * positionLimitUp;
    }
}
