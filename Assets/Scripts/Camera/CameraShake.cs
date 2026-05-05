using UnityEngine;
using Unity.Cinemachine;
public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance {get; private set;}

    private CinemachineCamera cinemachineCamera;
    private float shakeTimer;

    private void Awake()
    {
        Instance = this;
        cinemachineCamera = GetComponent<CinemachineCamera>();
    }

    public void ShakeCamera(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = 
            cinemachineCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();
    
    
        cinemachineBasicMultiChannelPerlin.FrequencyGain = intensity;

        shakeTimer = time;
    }

    private void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if(shakeTimer <= 0)
            {
                CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
                cinemachineCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();


                cinemachineBasicMultiChannelPerlin.FrequencyGain = 0;
            }


        }
    }
}
