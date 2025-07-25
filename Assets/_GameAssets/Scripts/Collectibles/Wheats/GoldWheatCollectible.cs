using UnityEngine.UI;
using UnityEngine;

public class GoldWheatCollectible : MonoBehaviour, ICollectible
{
    [SerializeField] private WheatDesignSO _wheatDesignSO;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private PlayerStateUI _playerStateUI;

    private RectTransform _playerBoosterTransform;
    private Image _playerBoosterImage;

    private void Awake() {
        _playerBoosterTransform = _playerStateUI.GetBoosterSpeedTransform();
        _playerBoosterImage = _playerBoosterTransform.GetComponent<Image>();
    }

    public void Collect()
    {
        _playerController.SetMoveSpeed(_wheatDesignSO.IncreaseDecreaseMultiplier, _wheatDesignSO.ResetBoostDuration);
        _playerStateUI.PlayBoosterUIAnimations(_playerBoosterTransform, _playerBoosterImage, _playerStateUI.GetGoldBoosterWheatImage(),
        _wheatDesignSO.ActiveSprite, _wheatDesignSO.PassiveSprite,
        _wheatDesignSO.ActiveWheatSprite, _wheatDesignSO.PassiveWheatSprite, _wheatDesignSO.ResetBoostDuration);
        AudioManager.Instance.Play(SoundType.PickupGoodSound);
        CameraShake.Instance.ShakeCamera(0.5f, 0.5f);
        Destroy(gameObject);
    }
}
