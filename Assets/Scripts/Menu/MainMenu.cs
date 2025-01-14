using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void Play(int level)
    {
        SceneTransitionManager.Instance.GoToScene(level);
    }
    public void Quit()
    {
        Application.Quit();
        Debug.Log("Player Has quit the game");
    }
}
