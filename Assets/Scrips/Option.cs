using UnityEngine;
using UnityEngine.UI;

public class Option : MonoBehaviour
{
    public GameObject optionsPanel; // 설정 패널
    public AudioSource bgmSource; // 배경 음악
    public AudioSource[] sfxSources; // 효과음들
    public Slider bgmSlider;
    public Slider sfxSlider;
    public Toggle sfxToggle;

    void Start()
    {
        // 초기화: 슬라이더 값 적용
        if (bgmSlider != null)
        {
            bgmSlider.value = bgmSource.volume;
            bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        }

        if (sfxSlider != null)
        {
            sfxSlider.value = sfxSources[0].volume; // 첫 번째 효과음 기준
            sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        }

        if (sfxToggle != null)
        {
            sfxToggle.isOn = sfxSources[0].mute == false;
            sfxToggle.onValueChanged.AddListener(ToggleSFX);
        }

        optionsPanel.SetActive(false); // 시작할 때 옵션 창 닫기
    }

    public void OpenOptions()
    {
        optionsPanel.SetActive(true);
    }

    public void CloseOptions()
    {
        optionsPanel.SetActive(false);
    }

    public void SetBGMVolume(float volume)
    {
        bgmSource.volume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        foreach (var sfx in sfxSources)
        {
            sfx.volume = volume;
        }
    }

    public void ToggleSFX(bool isOn)
    {
        foreach (var sfx in sfxSources)
        {
            sfx.mute = !isOn;
        }
    }
}
