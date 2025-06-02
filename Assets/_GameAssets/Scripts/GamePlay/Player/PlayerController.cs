using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerController : MonoBehaviour

{
    [Header("References")]
    [SerializeField] private Transform _orientationTransform;


    [Header("Movement Settings")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private KeyCode _moveKey;

    [Header("Jumping Settings")]
    [SerializeField] private KeyCode _jumpKey;
    [SerializeField] private float _jumpForce;
    [SerializeField] private bool _canJump;
    [SerializeField] private float _jumpCooldown;

    [Header("Sliding Settings")]
    [SerializeField] private KeyCode _slideKey;
    [SerializeField] private float _slideMultiplier;
    [SerializeField] private float _slideDrag;
    

    [Header("Ground Check Settings")]
    [SerializeField] private float _playerHeight;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _groundDrag;


    private Rigidbody _playerRigidbody;

    private float _horizontalInput, _verticalInput;

    private Vector3 _moveDirection;

    private bool _isSliding;


    private void Awake()
    {
        // Movement
        _moveSpeed = 30f;
        _moveKey = KeyCode.E;
        _playerRigidbody = GetComponent<Rigidbody>();
        _playerRigidbody.freezeRotation = true;
        _playerRigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        _playerRigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
        _groundDrag = 5f;

        // Jumping
        _jumpKey = KeyCode.Space;
        _canJump = true;
        _jumpForce = 8f;
        _jumpCooldown = 0.5f;

        // Ground Check
        _playerHeight = 1f;
        _groundLayer = LayerMask.GetMask("Ground");

        // Sliding
        _slideKey = KeyCode.Q;
        _slideMultiplier = 2f;
        _slideDrag = 2f;

    }
    private void Update()
    {
        SetInputs();
        SetPlayerDrag();
        LimitPlayerVelocity();

    }

    private void FixedUpdate()
    {
        SetPlayerMovement();
    }


    private void SetInputs()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(_slideKey))
        {
            _isSliding = true;
            Debug.Log("Sliding");
        }
        else if (Input.GetKeyDown(_moveKey))
        {
            _isSliding = false;
            Debug.Log("Moving");
        }

        else if (Input.GetKey(_jumpKey) && _canJump && IsGrounded())
        {
            _canJump = false;
            SetPlayerJumping();
            Invoke(nameof(ResetJumping), _jumpCooldown);
        }
    }

    private void SetPlayerMovement()
    {
        _moveDirection = _orientationTransform.forward * _verticalInput + _orientationTransform.right * _horizontalInput;

        float forceMultiplier = 1f;
        if (_isSliding)
        {
            forceMultiplier = _slideMultiplier;
        }

        _playerRigidbody.AddForce(_moveDirection.normalized * _moveSpeed * forceMultiplier, ForceMode.Force);
    }

    private void SetPlayerJumping()
    {
        _playerRigidbody.linearVelocity = new Vector3(_playerRigidbody.linearVelocity.x, 0f, _playerRigidbody.linearVelocity.z);
        _playerRigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
    }

    private void SetPlayerDrag()
    {
        if(_isSliding)
        {
            _playerRigidbody.linearDamping = _slideDrag;
        }
        else
        {
            _playerRigidbody.linearDamping = _groundDrag;
        }
    }

    private void LimitPlayerVelocity()
    {
        Vector3 flatVelocity = new Vector3(_playerRigidbody.linearVelocity.x, 0f, _playerRigidbody.linearVelocity.z);
        if (flatVelocity.magnitude > _moveSpeed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * _moveSpeed;
            _playerRigidbody.linearVelocity = new Vector3(limitedVelocity.x, _playerRigidbody.linearVelocity.y, limitedVelocity.z);
        }
    }

    private void ResetJumping()
    {
        _canJump = true;
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, _playerHeight * 0.5f + 0.1f, _groundLayer);
    }
    







}
