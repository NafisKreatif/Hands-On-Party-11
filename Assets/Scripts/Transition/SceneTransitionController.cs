using UnityEngine;

public class SceneTransitionController : MonoBehaviour
{
    public void TransitionIn()
    {
        SceneTransitionManager.Instance.TransitionIn();
    }
    public void TransitionOut()
    {
        SceneTransitionManager.Instance.TransitionOut();
    }
    public void GoToScene(string name)
    {
        SceneTransitionManager.Instance.GoToScene(name);
    }
    public void GoToScene(int index)
    {
        SceneTransitionManager.Instance.GoToScene(index);

    }
    public void ReloadScene()
    {
        SceneTransitionManager.Instance.ReloadScene();
    }
    public void NextScene()
    {
        SceneTransitionManager.Instance.NextScene();
    }
}
