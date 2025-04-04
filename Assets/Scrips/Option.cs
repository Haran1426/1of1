using UnityEngine;
using UnityEngine.UI;

public class Option : MonoBehaviour
{
    public GameObject optionsPanel; // ���� �г�
    public AudioSource bgmSource; // ��� ����
    public AudioSource[] sfxSources; // ȿ������
    public Slider bgmSlider;
    public Slider sfxSlider;
    public Toggle sfxToggle;

    void Start()
    {
        // �ʱ�ȭ: �����̴� �� ����
        if (bgmSlider != null)
        {
            bgmSlider.value = bgmSource.volume;
            bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        }

        if (sfxSlider != null)
        {
            sfxSlider.value = sfxSources[0].volume; // ù ��° ȿ���� ����
            sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        }

        if (sfxToggle != null)
        {
            sfxToggle.isOn = sfxSources[0].mute == false;
            sfxToggle.onValueChanged.AddListener(ToggleSFX);
        }

        optionsPanel.SetActive(false); // ������ �� �ɼ� â �ݱ�
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
