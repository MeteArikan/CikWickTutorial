using UnityEngine;

public class PlayerController : MonoBehaviour

{
    [Header("References")]
    [SerializeField] private Transform _orientationTransform;


    [Header("Movement Settings")]
    [SerializeField] private float _moveSpeed;

    [Header("Jumping Settings")]
    [SerializeField] private KeyCode _jumpKey;
    [SerializeField] private float _jumpForce;
    [SerializeField] private bool _canJump;
    [SerializeField] private float _jumpCooldown;

    [Header("Ground Check Settings")]
    [SerializeField] private float _playerHeight;
    [SerializeField] private LayerMask _groundLayer;

    private Rigidbody _playerRigidbody;

    private float _horizontalInput, _verticalInput;

    private Vector3 _moveDirection;


    private void Awake()
    {
        _moveSpeed = 20f;
        _playerRigidbody = GetComponent<Rigidbody>();
        _playerRigidbody.freezeRotation = true;
        _playerRigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        _playerRigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;


        _jumpKey = KeyCode.Space;
        _canJump = true;
        _jumpForce = 5f;
        _jumpCooldown = 0.5f;

        _playerHeight = 1f;
        _groundLayer = LayerMask.GetMask("Ground");

    }
    private void Update()
    {
        SetInputs();
    }

    private void FixedUpdate()
    {
        SetPlayerMovement();
    }


    private void SetInputs()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");

        if (Input.GetKey(_jumpKey) && _canJump && IsGrounded())
        {
            _canJump = false;
            SetPlayerJumping();
            Invoke(nameof(ResetJumping), _jumpCooldown);
        }
    }

    private void SetPlayerMovement()
    {
        _moveDirection = _orientationTransform.forward * _verticalInput + _orientationTransform.right * _horizontalInput;

        _playerRigidbody.AddForce(_moveDirection.normalized * _moveSpeed, ForceMode.Force);
    }

    private void SetPlayerJumping()
    {
        _playerRigidbody.linearVelocity = new Vector3(_playerRigidbody.linearVelocity.x, 0f, _playerRigidbody.linearVelocity.z);
        _playerRigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
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
