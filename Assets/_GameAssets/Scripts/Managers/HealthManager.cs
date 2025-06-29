using System;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public static HealthManager Instance { get; private set; }
    public event Action OnGameLoseEvent;

    [Header("References")]
    [SerializeField] private PlayerHealthUI _healthUI;

    [Header("Settings")]
    [SerializeField] private int _maxHealth = 3;
    private int _currenthealth;

    private void Awake()
    {
        Instance = this;
    }  

    private void Start()
    {
        _currenthealth = _maxHealth;
    }

    public void Damage(int damageAmount)
    {
        if (_currenthealth > 0)
        {
            _currenthealth -= damageAmount;
            _healthUI.AnimateDamage();
            if (_currenthealth <= 0)
            {
                OnGameLoseEvent?.Invoke();

            }
            {

            }
        }

    }

    public void Heal(int healAmount)
    {
        if (_currenthealth < _maxHealth)
        {
            _currenthealth = Mathf.Min(_currenthealth + healAmount, _maxHealth);
        }
    }
}
