using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class PlayerHealthUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image[] _playerHealthImages;

    [Header("Sprites")]
    [SerializeField] private Sprite _healthySprite;
    [SerializeField] private Sprite _damagedSprite;

    [Header("Sprites")]
    [SerializeField] private float _animationDuration;

    private RectTransform[] _playerHealthTransforms;


    private void Awake()
    {
        _playerHealthTransforms = new RectTransform[_playerHealthImages.Length];
        for (int i = 0; i < _playerHealthImages.Length; i++)
        {
            _playerHealthTransforms[i] = _playerHealthImages[i].GetComponent<RectTransform>();
        }
    }


    // private void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.O))
    //     {
    //         AnimateDamage();
    //     }
    //     if (Input.GetKeyDown(KeyCode.P))
    //     {
    //         AnimateDamageForAll();
    //     }
    // }

    // public void AnimateDamage()
    // {
    //     for (int i = 0; i < _playerHealthImages.Length; i++)
    //     {
    //         if (_playerHealthImages[i].sprite == _healthySprite)
    //         {
    //             AnimateDamageSprites(_playerHealthImages[i], _playerHealthTransforms[i]);
    //             break;
    //         }
    //     }
    // }

    // public void AnimateDamageForAll()
    // {
    //     for (int i = 0; i < _playerHealthImages.Length; i++)
    //     {
    //         AnimateDamageSprites(_playerHealthImages[i], _playerHealthTransforms[i]);
    //     }
    // }

    public void AnimateDamage()
    {
        AnimateDamage(1);
    }

    public void AnimateDamageForAll()
    {
        AnimateDamage(_playerHealthImages.Length);
    }

    public void AnimateDamage(int damageCount)
    {
        int animatedCount = 0;
        
        for (int i = 0; i < _playerHealthImages.Length && animatedCount < damageCount; i++)
        {
            if (_playerHealthImages[i].sprite == _healthySprite)
            {
                AnimateDamageSprites(_playerHealthImages[i], _playerHealthTransforms[i]);
                animatedCount++;
            }
        }
    }

    private void AnimateDamageSprites(Image activeImage, RectTransform activeImageTransform)
    {
        activeImageTransform.DOScale(0f, _animationDuration).SetEase(Ease.InBack).OnComplete(() =>
        {
            activeImage.sprite = _damagedSprite;
            activeImageTransform.DOScale(1f, _animationDuration).SetEase(Ease.OutBack);
        });
    }

}
