using UnityEngine;

[RequireComponent(typeof(Camera))]
public class RuntimeGrid : MonoBehaviour
{
    [Header("���� ũ�� (���� ����)")]
    public float cellSize = 1f;
    [Header("���� ���� (������ ��õ)")]
    public Color gridColor = new Color(1f, 1f, 1f, 0.3f);

    Material lineMat;

    void Awake()
    {
        // Internal-Colored ���̴��� ���ο� ��Ƽ���� ����
        Shader shader = Shader.Find("Hidden/Internal-Colored");
        lineMat = new Material(shader) { hideFlags = HideFlags.HideAndDontSave };
        lineMat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        lineMat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        lineMat.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
        lineMat.SetInt("_ZWrite", 0);
    }

    void OnPostRender()
    {
        if (!lineMat) return;
        lineMat.SetPass(0);

        GL.Begin(GL.LINES);
        GL.Color(gridColor);

        Camera cam = GetComponent<Camera>();
        if (!cam.orthographic)
        {
            GL.End();
            return;
        }

        // ī�޶� ����Ʈ ���� ��� ���
        float halfH = cam.orthographicSize;
        float halfW = halfH * cam.aspect;
        Vector3 cpos = cam.transform.position;
        float left = cpos.x - halfW;
        float bottom = cpos.y - halfH;

        int xCount = Mathf.CeilToInt((halfW * 2f) / cellSize);
        int yCount = Mathf.CeilToInt((halfH * 2f) / cellSize);

        // ������
        for (int i = 0; i <= xCount; i++)
        {
            float x = left + i * cellSize;
            GL.Vertex3(x, bottom, 0f);
            GL.Vertex3(x, bottom + yCount * cellSize, 0f);
        }
        // ����
        for (int j = 0; j <= yCount; j++)
        {
            float y = bottom + j * cellSize;
            GL.Vertex3(left, y, 0f);
            GL.Vertex3(left + xCount * cellSize, y, 0f);
        }

        GL.End();
    }
}
