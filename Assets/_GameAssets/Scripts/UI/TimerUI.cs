using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class TimerUI : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private RectTransform _timerRotatableTransform;
    [SerializeField] private TMP_Text _timerText;

    [Header("Settings")]
    [SerializeField] private float _rotationDuration;
    [SerializeField] private float _rotationDurationTest;
    [SerializeField] private Ease _rotationEase;

    private bool _isRotating;




    private void Update() {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (!_isRotating)
            {
                PlayClockAnimationTest();
            }
            
        }
    }
    private void PlayClockAnimation()
    {
        _timerRotatableTransform.DORotate(new Vector3(0f, 0f, -360f), _rotationDuration, RotateMode.FastBeyond360)
            .SetEase(_rotationEase)
            .SetLoops(-1, LoopType.Restart);

    }

    private void PlayClockAnimationTest()
    {
        _isRotating = true;
        _timerRotatableTransform.DORotate(new Vector3(0f, 0f,  - 30f), _rotationDurationTest, RotateMode.LocalAxisAdd)
            .SetEase(_rotationEase).OnComplete(() =>
            {
                _isRotating = false;
            });

    }

}
