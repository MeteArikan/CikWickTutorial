using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [SerializeField] private int _maxHealth = 3;
    private int _currenthealth;

    private void Start()
    {
        _currenthealth = _maxHealth;
    }

    public void Damage(int damageAmount)
    {
        if (_currenthealth > 0)
        {
            _currenthealth -= damageAmount;
            // TODO: Damage Animation
            if (_currenthealth <= 0)
            {
                // TODO: Player Death
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
