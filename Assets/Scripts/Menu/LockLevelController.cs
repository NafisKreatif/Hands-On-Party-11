using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class LockLevelController : MonoBehaviour
{
    public Button button;
    public RawImage lockImage;
    public int indexLevelSyaratUnlock = 0;

    void Start()
    {
        if (LevelInfoManager.Instance.GetLevelInfo(indexLevelSyaratUnlock).bestTime != -1)
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
        lockImage.color = new Color(0, 0, 0, 0);
        button.interactable = true;
    }
    public void Lock()
    {
        lockImage.color = new Color(1, 1, 1, 1);
        button.interactable = false;
    }
}
