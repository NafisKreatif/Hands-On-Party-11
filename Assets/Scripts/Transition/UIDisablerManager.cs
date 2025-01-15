using UnityEngine;

public class UIDisablerManager : MonoBehaviour
{
    public static UIDisablerManager Instance;
    public GameObject panel;
    void Awake()
    {
        // Kalo udah ada transition manager
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // ga usah buat lagi, pake yang lama
        }
        else
        {
            // Set instance
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void DisableUI()
    {
        panel.SetActive(true); // Buat jadi tidak bisa pejet apa apa
    }
    public void EnableUI()
    {
        panel.SetActive(false); // Kembali bisa pejet apa apa
    }
}