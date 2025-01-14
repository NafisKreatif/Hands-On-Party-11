using UnityEngine;

public class UIDisablerController : MonoBehaviour
{
    public void DisableUI()
    {
        UIDisablerManager.Instance.DisableUI();
    }
    public void EnableUI()
    {
        UIDisablerManager.Instance.EnableUI();
    }
}