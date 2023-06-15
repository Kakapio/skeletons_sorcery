using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;

public class ThirdPersonCameraController : MonoBehaviour
{
    public CinemachineVirtualCamera aimVirtualCamera;

    private StarterAssetsInputs starterAssetsInputs;

    void Start()
    {
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
    }

    // Update is called once per frame
    void Update()
    {
        if(starterAssetsInputs.aim)
        {
            aimVirtualCamera.gameObject.SetActive(true);
        }
        else
        {
            aimVirtualCamera.gameObject.SetActive(false);
        }
    }
}
