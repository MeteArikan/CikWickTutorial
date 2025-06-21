using DG.Tweening;
using TMPro;
using UnityEngine;

public class EggCounterUI : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private TMP_Text _eggCounterText;


    [Header("Settings")]
    [SerializeField] private Color _eggCounterTextColor;
    [SerializeField] private float _colorDuration;
    [SerializeField] private float _scaleDuration;

    private RectTransform _eggCounterRectTransform;



    private void Awake()
    {
        _eggCounterRectTransform = _eggCounterText.GetComponent<RectTransform>();

    }


    public void SetEggCounter(int counter, int max)
    {
        _eggCounterText.text = $"{counter}/{max}";
    }

    public void SetEggCompleted()
    {
        _eggCounterText.DOColor(_eggCounterTextColor, _colorDuration);
        _eggCounterRectTransform.DOScale(1.2f, _scaleDuration).SetEase(Ease.OutBack);
    }
}
