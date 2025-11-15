using System;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputBindingCompositeContext;

public class FirstPersonController : MonoBehaviour
{

    [SerializeField]
    private CharacterController characterController;
    [SerializeField]
    private Transform cameraTransform;
    [SerializeField]
    private InputAction moveAction;
    [SerializeField]
    private InputAction runAction;

    [Header("Movement")]
    [SerializeField]
    private float moveSpeed = 5f;
    [SerializeField]
    private float runSpeed = 1.5f;
    [SerializeField]
    private float movementLerp = 10f;
    [SerializeField]
    private Vector2 mouseSensitivity = new Vector2(2f,2f);
    [SerializeField]
    private Vector2 clampedView = new Vector2(-45f,45f);
    [SerializeField]
    private float mouseLerp = 10f;

    #region Variables
    private float _rotation_y = 0f;
    private float _rotation_x = 0f;
    private Vector3 _smoothedMoveInput = Vector3.zero;
    private Vector2 _moveInput = Vector2.zero;
    private Vector3 _move = Vector3.zero;
    private Vector2 _mouseDelta = Vector2.zero;
    private bool _spaceInput = false;
    private float run = 0f;
    private Vector2 _rawMouseDelta = Vector2.zero;
    #endregion

    void OnEnable()
    {
        moveAction?.Enable();
        runAction?.Enable();
    }

    void OnDisable()
    {
        moveAction?.Disable();
        runAction?.Disable();
    }

    private void Awake()
    {
        moveAction.Enable();
        runAction?.Enable();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        HandleLook();
        HandleMovement();
    }

    private void HandleMovement()
    {

        if (!characterController.isGrounded)
        {
            characterController.Move(Vector3.down * Time.deltaTime);
        }

        _moveInput = moveAction.ReadValue<Vector2>();
        _spaceInput = runAction.ReadValue<float>() > 0.1f;

        if (_spaceInput) run = runSpeed + 1;
        else run = 1;

        _smoothedMoveInput = Vector2.Lerp(_smoothedMoveInput, _moveInput, Time.deltaTime * movementLerp);
        if (_smoothedMoveInput.magnitude > 1f)
            _smoothedMoveInput = _smoothedMoveInput.normalized;

        _move = (transform.right * _smoothedMoveInput.x + transform.forward * _smoothedMoveInput.y);
        characterController.Move(_move * moveSpeed * Time.deltaTime * run);
    }

    private void HandleLook()
    {
        _rawMouseDelta = Mouse.current.delta.ReadValue() * mouseSensitivity * Time.deltaTime;

        _mouseDelta = Vector2.Lerp(_mouseDelta, _rawMouseDelta, Time.deltaTime * mouseLerp);

        _rotation_y -= _mouseDelta.y;
        _rotation_x += _mouseDelta.x;
        _rotation_y = Mathf.Clamp(_rotation_y, clampedView.x, clampedView.y);

        cameraTransform.localRotation = Quaternion.Euler(_rotation_y, 0f, 0f);
        transform.rotation = Quaternion.Euler(0f, _rotation_x, 0f);
    }
}
