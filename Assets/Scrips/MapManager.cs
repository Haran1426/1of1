    using System.Collections.Generic;
    using System.IO;
    using UnityEngine;

    public class MapManager : MonoBehaviour
    {
        [System.Serializable]
        public class NoteData
        {
            public float time;
            public string type;
        }

        [System.Serializable]
        public class GimmickData
        {
            public float time;
            public string type;
            public float value;
            public float duration;
        }

        [System.Serializable]
        public class MapData
        {
            public string songName;
            public float bpm;
            public List<NoteData> notes;
            public List<GimmickData> gimmicks;
        }

        [Header("Map Settings")]
        public string mapName = "test_track"; // Resources/Maps/test_track.json
        private MapData currentMap;

        [Header("Note Settings")]
        public GameObject notePrefab;
        public float spawnX = 6f;
        public float moveSpeed = 2f;
        public Sprite redNote, greenNote, blueNote, yellowNote, magentaNote, cyanNote, whiteNote;

        public enum NoteType { Red, Green, Blue, Yellow, Magenta, Cyan, White }
        private Dictionary<string, NoteType> noteTypeMap;
        private Dictionary<NoteType, Sprite> noteSpriteMap;

        private List<GameObject> activeNotes = new();
        private float[] yPositions = { 1.5f, 0f, -1.5f };

        [Header("Music")]
        public AudioSource musicSource;

        [Header("Judge Line")]
        public Transform hitLine;
        public float judgementRange = 1.0f;

        private int nextNoteIndex = 0;

        void Start()
        {
            SetupNoteMaps();
            LoadMap(mapName);
            musicSource.Play();
        }

        void Update()
        {
            if (!musicSource.isPlaying || currentMap == null) return;

            // 노트 스폰
            while (nextNoteIndex < currentMap.notes.Count && currentMap.notes[nextNoteIndex].time <= musicSource.time)
            {
                SpawnNote(currentMap.notes[nextNoteIndex]);
                nextNoteIndex++;
            }

            HandleInput();
        }

        void SetupNoteMaps()
        {
            noteTypeMap = new Dictionary<string, NoteType>
            {
                {"R", NoteType.Red},
                {"G", NoteType.Green},
                {"B", NoteType.Blue},
                {"RG", NoteType.Yellow},
                {"RB", NoteType.Magenta},
                {"GB", NoteType.Cyan},
                {"RGB", NoteType.White}
            };

            noteSpriteMap = new Dictionary<NoteType, Sprite>
            {
                { NoteType.Red, redNote },
                { NoteType.Green, greenNote },
                { NoteType.Blue, blueNote },
                { NoteType.Yellow, yellowNote },
                { NoteType.Magenta, magentaNote },
                { NoteType.Cyan, cyanNote },
                { NoteType.White, whiteNote }
            };
        }

        void LoadMap(string name)
        {
            TextAsset json = Resources.Load<TextAsset>($"Maps/{name}");
            if (json == null)
            {
                Debug.LogError("맵 파일을 찾을 수 없습니다: " + name);
                return;
            }

            currentMap = JsonUtility.FromJson<MapData>(json.text);
            Debug.Log($"맵 {currentMap.songName} 불러오기 성공. BPM: {currentMap.bpm}, 노트 수: {currentMap.notes.Count}");
        }

        void SpawnNote(NoteData noteData)
        {
            if (!noteTypeMap.ContainsKey(noteData.type)) return;

            NoteType type = noteTypeMap[noteData.type];
            float randomY = yPositions[Random.Range(0, yPositions.Length)];
            Vector3 spawnPos = new Vector3(spawnX, randomY, 0);

            GameObject note = Instantiate(notePrefab, spawnPos, Quaternion.identity);
            note.tag = "Note";
            note.AddComponent<NoteInfo>().type = type;
            note.GetComponent<SpriteRenderer>().sprite = noteSpriteMap[type];
            note.GetComponent<Rigidbody2D>().velocity = Vector2.left * moveSpeed;

            activeNotes.Add(note);
        }

        void HandleInput()
        {
            if (!Input.anyKeyDown) return;

            activeNotes.RemoveAll(note => note == null);

            for (int i = 0; i < activeNotes.Count; i++)
            {
                GameObject note = activeNotes[i];
                float distance = Mathf.Abs(note.transform.position.x - hitLine.position.x);
                if (distance > judgementRange) continue;

                NoteInfo info = note.GetComponent<NoteInfo>();
                if (info == null) continue;

                bool correct = IsCorrectKey(info.type);
                if (correct)
                {
                    Debug.Log("노트 맞음!");
                    Destroy(note);
                    activeNotes.RemoveAt(i);
                    break;
                }
            }
        }

        bool IsCorrectKey(NoteType type)
        {
            switch (type)
            {
                case NoteType.Red: return Input.GetKeyDown(KeyCode.R);
                case NoteType.Green: return Input.GetKeyDown(KeyCode.G);
                case NoteType.Blue: return Input.GetKeyDown(KeyCode.B);
                case NoteType.Yellow: return Input.GetKey(KeyCode.R) && Input.GetKey(KeyCode.G);
                case NoteType.Magenta: return Input.GetKey(KeyCode.R) && Input.GetKey(KeyCode.B);
                case NoteType.Cyan: return Input.GetKey(KeyCode.G) && Input.GetKey(KeyCode.B);
                case NoteType.White: return Input.GetKey(KeyCode.R) && Input.GetKey(KeyCode.G) && Input.GetKey(KeyCode.B);
            }
            return false;
        }

        // 이 스크립트에 필요한 보조 클래스
        public class NoteInfo : MonoBehaviour
        {
            public NoteType type;
        }
    }
