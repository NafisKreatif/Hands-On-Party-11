using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class LockLevelController : MonoBehaviour
{
    private Button _button;
    public Image lockImage;
    public Image overlay;
    public int indexLevelSyaratUnlock = -2;

    void Start()
    {
        _button = GetComponent<Button>();
        if (indexLevelSyaratUnlock == -2 || LevelInfoManager.Instance.GetLevelInfo(indexLevelSyaratUnlock).bestTime != -1)
        {
            Unlock();
        }
        else
        {
            Lock();
        }
    }
    public void Unlock()
    {
        Debug.Log("Level " + name + " is unlocked!");
        overlay.color = new Color(0, 0, 0, 0f);
        lockImage.color = new Color(lockImage.color.r, lockImage.color.g, lockImage.color.b, 0);
        _button.interactable = true;
    }
    public void Lock()
    {
        Debug.Log("Level " + name + " is locked!");
        overlay.color = new Color(0, 0, 0, 0.5f);
        lockImage.color = new Color(lockImage.color.r, lockImage.color.g, lockImage.color.b, 1);
        _button.interactable = false;
    }
}
