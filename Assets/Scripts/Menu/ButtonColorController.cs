using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonColorController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IEndDragHandler
{
    public Color baseColor = Color.white;
    public Color onClickColor = Color.white;
    public Color onHoverColor = Color.white;
    public Color disabledColor = Color.white;

    public Image buttonFill;
    private Button _button;
    void Start()
    {
        _button = GetComponent<Button>();
        if (buttonFill == null)
        {
            buttonFill = GetComponent<Image>();
        }
    }

    // Lagi klik pake onClickColor
    public void OnPointerDown(PointerEventData data)
    {
        if (_button != null && _button.interactable == false) return;
        buttonFill.color = onClickColor;
    }
    // Setelah klik pake onHoverColor
    public void OnPointerUp(PointerEventData data)
    {
        if (_button != null && _button.interactable == false) return;
        buttonFill.color = onHoverColor;
    }
    // Masuk button berarti sedang hover
    public void OnPointerEnter(PointerEventData data)
    {
        if (_button != null && _button.interactable == false) return;
        buttonFill.color = onHoverColor;
    }
    // Keluar button kembalikan warnanya seperti semula
    public void OnPointerExit(PointerEventData data)
    {
        if (_button != null && _button.interactable == false) return;
        buttonFill.color = baseColor;
    }
    // Kecuali jika keluarnya sambil drag, pakenya onClickColor
    public void OnDrag(PointerEventData data)
    {
        if (_button != null && _button.interactable == false) return;
        buttonFill.color = onClickColor;
    }
    // Selesai drag (di luar button) kembalikan color seperti semula
    public void OnEndDrag(PointerEventData data)
    {
        if (_button != null && _button.interactable == false) return;
        buttonFill.color = baseColor;
    }

    public void SetColor(string hexRGBA)
    {
        if (hexRGBA.Length != 8)
        {
            Debug.LogError("Not a valid color hex! Length must be 8!");
            return;
        }
        hexRGBA = hexRGBA.ToUpper();
        int red = 16 * CharToInt(hexRGBA[0]) + CharToInt(hexRGBA[1]);
        int green = 16 * CharToInt(hexRGBA[2]) + CharToInt(hexRGBA[3]);
        int blue = 16 * CharToInt(hexRGBA[4]) + CharToInt(hexRGBA[5]);
        int alpha = 16 * CharToInt(hexRGBA[6]) + CharToInt(hexRGBA[7]);

        Debug.Log($"Set {buttonFill.name} color to rgba({red}, {green}, {blue}, {alpha})");
        buttonFill.color = new Color(red / 255f, green / 255f, blue / 255f, alpha / 255f);
    }
    private int CharToInt(char c)
    {
        if ('0' <= c && c <= '9')
        {
            return c - '0';
        }
        else if ('A' <= c && c <= 'F')
        {
            return c - 'A' + 10;
        }
        else
        {
            Debug.LogError("Not a valid Hex! Char: " + c);
            return -1;
        }
    }
}
