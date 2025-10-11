using Unity.Cinemachine;
using UnityEngine;

public class ShakeManager : MonoBehaviour
{
    public static ShakeManager instance { get; private set; }

    private CinemachineCamera _cineCam;
    private float shakeTimer;
    private float initialShakeAmplitude;
    private float totalShakeDuration;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        _cineCam = GetComponent<CinemachineCamera>();
    }

    private void Update()
    {
        if (shakeTimer > 0f)
        {
            shakeTimer -= Time.deltaTime;

            CinemachineBasicMultiChannelPerlin perlin = _cineCam.GetComponent<CinemachineBasicMultiChannelPerlin>();

            float timeRatio = shakeTimer / totalShakeDuration;
            perlin.AmplitudeGain = initialShakeAmplitude * timeRatio;

            if (shakeTimer <= 0f)
            {
                perlin.AmplitudeGain = 0f;
                perlin.FrequencyGain = 0f;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            shakeCam(3f, 0.1f, 0.2f);
        }
    }
    public void shakeCam(float intensity, float frequency, float duration)
    {
        CinemachineBasicMultiChannelPerlin perlin = _cineCam.GetComponent<CinemachineBasicMultiChannelPerlin>();
        perlin.AmplitudeGain = intensity;
        perlin.FrequencyGain = frequency;

        initialShakeAmplitude = intensity;
        shakeTimer = duration;
        totalShakeDuration = duration;
    }
}
