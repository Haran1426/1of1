using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Option : MonoBehaviour
{
    public GameObject options;
    public AudioSource bgmSource;
    public AudioSource[] sfxSources;
    public Slider bgm;
    public Slider sfx;
    public Toggle sfxToggle;

    [Header("프레임 설정")]
    public Slider fpsSlider;
    public TextMeshProUGUI fpsText;
    private int targetFPS = 60;
    public Toggle vSyncToggle;

    [Header("해상도 설정")]
    public TMP_Dropdown resolutionDropdown;

    private int screenWidth = 1920;
    private int screenHeight = 1080;
    private bool fullScreen = true;

    void Start()
    {
        ApplySettings();

        if (bgm != null)
        {
            bgm.value = bgmSource.volume;
            bgm.onValueChanged.AddListener(SetBGMVolume);
        }

        if (sfx != null && sfxSources.Length > 0)
        {
            sfx.value = sfxSources[0].volume;
            sfx.onValueChanged.AddListener(SetSFXVolume);
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

        if (vSyncToggle != null)
        {
            vSyncToggle.isOn = false;
            vSyncToggle.onValueChanged.AddListener(ToggleVSync);
        }

        if (resolutionDropdown != null)
        {
            resolutionDropdown.ClearOptions();
            resolutionDropdown.AddOptions(new System.Collections.Generic.List<string> {
                "1920x1080 (FHD)",
                "2560x1440 (WQHD)"
            });
            resolutionDropdown.value = 0;
            resolutionDropdown.onValueChanged.AddListener(OnResolutionDropdownChanged);
        }

        options.SetActive(false);
    }

    public void ApplySettings()
    {
        QualitySettings.vSyncCount = vSyncToggle != null && vSyncToggle.isOn ? 1 : 0;
        Application.targetFrameRate = QualitySettings.vSyncCount > 0 ? -1 : targetFPS;
        Screen.SetResolution(screenWidth, screenHeight, fullScreen);
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
            case 0:
                SetResolution(1920, 1080, true);
                break;
            case 1:
                SetResolution(2560, 1440, true);
                break;
        }
    }

    public void SetResolution(int width, int height, bool isFullScreen)
    {
        screenWidth = width;
        screenHeight = height;
        fullScreen = isFullScreen;
        Screen.SetResolution(screenWidth, screenHeight, fullScreen);
    }

    public void ToggleVSync(bool enable)
    {
        QualitySettings.vSyncCount = enable ? 1 : 0;
        Application.targetFrameRate = enable ? -1 : targetFPS;
    }

    public void OpenOptions() => options.SetActive(true);
    public void CloseOptions() => options.SetActive(false);

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
}
