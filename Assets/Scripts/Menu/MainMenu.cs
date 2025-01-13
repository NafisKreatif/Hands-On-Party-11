using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void Play(int level)
    {
        SceneTransitionController.Instance.GoToScene(level);
    }
    public void Quit()
    {
        Application.Quit();
        Debug.Log("Player Has quit the game");
    }
}
