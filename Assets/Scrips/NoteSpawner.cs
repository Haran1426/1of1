﻿using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    [Header("Rhythm setting")]
    public AudioSource musicSource;
    public int bpm = 120;
    private float spawnInterval;
    private float nextSpawnTime;

    [Header("Note Setting")]
    public GameObject objectPrefab;
    public float spawnX = 6f;
    public float moveSpeed = 7f;
    public int maxObjects = 999;

    [Header("Note Color")]
    public Sprite redNote;
    public Sprite greenNote;
    public Sprite blueNote;
    public Sprite yellowNote;
    public Sprite magentaNote;
    public Sprite cyanNote;
    public Sprite whiteNote;

    public enum NoteType { Red, Green, Blue, Yellow, Magenta, Cyan, White }

    public NoteType noteType = NoteType.Red;

    public NoteType[] types;

    private List<GameObject> spawnedObjects = new();
    private float[] yPositions = { 1.5f, 0f, -1.5f };

    void Start()
    {
        if (objectPrefab == null || musicSource == null)
        {

            return;
        }

        spawnInterval = 60f / bpm;
        nextSpawnTime = 0f;
        musicSource.Play();
    }

    void Update()
    {
        if (!musicSource.isPlaying) return;

        while (musicSource.time >= nextSpawnTime)
        {
            SpawnNote();
            nextSpawnTime += spawnInterval;
        }
    }

    void SpawnNote()
    {
        if (Time.timeScale > 0)
        {
            if (spawnedObjects.Count >= maxObjects) return;

            float randomY = yPositions[Random.Range(0, yPositions.Length)];
            Vector3 spawnPosition = new Vector3(spawnX, randomY, 0);
            GameObject note = Instantiate(objectPrefab, spawnPosition, Quaternion.identity);

     
            // 랜덤 스프라이트 지정
            Sprite[] sprites = { redNote, greenNote, blueNote, yellowNote, magentaNote, cyanNote, whiteNote };

            int rand = Random.Range(0, sprites.Length);
            SpriteRenderer sr = note.GetComponent<SpriteRenderer>();
            sr.sprite = sprites[rand];
            noteType = types[rand];

            Rigidbody2D rb = note.GetComponent<Rigidbody2D>();
            rb.gravityScale = 0;
            rb.velocity = Vector2.left * moveSpeed;


            note.AddComponent<NoteCollision>();

            spawnedObjects.Add(note);
        }
    }
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("CutLine"))
    //    {
    //        Score.Instance.OnHitCutLine();
    //        Debug.Log("Note가 CutLine에 닿음");

    //    }
    //    else if (collision.CompareTag("HitBox"))
    //    {
    //        Score.Instance.OnHitHitBox();
    //        Debug.Log("Note가 HitBox에 닿음");
    //        Destroy(gameObject);
    //    }
    //}
    public void RemoveFromList(GameObject note)
    {
        if (spawnedObjects.Contains(note))
        {
            spawnedObjects.Remove(note);
        }
    }

}