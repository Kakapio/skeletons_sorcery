using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 10;

    Transform playerBody;
    float defaultSensitivity;
    float pitch = 0;

    void Start()
    {
        playerBody = transform.parent.transform;
        defaultSensitivity = mouseSensitivity;
        UpdateMouseSensitivity();
    }

    // Update is called once per frame
    void Update()
    {
        if(!PauseMenuBehavior.isGamePaused)
        {
            float moveX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float moveY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            //yaw
            playerBody.Rotate(Vector3.up * moveX);
            
            //pitch
            pitch -= moveY;

            pitch = Mathf.Clamp(pitch, -90f, 90f);

            transform.localRotation = Quaternion.Euler(pitch, 0, 0);
        }
    }

    public void UpdateMouseSensitivity()
    {
        mouseSensitivity = defaultSensitivity * PlayerPrefs.GetFloat("sensitivitySetting", 1f);
    }
}
