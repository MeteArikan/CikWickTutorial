using UnityEngine;

public class PlayerInteractionController : MonoBehaviour

    
{
    [SerializeField] private Transform _playerVisualTransform;
    private PlayerController _playerController;
    private Rigidbody _playerRigidbody;
    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _playerRigidbody = GetComponent<Rigidbody>();

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<ICollectible>(out var collectible))
        {
            CameraShake.Instance.ShakeCamera(0.5f, 0.5f);
            collectible.Collect();
        }
    }
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent<IBoostable>(out var boostable))
        {
            boostable.Boost(_playerController);
        }
    }
    
    private void OnParticleCollision(GameObject other) {
        if (other.gameObject.TryGetComponent<IDamageable>(out var damageable))
        {
            CameraShake.Instance.ShakeCamera(1f, 0.5f);
            damageable.GiveDamage(_playerRigidbody, _playerVisualTransform);
        }
    }

}
