using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    public NoteSpawner.NoteType noteType;

    private float hitZoneX = -4.3f; // 판정선 X 위치
    private float hitRange = 0.3f;  // 허용 오차 범위
    private bool isHit = false; // 중복 판정 방지
    [Header("Effect Prefabs")]
    public GameObject hitEffectPrefab;
    public GameObject missEffectPrefab;
    void Update()
    {
        if (isHit) return;

        float distanceToHitZone = Mathf.Abs(transform.position.x - hitZoneX);

        if (Input.anyKeyDown && distanceToHitZone <= hitRange)
        {
            if (CheckInputForNote(noteType))
            {
                isHit = true;
                Score.Instance.AddScore(Random.Range(100, 151));
                ShowEffect(hitEffectPrefab);
                Destroy(gameObject);
            }
        }

        if (transform.position.x < hitZoneX - 1f)
        {
            isHit = true;
            Score.Instance.OnHitCutLine();
            ShowEffect(missEffectPrefab);
            Destroy(gameObject);
        }
    }
    void ShowEffect(GameObject effectPrefab)
    {
        if (effectPrefab != null)
        {
            GameObject effect = Instantiate(effectPrefab, transform.position, Quaternion.identity);
            Destroy(effect, 1f); // 1초 후 자동 제거
        }
    }
    bool CheckInputForNote(NoteSpawner.NoteType type)
    {
        switch (type)
        {
            case NoteSpawner.NoteType.Red:
                return Input.GetKeyDown(KeyCode.R);
            case NoteSpawner.NoteType.Green:
                return Input.GetKeyDown(KeyCode.G);
            case NoteSpawner.NoteType.Blue:
                return Input.GetKeyDown(KeyCode.B);
            case NoteSpawner.NoteType.Yellow:
                return (Input.GetKey(KeyCode.R) && Input.GetKeyDown(KeyCode.G)) ||
                       (Input.GetKey(KeyCode.G) && Input.GetKeyDown(KeyCode.R));
            case NoteSpawner.NoteType.Magenta:
                return (Input.GetKey(KeyCode.R) && Input.GetKeyDown(KeyCode.B)) ||
                       (Input.GetKey(KeyCode.B) && Input.GetKeyDown(KeyCode.R));
            case NoteSpawner.NoteType.Cyan:
                return (Input.GetKey(KeyCode.G) && Input.GetKeyDown(KeyCode.B)) ||
                       (Input.GetKey(KeyCode.B) && Input.GetKeyDown(KeyCode.G));
            case NoteSpawner.NoteType.White:
                return (Input.GetKey(KeyCode.R) && Input.GetKey(KeyCode.G) && Input.GetKeyDown(KeyCode.B)) ||
                       (Input.GetKey(KeyCode.R) && Input.GetKeyDown(KeyCode.G) && Input.GetKey(KeyCode.B)) ||
                       (Input.GetKeyDown(KeyCode.R) && Input.GetKey(KeyCode.G) && Input.GetKey(KeyCode.B));
        }

        return false;
    }
}
