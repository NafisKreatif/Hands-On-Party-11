using UnityEngine;

public class ButtonPressSfx : MonoBehaviour
{
    public AudioSource pressSfx;
    public void PlaySfx()
    {
        pressSfx.Play();
    }
}
