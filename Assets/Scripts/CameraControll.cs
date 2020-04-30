using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControll : MonoBehaviour
{
    public Camera MainCamera;
    public Transform FollowerTrans;
    public float MouseSensitivity;

    float yaw, pitch;

    private void Awake()
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer)
            MouseSensitivity = 1;
    }

    private void Update()
    {
        pitch -= Input.GetAxis("Mouse Y") * MouseSensitivity;
        pitch = Mathf.Clamp(pitch, 10, 50);
        yaw += Input.GetAxis("Mouse X") * MouseSensitivity;
        transform.position = FollowerTrans.position;
    }

    private void FixedUpdate()
    {
        MainCamera.transform.LookAt(transform);
        transform.rotation = Quaternion.Euler(new Vector3(pitch, yaw, 0));
    }
}
