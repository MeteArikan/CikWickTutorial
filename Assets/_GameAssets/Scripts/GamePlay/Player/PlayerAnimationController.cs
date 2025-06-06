using System;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Animator _playerAnimator;

    private PlayerController _playerController;
    private StateController _stateController;

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _stateController = GetComponent<StateController>();
    }
    
    private void Start() {
        _playerController.OnPlayerJumped += PlayerController_OnPlayerJumped;
    }


    private void Update()
    {
        SetPlayerAnimations();
    }


    private void PlayerController_OnPlayerJumped()
    {
        _playerAnimator.SetBool(Consts.PlayerAnimations.IS_JUMPING, true);
        Invoke(nameof(ResetJumpingAnimation), 0.5f);
    }

    private void ResetJumpingAnimation()
    {
        _playerAnimator.SetBool(Consts.PlayerAnimations.IS_JUMPING, false);
    }

    private void SetPlayerAnimations()
    {
        var currentState = _stateController.GetPlayerState();

        switch (currentState)
        {
            case PlayerState.Idle:
                _playerAnimator.SetBool(Consts.PlayerAnimations.IS_MOVING, false);
                _playerAnimator.SetBool(Consts.PlayerAnimations.IS_SLIDING, false);
                break;

            case PlayerState.Move:
                _playerAnimator.SetBool(Consts.PlayerAnimations.IS_MOVING, true);
                _playerAnimator.SetBool(Consts.PlayerAnimations.IS_SLIDING, false);
                break;

            case PlayerState.SlideIdle:
                _playerAnimator.SetBool(Consts.PlayerAnimations.IS_SLIDING_ACTIVE, false);
                _playerAnimator.SetBool(Consts.PlayerAnimations.IS_SLIDING, true);
                break;

            case PlayerState.Slide:
                _playerAnimator.SetBool(Consts.PlayerAnimations.IS_SLIDING_ACTIVE, true);
                _playerAnimator.SetBool(Consts.PlayerAnimations.IS_SLIDING, true);
                break;

        }
    }
}
