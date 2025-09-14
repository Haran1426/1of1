using UnityEngine;
using UnityEngine.SceneManagement;

public class Title2 : MonoBehaviour
{
    // 버튼 이벤트 연결용
    public void LoadChoiceScene()
    {
        SceneManager.LoadScene("Choice");
    }

    public void LoadEditorScene()
    {
        SceneManager.LoadScene("Editor");
    }
}
