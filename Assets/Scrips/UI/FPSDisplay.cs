using UnityEngine;
using TMPro;

public class FPSDisplay : MonoBehaviour
{
    public TextMeshProUGUI fpsText;
    private float deltaTime;
    private float updateInterval = 0.12f; //���� ���� ����
    private float timeSinceLastUpdate = 0f;

    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        timeSinceLastUpdate += Time.unscaledDeltaTime;

        if (timeSinceLastUpdate >= updateInterval)
        {
            float fps = 1f / deltaTime;
            fpsText.text = $"FPS : {Mathf.Ceil(fps)}";
            timeSinceLastUpdate = 0f;
        }
    }
}
