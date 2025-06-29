using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public event Action<GameState> OnGameStateChanged;

    [Header("References")]
    [SerializeField] private EggCounterUI _eggCounterUI;
    [SerializeField] private WinLoseUI _winLoseUI;


    [Header("Settings")]
    [SerializeField] private int _maxEggCount = 5;
    [SerializeField] private float _duration = 0.4f;


    private GameState _currentGameState;
    private int _currentEggCount;


    private void Awake()
    {
        Instance = this;
    }

    private void Start() {
        HealthManager.Instance.OnGameLoseEvent += OnGameLoseHandler;
    }



    private void OnEnable()
    {
        ChangeGameState(GameState.Play);
    }
    public void ChangeGameState(GameState gameState)
    {
        OnGameStateChanged?.Invoke(gameState);
        _currentGameState = gameState;
        Debug.Log("Game State: " + gameState);
    }
    public void OnEggCollected()
    {
        _currentEggCount++;
        _eggCounterUI.SetEggCounter(_currentEggCount, _maxEggCount);
        if (_currentEggCount == _maxEggCount)
        {
            // WIN
            ChangeGameState(GameState.GameOver);
            _eggCounterUI.SetEggCompleted();
            _winLoseUI.OnGameWin();
            
        }
    }
    private IEnumerator OnGameLose()
    {
        yield return new WaitForSeconds(_duration);
        ChangeGameState(GameState.GameOver);
        _winLoseUI.OnGameLose();
    }
    private void OnGameLoseHandler()
    {
        StartCoroutine(OnGameLose());
    }

    public GameState GetCurrentGameState()
    {
        return _currentGameState;
    }

}
