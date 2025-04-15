using UnityEngine;
using UnityEngine.UI;

public class Option : MonoBehaviour
{
    public GameObject options;
    public AudioSource bgmSource;
    public AudioSource[] sfxSources;
    public Slider bgm;   // BGM 슬라이더
    public Slider sfx;   // SFX 슬라이더
    public Toggle sfxToggle;

    void Start()
    {
        // 🎵 BGM 슬라이더 값 초기화
        if (bgm != null)
        {
            bgm.value = bgmSource.volume;  // BGM 슬라이더 값 설정
            bgm.onValueChanged.AddListener(SetBGMVolume);
        }

        // 🔊 SFX 슬라이더 값 초기화
        if (sfx != null && sfxSources.Length > 0)
        {
            sfx.value = sfxSources[0].volume; // 첫 번째 효과음 기준
            sfx.onValueChanged.AddListener(SetSFXVolume);
        }

        // 🎚️ 효과음 ON/OFF 토글 초기화
        if (sfxToggle != null && sfxSources.Length > 0)
        {
            sfxToggle.isOn = !sfxSources[0].mute; // 음소거 상태 반영
            sfxToggle.onValueChanged.AddListener(ToggleSFX);
        }

        options.SetActive(false); // 옵션 창 기본 비활성화
    }

    public void OpenOptions()
    {
        options.SetActive(true);
    }

    public void CloseOptions()
    {
        options.SetActive(false);
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
}//ssss