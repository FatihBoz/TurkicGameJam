using UnityEngine;
using Unity.Cinemachine;

public class ScreenShake : MonoBehaviour
{
    public static ScreenShake Instance;

    private CinemachineCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin noise;

    private float shakeTimer;
    private float shakeTimerTotal;
    private float startingIntensity;

    private void Awake()
    {
        Instance = this;
        virtualCamera = GetComponent<CinemachineCamera>();
        noise = virtualCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void Shake(float intensity, float time)
    {
        noise.AmplitudeGain = intensity;
        startingIntensity = intensity;
        shakeTimer = time;
        shakeTimerTotal = time;
    }

    private void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            noise.AmplitudeGain = Mathf.Lerp(startingIntensity, 0f, 1 - (shakeTimer / shakeTimerTotal));
        }
    }
}
