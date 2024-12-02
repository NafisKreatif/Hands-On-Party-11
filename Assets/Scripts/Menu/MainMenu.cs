using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public SceneTransitionController transitionController;
    public Canvas MainMenuCanvas;
    void Start()
    {
        MainMenuCanvas.worldCamera = Camera.main;
    }
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
