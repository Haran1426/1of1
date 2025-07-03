using UnityEngine;

public class Note : MonoBehaviour
{
    public NoteSpawner.NoteType noteType;

    private float hitZoneX = -4.3f;
    private float perfectRange = 0.1f;
    private float hitRange = 0.3f;
    private bool isHit = false;

    [Header("Effect Prefabs")]
    public GameObject perfectEffectPrefab;
    public GameObject hitEffectPrefab;
    public GameObject missEffectPrefab;

    // ★ GameUI 참조
    private GameUI gameUI;

    void Start()
    {
        gameUI = FindObjectOfType<GameUI>();
    }

    void Update()
    {
        if (isHit) return;

        float distanceToHitZone = Mathf.Abs(transform.position.x - hitZoneX);

        if (Input.anyKeyDown && distanceToHitZone <= hitRange)
        {
            if (CheckInputForNote(noteType))
            {
                isHit = true;

                if (distanceToHitZone <= perfectRange)
                {
                    Score.Instance.AddScore(Random.Range(300, 401));
                    ShowEffect(perfectEffectPrefab);
                    if (gameUI != null) gameUI.ShowJudgement("Perfect");
                }
                else
                {
                    Score.Instance.AddScore(Random.Range(100, 151));
                    ShowEffect(hitEffectPrefab);
                    if (gameUI != null) gameUI.ShowJudgement("Hit");
                }

                Destroy(gameObject);
            }
        }

        if (transform.position.x < hitZoneX - 1f)
        {
            isHit = true;
            Score.Instance.OnHitCutLine();
            ShowEffect(missEffectPrefab);
            if (gameUI != null) gameUI.ShowJudgement("Miss");
            Destroy(gameObject);
        }
    }

    private void ShowEffect(GameObject effectPrefab)
    {
        if (effectPrefab != null)
        {
            GameObject effect = Instantiate(effectPrefab, transform.position, Quaternion.identity);
            Destroy(effect, 1f);
        }
    }

    private bool CheckInputForNote(NoteSpawner.NoteType type)
    {
        if (type == NoteSpawner.NoteType.Red)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Debug.Log("Red 판정됨!");
                return true;
            }
        }
        else if (type == NoteSpawner.NoteType.Green)
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                Debug.Log("Green 판정됨!");
                return true;
            }
        }
        else if (type == NoteSpawner.NoteType.Blue)
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                Debug.Log("Blue 판정됨!");
                return true;
            }
        }
        else if (type == NoteSpawner.NoteType.Yellow)
        {
            if ((Input.GetKey(KeyCode.R) && Input.GetKeyDown(KeyCode.G)) ||
                (Input.GetKey(KeyCode.G) && Input.GetKeyDown(KeyCode.R)))
            {
                Debug.Log("Yellow 판정됨!");
                return true;
            }
        }

        return false;
    }
}
