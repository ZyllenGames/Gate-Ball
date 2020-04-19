using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerHandle : MonoBehaviour
{
    public Camera MainCamera;
    public Transform CameraTarget;
    Rigidbody m_Rigidbody;

    public float PlayerSpeed;
    public float PlayerSlowSpeed;
    public float MouseSensitivity;

    public float ForceDistRatio;

    float yaw, pitch;
    float m_Speed;
    Vector3 m_CurInputDirection;

    bool m_AcurateMode = false;
    bool m_ShootingMode = false;

    float CurMouseY = 0;
    float BottomMouseY = -10;
    float CurMouseYChange = 0;

    Vector3 m_OriginPos;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        //Get input Direction
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        m_CurInputDirection = input.normalized;

        //AcurateMode
        if (Input.GetMouseButton(0))
            m_AcurateMode = true;
        else
            m_AcurateMode = false;

        //Speed
        if(m_AcurateMode)
            m_Speed = PlayerSlowSpeed;
        else
            m_Speed = PlayerSpeed;

        //Mouse Y
        if(Input.GetMouseButtonDown(0))
        {
            CurMouseY = 0;
        }
        else if(Input.GetMouseButtonUp(0))
        {
            m_ShootingMode = true;
            CurMouseYChange = 0;
            m_OriginPos = CalculateOriginPos();
        }
        else if(Input.GetMouseButton(0))
        {
            float temp = CurMouseY;
            CurMouseY += Input.GetAxis("Mouse Y");
            CurMouseY = Mathf.Clamp(CurMouseY, BottomMouseY, 0);
            CurMouseYChange = CurMouseY - temp;
        }

       
    }

    private Vector3 CalculateOriginPos()
    {
        Ray shootRay = new Ray(transform.position, transform.forward);
        return shootRay.GetPoint(-CurMouseY);
    }

    private void LateUpdate()
    {
        if(!m_ShootingMode)
        {
            if(!m_AcurateMode)
                pitch -= Input.GetAxis("Mouse Y") * MouseSensitivity;
            yaw += Input.GetAxis("Mouse X") * MouseSensitivity;
            pitch = Mathf.Clamp(pitch, -80, 60);

            MainCamera.transform.LookAt(CameraTarget);
            CameraTarget.eulerAngles = new Vector3(pitch, yaw, 0);
        }
    }

    private void FixedUpdate()
    {
        if (!m_ShootingMode)
        {
            m_Rigidbody.MoveRotation(Quaternion.Euler(new Vector3(0, yaw, 0)));

            Vector3 direction = Quaternion.AngleAxis(yaw, Vector3.up) * m_CurInputDirection;
            m_Rigidbody.MovePosition(m_Rigidbody.position + direction * m_Speed * Time.fixedDeltaTime);

            m_Rigidbody.MovePosition(m_Rigidbody.position + transform.forward * CurMouseYChange * 4);
        }
        else
        {
            Vector3 force = m_OriginPos - m_Rigidbody.position;
            m_Rigidbody.AddForce(force * ForceDistRatio);

            if (force.magnitude < 1)
            {
                float angle = Vector3.Angle(transform.forward, m_Rigidbody.velocity);
                if(angle > 90)
                {
                    m_ShootingMode = false;
                    m_Rigidbody.velocity = Vector3.zero;
                } 
            } 
        }
    }
}
