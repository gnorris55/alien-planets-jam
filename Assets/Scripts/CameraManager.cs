using UnityEngine;
using Unity.Cinemachine;

public class CameraManager : MonoBehaviour
{

    

    public static CameraManager Instance { get; private set; }

    [SerializeField] private float maxZoom = 2.0f;
    [SerializeField] private float minZoom = 0.5f;

    private CinemachineCamera cinemachineCamera;
    private float shakeTimer;
    private float shakeTimerTotal;
    private float startingIntensity;
    void Awake()
    {
        Instance = this;
        cinemachineCamera =  GetComponent<CinemachineCamera>();
    }

    private void Start()
    {
        //ShakeCamera(5f, 5f);
    }

    private void Update()
    {
        if (shakeTimer > 0)
        {

            shakeTimer -= Time.deltaTime;

            CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = cinemachineCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();
            cinemachineBasicMultiChannelPerlin.AmplitudeGain = Mathf.Lerp(startingIntensity, 0f, 1 - (shakeTimer / shakeTimerTotal));

        }

        float scrollValue = GameInput.Instance.GetScroll().y;
        if (scrollValue > 0f && cinemachineCamera.Lens.OrthographicSize > minZoom)
        {
            cinemachineCamera.Lens.OrthographicSize -= Time.deltaTime*5;
        }
        else if (scrollValue < 0f && cinemachineCamera.Lens.OrthographicSize < maxZoom)
        {
            cinemachineCamera.Lens.OrthographicSize += Time.deltaTime*5;
        }
    }

    public void ShakeCamera(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = cinemachineCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();
        cinemachineBasicMultiChannelPerlin.AmplitudeGain = intensity;
        
        shakeTimer = time;
        shakeTimerTotal = time;
        startingIntensity = intensity;
    }

    public void SetShakeCamera(float intensity)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = cinemachineCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();
        cinemachineBasicMultiChannelPerlin.AmplitudeGain = intensity;
    }

}
