using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public TransitionController transitionController;
    public void Play(int level)
    {
        transitionController.GoToSceneByIndex(level);
    }
    public void Quit()
    {
        Application.Quit();
        Debug.Log("Player Has quit the game");
    }
}
