using UnityEngine;
using UnityEngine.SceneManagement;

public class Title2 : MonoBehaviour
{
    public void LoadChoiceScene()
    {
        SceneFader.FadeToScene("Choice");
    }

    public void LoadEditorScene()
    {
        SceneFader.FadeToScene("Editor");
    }
    public void LoadOrScene()
    {
        SceneFader.FadeToScene("Or");
    }
    public void LoadTitleScene()
    {
        SceneFader.FadeToScene("Title");
    }
}
