using UnityEngine;

public class EggCollectible : MonoBehaviour, ICollectible
{
    public void Collect()
    {
        GameManager.Instance.OnEggCollected();
        AudioManager.Instance.Play(SoundType.PickupGoodSound);
        CameraShake.Instance.ShakeCamera(0.5f, 0.5f);
        Destroy(gameObject);
    }
}
