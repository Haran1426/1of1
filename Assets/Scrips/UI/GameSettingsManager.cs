using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettingsManager : MonoBehaviour
{
    public static GameSettingsManager Instance;

    public int targetFPS = 60;
    public bool useVSync = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            ApplyFrameSettings();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ApplyFrameSettings()
    {
        QualitySettings.vSyncCount = useVSync ? 1 : 0;
        Application.targetFrameRate = useVSync ? -1 : targetFPS;
    }
}
