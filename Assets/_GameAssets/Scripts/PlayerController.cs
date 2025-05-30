using UnityEngine;

public class PlayerController : MonoBehaviour

{
    [Header("References")]
    [SerializeField] private Transform _orientationTransform;


    [Header("Movement Settings")]
    [SerializeField] private float _moveSpeed = 20f;


    private Rigidbody _playerRigidbody;

    private float _horizontalInput, _verticalInput;

    private Vector3 _moveDirection;


    private void Awake()
    {
        _playerRigidbody = GetComponent<Rigidbody>();
        _playerRigidbody.freezeRotation = true;
        _playerRigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        _playerRigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
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
    }

    private void SetPlayerMovement()
    {
        _moveDirection = _orientationTransform.forward * _verticalInput + _orientationTransform.right * _horizontalInput;

        _playerRigidbody.AddForce(_moveDirection * _moveSpeed, ForceMode.Force);
    }






}
