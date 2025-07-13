using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverController : MonoBehaviour, IPointerEnterHandler
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.Instance.Play(SoundType.ButtonHoverSound);
    }
}
