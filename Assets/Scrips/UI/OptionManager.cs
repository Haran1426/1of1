using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class Option : MonoBehaviour
{
    [Header("패널")]
    public GameObject options;         // 옵션 창
    public GameObject loadingPanel;    // 로딩 패널

    [Header("오디오")]
    public AudioSource bgmSource;
    public AudioSource[] sfxSources;
    public Slider bgmSlider;
    public Slider sfxSlider;
    public Toggle sfxToggle;

    [Header("프레임")]
    public Slider fpsSlider;
    public TextMeshProUGUI fpsText;
    private int targetFPS = 240;

    [Header("해상도")]
    public TMP_Dropdown resolutionDropdown;
    private int screenWidth = 1920;
    private int screenHeight = 1080;
    private bool fullScreen = true;

    [Header("로딩 설정")]
    public float fakeLoadingDuration = 2f;
    public string nextSceneName = "Game"; // 이동할 씬 이름

    void Start()
    {
        ApplySettings();

        if (bgmSlider != null)
        {
            bgmSlider.value = bgmSource.volume;
            bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        }

        if (sfxSlider != null && sfxSources.Length > 0)
        {
            sfxSlider.value = sfxSources[0].volume;
            sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        }

        if (sfxToggle != null && sfxSources.Length > 0)
        {
            sfxToggle.isOn = !sfxSources[0].mute;
            sfxToggle.onValueChanged.AddListener(ToggleSFX);
        }

        if (fpsSlider != null && fpsText != null)
        {
            fpsSlider.minValue = 30;
            fpsSlider.maxValue = 240;
            fpsSlider.value = targetFPS;
            fpsSlider.onValueChanged.AddListener(OnFPSSliderChanged);
            UpdateFPSText();
        }

        if (resolutionDropdown != null)
        {
            resolutionDropdown.ClearOptions();
            resolutionDropdown.AddOptions(new List<string> {
                "1920x1080 (FHD)",
                "2560x1440 (WQHD)"
            });
            resolutionDropdown.value = 0;
            resolutionDropdown.onValueChanged.AddListener(OnResolutionDropdownChanged);
        }

        if (options != null) options.SetActive(false);
        if (loadingPanel != null) loadingPanel.SetActive(false);
    }

    // ============================
    // ✅ 게임 시작 (로딩 후 씬 이동)
    // ============================
    public void OnStartButtonClicked()
    {
        StartCoroutine(LoadingAndChangeScene());
    }

    private IEnumerator LoadingAndChangeScene()
    {
        loadingPanel.SetActive(true);
        yield return new WaitForSeconds(fakeLoadingDuration);
        SceneManager.LoadScene(nextSceneName);
    }

    // ============================
    // ✅ 옵션 관련 메서드들
    // ============================
    public void OpenOptions() => options?.SetActive(true);
    public void CloseOptions() => options?.SetActive(false);

    public void SetBGMVolume(float volume) => bgmSource.volume = volume;

    public void SetSFXVolume(float volume)
    {
        foreach (var sfx in sfxSources)
            sfx.volume = volume;
    }

    public void ToggleSFX(bool isOn)
    {
        foreach (var sfx in sfxSources)
            sfx.mute = !isOn;
    }

    public void OnFPSSliderChanged(float value)
    {
        targetFPS = Mathf.RoundToInt(value);
        Application.targetFrameRate = targetFPS;
        UpdateFPSText();
    }

    private void UpdateFPSText()
    {
        if (fpsText != null)
        {
            fpsText.text = $"{targetFPS} FPS";
        }
    }

    public void OnResolutionDropdownChanged(int index)
    {
        switch (index)
        {
            case 0: SetResolution(1920, 1080, true); break;
            case 1: SetResolution(2560, 1440, true); break;
        }
    }

    public void SetResolution(int width, int height, bool isFullScreen)
    {
        screenWidth = width;
        screenHeight = height;
        fullScreen = isFullScreen;
        Screen.SetResolution(screenWidth, screenHeight, fullScreen);
    }

    public void ApplySettings()
    {
        Application.targetFrameRate = targetFPS;
        Screen.SetResolution(screenWidth, screenHeight, fullScreen);
    }
}


// --- Merged from GameSettingsManager.cs ---
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
