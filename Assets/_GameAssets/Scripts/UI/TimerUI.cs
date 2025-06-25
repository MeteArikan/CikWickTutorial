using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private RectTransform _timerRotatableTransform;
    [SerializeField] private TMP_Text _timerText;

    [Header("Settings")]
    [SerializeField] private float _rotationDuration;
    [SerializeField] private Ease _rotationEase;

    private float _elapsedTine;
    private bool _isTimerRunning;
    private Tween _rotationTween;


    private void Start()
    {
        PlayClockAnimation();
        StartTimer();
        GameManager.Instance.OnGameStateChanged += GameManager_OnGameStateChanged;
    }

    private void GameManager_OnGameStateChanged(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.Pause:
                PauseTimer();
                break;

            case GameState.Resume:
                ResumeTimer();
                break;
        }
    }

    private void PauseTimer()
    {
        _isTimerRunning = false;
        CancelInvoke(nameof(UpdateTimerUI));
        _rotationTween.Pause();

    }

    private void ResumeTimer()
    {
        if (!_isTimerRunning)
        {
            _isTimerRunning = true;
            InvokeRepeating(nameof(UpdateTimerUI), 0f, 1f);
            _rotationTween.Play();
            
        }
    }



    private void PlayClockAnimation()
    {
       _rotationTween = _timerRotatableTransform.DORotate(new Vector3(0f, 0f, -360f), _rotationDuration, RotateMode.FastBeyond360)
            .SetEase(_rotationEase)
            .SetLoops(-1, LoopType.Restart);

    }
    private void StartTimer()
    {
        _isTimerRunning = true;
        _elapsedTine = 0f;
        InvokeRepeating(nameof(UpdateTimerUI), 0f, 1f);
    }

    private void UpdateTimerUI()
    {
        if (!_isTimerRunning) { return; }

        _elapsedTine += 1f;
        int minutes = Mathf.FloorToInt(_elapsedTine / 60f);
        int seconds = Mathf.FloorToInt(_elapsedTine % 60f);
        _timerText.text = $"{minutes:00}:{seconds:00}";
    }

}
