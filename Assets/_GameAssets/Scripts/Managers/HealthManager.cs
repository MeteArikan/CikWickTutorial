using System;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [SerializeField] private int _maxHealth = 3;
    [SerializeField] private PlayerHealthUI _playerHealthUI;
    private int _currenthealth;


    public int CurrentHealth => _currenthealth;
    public int MaxHealth => _maxHealth;

    private void Start()
    {
        _currenthealth = _maxHealth;
    }
    
        private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            Damage(1);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            Damage(_maxHealth);
        }
    }

    public void Damage(int damageAmount)
    {
        if (_currenthealth > 0)
        {
            _currenthealth = Mathf.Max(_currenthealth - damageAmount, 0);
            // TODO: Damage Animation
            _playerHealthUI.AnimateDamage(damageAmount);
            Debug.Log("Current Health: " + _currenthealth);
            if (_currenthealth <= 0)
            {
                // TODO: Player Death
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
