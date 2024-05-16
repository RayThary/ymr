using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    private CinemachineVirtualCamera mainCamera;
    void Start()
    {
        mainCamera = GetComponent<CinemachineVirtualCamera>();
        if (mainCamera.Follow == null)
        {
            mainCamera.Follow = GameManager.instance.GetPlayerTransform;
        }

        if (mainCamera.LookAt == null)
        {
            mainCamera.LookAt = GameManager.instance.GetPlayerTransform;
        }
    }


}
