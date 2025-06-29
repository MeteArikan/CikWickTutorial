using UnityEngine;

public class FireDamageable : MonoBehaviour, IDamageable
{
    [SerializeField] private float _force = 10f;
    public void GiveDamage(Rigidbody playerRigidbody, Transform playerVisualTransform)
    {
        playerRigidbody.AddForce(-playerVisualTransform.forward * _force, ForceMode.Impulse);
        HealthManager.Instance.Damage(1);
        Destroy(gameObject);
    }
}
