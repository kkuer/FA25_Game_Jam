using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    [Header("Volume Sliders")]
    public Slider mainVolumeSlider;
    public Slider musicVolumeSlider;

    [Header("Volume Text")]
    public TextMeshProUGUI mainVolumeText;
    public TextMeshProUGUI musicVolumeText;

    private void Start()
    {
        InitializeSliders();
    }

    private void InitializeSliders()
    {
        //set slider values from SoundManager
        if (SoundManager.instance != null)
        {
            mainVolumeSlider.value = SoundManager.instance.MainVolume;
            musicVolumeSlider.value = SoundManager.instance.MusicVolume;
        }

        //add listeners
        mainVolumeSlider.onValueChanged.AddListener(OnMainVolumeChanged);
        musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);

        //update text displays
        UpdateVolumeDisplays();
    }

    private void OnMainVolumeChanged(float value)
    {
        SoundManager.instance?.SetMainVolume(value);
        UpdateVolumeDisplays();
    }

    private void OnMusicVolumeChanged(float value)
    {
        SoundManager.instance?.SetMusicVolume(value);
        UpdateVolumeDisplays();
    }

    private void UpdateVolumeDisplays()
    {
        if (mainVolumeText != null)
            mainVolumeText.text = Mathf.RoundToInt(SoundManager.instance.MainVolume * 100) + "%";

        if (musicVolumeText != null)
            musicVolumeText.text = Mathf.RoundToInt(SoundManager.instance.MusicVolume * 100) + "%";
    }

    //called when settings menu is opened
    public void OnSettingsOpened()
    {
        InitializeSliders();
    }
}
