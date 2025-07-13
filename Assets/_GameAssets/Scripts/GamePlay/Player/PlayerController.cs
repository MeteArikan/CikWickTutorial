using System;
using UnityEngine;

public class PlayerController : MonoBehaviour

{
    public event Action OnPlayerJumped;
    public event Action<PlayerState> OnPlayerStateChanged;

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
    [SerializeField] private float _airMultiplier;
    [SerializeField] private float _airDrag;

    [Header("Sliding Settings")]
    [SerializeField] private KeyCode _slideKey;
    [SerializeField] private float _slideMultiplier;
    [SerializeField] private float _slideDrag;


    [Header("Ground Check Settings")]
    [SerializeField] private float _playerHeight;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _groundDrag;

    private StateController _stateController;
    private Rigidbody _playerRigidbody;

    private float _startingMoveSpeed, _startingJumpForce;
    private float _horizontalInput, _verticalInput;

    private Vector3 _moveDirection;

    private bool _isSliding;


    private void Awake()
    {
        // State Controller
        _stateController = GetComponent<StateController>();
        // Movement
        _moveSpeed = 40f;
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
        _airMultiplier = 0.2f;
        _airDrag = 0.75f;

        // Ground Check
        _playerHeight = 1f;
        //_groundLayer = LayerMask.GetMask("Ground");

        // Sliding
        _slideKey = KeyCode.Q;
        _slideMultiplier = 1.3f;
        _slideDrag = 2.2f;


        _startingMoveSpeed = _moveSpeed;
        _startingJumpForce = _jumpForce;

    }
    private void Update()
    {
        if (GameManager.Instance.GetCurrentGameState() != GameState.Play
        && GameManager.Instance.GetCurrentGameState() != GameState.Resume)
        {
            return;
        }
        SetInputs();
        SetStates();
        SetPlayerDrag();
        LimitPlayerVelocity();

    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.GetCurrentGameState() != GameState.Play
        && GameManager.Instance.GetCurrentGameState() != GameState.Resume)
        {
            return;
        }
        SetPlayerMovement();
    }


    private void SetInputs()
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
            SetPlayerJumping();
            Invoke(nameof(ResetJumping), _jumpCooldown);
            AudioManager.Instance.Play(SoundType.JumpSound);
        }
    }

    private void SetStates()
    {
        Vector3 movementDirection = GetMovementDirection();
        bool isGrounded = IsGrounded();
        PlayerState currentState = _stateController.GetPlayerState();

        PlayerState newState = currentState switch
        {
            _ when movementDirection == Vector3.zero && isGrounded && !_isSliding => PlayerState.Idle,
            _ when movementDirection != Vector3.zero && isGrounded && !_isSliding => PlayerState.Move,
            _ when movementDirection != Vector3.zero && isGrounded && _isSliding => PlayerState.Slide,
            _ when movementDirection == Vector3.zero && isGrounded && _isSliding => PlayerState.SlideIdle,
            _ when !_canJump && !isGrounded => PlayerState.Jump,
            _ => currentState
        };

        if (newState != currentState)
        {
            _stateController.ChangePlayerState(newState);
            OnPlayerStateChanged?.Invoke(newState);
        }
    }

    private void SetPlayerMovement()
    {
        _moveDirection = _orientationTransform.forward * _verticalInput + _orientationTransform.right * _horizontalInput;

        float forceMultiplier = _stateController.GetPlayerState() switch
        {
            PlayerState.Move => 1f,
            PlayerState.Slide => _slideMultiplier,
            PlayerState.Jump => _airMultiplier,
            _ => 1f
        };

        _playerRigidbody.AddForce(_moveDirection.normalized * _moveSpeed * forceMultiplier, ForceMode.Force);
    }

    private void SetPlayerJumping()
    {
        OnPlayerJumped?.Invoke();
        _playerRigidbody.linearVelocity = new Vector3(_playerRigidbody.linearVelocity.x, 0f, _playerRigidbody.linearVelocity.z);
        _playerRigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
    }

    private void SetPlayerDrag()
    {
        _playerRigidbody.linearDamping = _stateController.GetPlayerState() switch
        {
            PlayerState.Move => _groundDrag,
            PlayerState.Slide => _slideDrag,
            PlayerState.Jump => _airDrag,
            _ => _playerRigidbody.linearDamping
        };
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

    #region Helper Functions

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, _playerHeight * 0.5f + 0.1f, _groundLayer);
    }

    private Vector3 GetMovementDirection()
    {
        return _moveDirection.normalized;
    }

    public void SetMoveSpeed(float speed, float duration)
    {
        _moveSpeed += speed;
        Invoke(nameof(ResetMoveSpeed), duration);
    }

    public void ResetMoveSpeed()
    {
        _moveSpeed = _startingMoveSpeed;
    }


    public void SetJumpForce(float force, float duration)
    {
        _jumpForce += force;
        Invoke(nameof(ResetJumpForce), duration);
    }

    public void ResetJumpForce()
    {
        _jumpForce = _startingJumpForce;
    }

    public Rigidbody GetPlayerRigidbody()
    {
        return _playerRigidbody;
    }

    public bool CanCatChase()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit,
        _playerHeight * 0.5f + 0.2f, _groundLayer))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer(Consts.Layers.FLOOR_LAYER))
            {
                return true;
            }
            else if (hit.collider.gameObject.layer == LayerMask.NameToLayer(Consts.Layers.GROUND_LAYER))
            {
                return false;
            }
        }
        return false;
    }
    
    #endregion






}
