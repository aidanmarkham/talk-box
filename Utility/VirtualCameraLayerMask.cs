#if CINEMACHINE_PRESENT
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class VirtualCameraLayerMask : MonoBehaviour
{
    public LayerMask CullingMask;

    private CinemachineVirtualCameraBase virtualCamera;
    private CinemachineBrain brain;
    private Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCameraBase>();

        brain = FindObjectOfType<CinemachineBrain>();
        cam = brain.GetComponent<Camera>();
        brain.m_CameraActivatedEvent.AddListener(CameraActivatedEvent);
    }

    public void CameraActivatedEvent(ICinemachineCamera to, ICinemachineCamera from)
    {
        CinemachineVirtualCameraBase toCamera = to as CinemachineVirtualCameraBase;
        if(toCamera && virtualCamera == (CinemachineVirtualCameraBase)to)
        {
            cam.cullingMask = CullingMask;
        }
    }

    
}
#endif