using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControll : MonoBehaviour
{
    public Camera MainCamera;
    public float PlayerSpeed;
    public float PlayerSlowSpeed;
    public float MouseSensitivity;

    float yaw, pitch;
    float m_Speed;


    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 direction = input.normalized;
        if (Input.GetKey(KeyCode.LeftShift))
            m_Speed = PlayerSlowSpeed;
        else
            m_Speed = PlayerSpeed;


        if (!Input.GetMouseButton(0))
        {
            transform.Translate(direction * m_Speed * Time.deltaTime);
        }

        if(!Input.GetMouseButton(0))
        {
            pitch -= Input.GetAxis("Mouse Y") * MouseSensitivity;
        }
        yaw += Input.GetAxis("Mouse X") * MouseSensitivity;
        pitch = Mathf.Clamp(pitch, -80, 60);
        transform.eulerAngles = new Vector3(0, yaw, 0);
        MainCamera.transform.eulerAngles = new Vector3(pitch, MainCamera.transform.eulerAngles.y, 0);
    }
}
