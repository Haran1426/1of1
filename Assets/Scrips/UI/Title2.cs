using UnityEngine;
using UnityEngine.SceneManagement;

public class Title2 : MonoBehaviour
{
    // ��ư �̺�Ʈ �����
    public void LoadChoiceScene()
    {
        SceneManager.LoadScene("Choice");
    }

    public void LoadEditorScene()
    {
        SceneManager.LoadScene("Editor");
    }
}
