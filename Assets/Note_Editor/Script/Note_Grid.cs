using UnityEngine;

public class Note_Grid : MonoBehaviour
{
    public float Sell_size;
    public int width;
    private int height;

    public SpriteRenderer Renderer;
    void Start()
    {
        Renderer = GetComponent <SpriteRenderer> ();
        width = 1;
        height = 3;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2(-8 + width, 0);
        Renderer.size = new Vector2 (width, height);
    }
}
