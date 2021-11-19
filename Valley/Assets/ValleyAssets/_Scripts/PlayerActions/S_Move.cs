using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using static ValleyInputActions;

public class S_Move : MonoBehaviour
{
    [SerializeField]
    private float _speed;
    private ValleyInputActions _playerActions;
    private Rigidbody _rbody;
    private Vector3 _moveInput;

    private void Awake()
    {
        _playerActions = new ValleyInputActions();
        _rbody = GetComponent<Rigidbody>();
        if (_rbody is null)
            Debug.LogError("Rigidbody2D is NULL");
    }

    private void OnEnable()
    {
        _playerActions.Player.Move.Enable();
    }

    private void OnDisable()
    {
        _playerActions.Player.Move.Disable();
    }

    private void FixedUpdate()
    {
        _moveInput = _playerActions.Player.Move.ReadValue<Vector2>();
        _moveInput.z = _moveInput.y;
        _moveInput.y = 0f;
        _rbody.velocity = _moveInput * _speed;
    }

}
