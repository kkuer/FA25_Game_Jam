using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    private AudioSource source;
    
    public AudioClip rookJump;

    public static SoundManager instance {  get; private set; }

    [Header("Audio Mixers")]
    public AudioMixer mainAudioMixer;

    [Header("Audio Parameters")]
    private const string MASTER_VOLUME_PARAM = "MasterVolume";
    private const string MUSIC_VOLUME_PARAM = "MusicVolume";

    private float _mainVolume = 0.8f;
    private float _musicVolume = 0.8f;

    public float MainVolume => _mainVolume;
    public float MusicVolume => _musicVolume;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        ApplyAudioSettings();
        source = GetComponent<AudioSource>();
    }

    public void SetMainVolume(float volume)
    {
        _mainVolume = Mathf.Clamp01(volume);
        ApplyMainVolume();
    }

    public void SetMusicVolume(float volume)
    {
        _musicVolume = Mathf.Clamp01(volume);
        ApplyMusicVolume();
    }

    private void ApplyAudioSettings()
    {
        ApplyMainVolume();
        ApplyMusicVolume();
    }

    private void ApplyMainVolume()
    {
        if (mainAudioMixer != null)
        {
            //convert linear 0-1 to logarithmic -80 to 0 dB
            float dB = _mainVolume > 0.001f ? 20f * Mathf.Log10(_mainVolume) : -80f;
            mainAudioMixer.SetFloat(MASTER_VOLUME_PARAM, dB);
        }
    }

    private void ApplyMusicVolume()
    {
        if (mainAudioMixer != null)
        {
            //convert linear 0-1 to logarithmic -80 to 0 dB
            float dB = _musicVolume > 0.001f ? 20f * Mathf.Log10(_musicVolume) : -80f;
            mainAudioMixer.SetFloat(MUSIC_VOLUME_PARAM, dB);
        }
    }

    public void playSFX(AudioClip sound)
    {
        source.PlayOneShot(sound);
    }
}
