using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ImageColorController : MonoBehaviour
{
    private Image _image;
    void Start()
    {
        _image = GetComponent<Image>();
    }
    int CharToInt(char c)
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
    public void SetColor(string hexRGBA)
    {
        if (hexRGBA.Length != 8) {
            Debug.LogError("Not a valid color hex! Length must be 8!");
            return;
        }
        hexRGBA = hexRGBA.ToUpper();
        int red = 16 * CharToInt(hexRGBA[0]) + CharToInt(hexRGBA[1]);
        int green = 16 * CharToInt(hexRGBA[2]) + CharToInt(hexRGBA[3]);
        int blue = 16 * CharToInt(hexRGBA[4]) + CharToInt(hexRGBA[5]);
        int alpha = 16 * CharToInt(hexRGBA[6]) + CharToInt(hexRGBA[7]);

        Debug.Log($"Set {_image.name} color to rgba({red}, {green}, {blue}, {alpha})");
        _image.color = new Color(red / 255f, green / 255f, blue / 255f, alpha / 255f);
    }
}
