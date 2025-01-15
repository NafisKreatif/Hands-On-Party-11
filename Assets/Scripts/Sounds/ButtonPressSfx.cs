using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonPressSfx : MonoBehaviour, IPointerClickHandler
{
    public AudioSource pressSfx;
    public void OnPointerClick(PointerEventData data)
    {
        pressSfx.Play();
    }
}
