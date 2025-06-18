using UnityEngine;
using UnityEngine.UI;

public class RottenWheatCollectible : MonoBehaviour, ICollectible
{
    [SerializeField] private WheatDesignSO _wheatDesignSO;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private PlayerStateUI _playerStateUI;
    [SerializeField] private HealthManager _healthManager;

    private RectTransform _playerBoosterTransform;
    private Image _playerBoosterImage;

    private void Awake()
    {
        _playerBoosterTransform = _playerStateUI.GetBoosterSlowTransform();
        _playerBoosterImage = _playerBoosterTransform.GetComponent<Image>();
    }

    public void Collect()
    {
        _playerController.SetMoveSpeed(_wheatDesignSO.IncreaseDecreaseMultiplier, _wheatDesignSO.ResetBoostDuration);
        _playerStateUI.PlayBoosterUIAnimations(_playerBoosterTransform, _playerBoosterImage, _playerStateUI.GetRottenBoosterWheatImage(),
        _wheatDesignSO.ActiveSprite, _wheatDesignSO.PassiveSprite,
        _wheatDesignSO.ActiveWheatSprite, _wheatDesignSO.PassiveWheatSprite, _wheatDesignSO.ResetBoostDuration);
        ApplyDamage();
        Destroy(gameObject);
    }

    public void ApplyDamage()
    {
        if (_healthManager != null)
        {
            if (_healthManager.CurrentHealth == _healthManager.MaxHealth)
            {
                Debug.Log("current health: " + _healthManager.CurrentHealth);
                _healthManager.Damage(2);
                Debug.Log("current health: " + _healthManager.CurrentHealth);

            }
            else
            {
                _healthManager.Damage(1);
            }
        }
        else
        {
            Debug.Log("Healthmanager is null or playr health is 0");
        }
    }
}
