using UnityEngine;
using Cinemachine;
public class CameraManager : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _idleCam;
    [SerializeField] private CinemachineVirtualCamera _followCam;

    void Awake()
    {
        SwitchToIdleCamera();
    }

    public void SwitchToIdleCamera()
    {
        _idleCam.enabled = true;
        _followCam.enabled = false;
    }

    public void SwitchFollowCamera(Transform followTransform)
    {
        _followCam.Follow = followTransform;
        _followCam.enabled = true;
        _idleCam.enabled = false;
    }
}
