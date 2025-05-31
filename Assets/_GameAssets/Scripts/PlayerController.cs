using UnityEngine;

public class PlayerController : MonoBehaviour

{
    [Header("References")]
    [SerializeField] private Transform _orientationTransform;


    [Header("Movement Settings")]
    [SerializeField] private KeyCode _moveKey;
    [SerializeField] private float _moveSpeed;

    [Header("Jump Settings")]
    [SerializeField] private KeyCode _jumpKey;
    [SerializeField] private float _jumpForce;
    [SerializeField] private bool _canJump;
    [SerializeField] private float _jumpcooldown;

    [Header("Sliding Settings")]
    [SerializeField] private KeyCode _slideKey;
    [SerializeField] private float _slideMultiplier;

    [Header("Ground Check Settings")]
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _playerHeight; // Height of the player for ground detection
    [SerializeField] private float _groundRaycastOffset; // Yeni eklenen: Raycast'in başlangıç yüksekliği






    private Rigidbody _playerRigidbody;

    private float _horizontalInput;
    private float _verticalInput;

    private Vector3 _moveDirection;

    private bool _isSliding;


    private void Awake()
    {
        _playerRigidbody = GetComponent<Rigidbody>();
        _playerRigidbody.freezeRotation = true;
        _playerRigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        _playerRigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
        _moveSpeed = 3f;

        _jumpForce = 8f; // Default jump force
        _jumpKey = KeyCode.Space; // Default jump key
        _canJump = true;
        _jumpcooldown = 0.5f;
        _playerHeight = 1f;
        _groundRaycastOffset = 0.1f; // Raycast'in yerden biraz yüksekten başlaması için


        _moveKey = KeyCode.E;
        _slideKey = KeyCode.Q;
        _slideMultiplier = 5f;

    }
    private void Update()
    {
        SetInput();
    }

    private void FixedUpdate()
    {
        SetPlayerMovement();
    }


    private void SetInput()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(_slideKey))
        {
            _isSliding = true;
        }
        else if (Input.GetKeyDown(_moveKey))
        {
            _isSliding = false;
        }
        else if (Input.GetKey(_jumpKey) && _canJump && IsGrounded())
        {
            _canJump = false;
            SetPlayerJump();
            Invoke(nameof(ResetJumping), _jumpcooldown);
        }
    }

    private void SetPlayerMovement()
    {
        _moveDirection = _orientationTransform.forward * _verticalInput + _orientationTransform.right * _horizontalInput;

        if (_isSliding)
        {
            _playerRigidbody.AddForce(_moveDirection.normalized * _moveSpeed * _slideMultiplier, ForceMode.Force);
        }
        else
        {
            _playerRigidbody.AddForce(_moveDirection.normalized * _moveSpeed, ForceMode.Force);
        }

    }

    private void SetPlayerJump()
    {
        _playerRigidbody.linearVelocity = new Vector3(_playerRigidbody.linearVelocity.x, 0f, _playerRigidbody.linearVelocity.z);
        _playerRigidbody.AddForce(transform.up * _jumpForce, ForceMode.Impulse);
    }

    private void ResetJumping()
    {
        _canJump = true;
    }

    private bool IsGrounded()
    {
        //return Physics.Raycast(transform.position, Vector3.down, _playerHeight * 0.5f + 0.2f, _groundLayer);
        return Physics.Raycast(transform.position + Vector3.up * _groundRaycastOffset, Vector3.down, _playerHeight * 0.5f + _groundRaycastOffset, _groundLayer);
    }





}
