using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stick : MonoBehaviour
{
    public Transform Origin;
    public Transform End;
    public Transform LineTrail;

    public float SpeedMultiplier;

    Rigidbody m_Rigidbody;

    float m_Speed;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            m_Speed = 0;
        }
        else if(Input.GetMouseButtonUp(0))
        {
            CalculateInitialSpeed();
            LineTrail.localScale = new Vector3(1, 1, 1);
        }

        else if(Input.GetMouseButton(0))
        {
            float mouseY = Input.GetAxis("Mouse Y");
            float distToEnd = (End.position - transform.position).magnitude;
            if(distToEnd > 2)
                transform.Translate(Vector3.forward * mouseY);
            else
            {
                if(mouseY < 0)
                    transform.Translate(Vector3.forward * mouseY);
            }
            LineTrail.localScale = new Vector3(1, 1, distToEnd);
        }
    }

    private void CalculateInitialSpeed()
    {
        Vector3 vec = Origin.position - transform.position;
        float distance = vec.magnitude;
        m_Speed = distance * SpeedMultiplier;
    }

    private void FixedUpdate()
    {
        m_Rigidbody.MovePosition(transform.position + transform.forward * m_Speed * Time.fixedDeltaTime);
        if((transform.position - End.position).magnitude < 1f)
        {
            //StartCoroutine(ReturnToOrigin());
            transform.position = Origin.position;
            m_Speed = 0;
        }
    }

    IEnumerator ReturnToOrigin()
    {
        while(Vector3.Distance(transform.position, Origin.position) > 0.1)
        {
            transform.position = Vector3.MoveTowards(transform.position, Origin.position, 0.2f);
            yield return null;
        }
    }
}
