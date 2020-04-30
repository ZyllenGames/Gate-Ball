using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControll : MonoBehaviour
{
	public Transform LineTrail;

	public float PlayerSpeed;
	public float PlayerSlowSpeed;
	public float PullBackSpeed;
	public float MaxPullBackDistance;
	public float MouseSensitivity;

	public float ForceDistRatio;

	enum PlayerState {	MOVE, AIM, SHOOT }
	PlayerState m_PlayerState;

	Rigidbody m_Rigidbody;

	float yaw;
	float m_CurYaw;
	Vector3 m_CurInputDirection;
	float m_Speed;

	float m_CurPullBack;
	float m_CurPullBackSpeed;

	Vector3 m_OriginPos;


	private void Awake()
	{
		m_Rigidbody = GetComponent<Rigidbody>();

		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;

		m_PlayerState = PlayerState.MOVE;
		m_CurPullBack = 0;

		if (Application.platform == RuntimePlatform.WebGLPlayer)
			MouseSensitivity = 1;
	}

	private void Update()
	{
		yaw += Input.GetAxis("Mouse X") * MouseSensitivity;

		//Get input Direction
		Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
		m_CurInputDirection = input.normalized;

		switch (m_PlayerState)
		{
			case PlayerState.MOVE:
				m_Speed = PlayerSpeed;
				LineTrail.localScale = new Vector3(1, 1, 1);

				if (Input.GetMouseButtonDown(0))
				{
					m_CurPullBackSpeed = PullBackSpeed;
					m_CurPullBack = 0;
					m_PlayerState = PlayerState.AIM;
				}
				break;
			case PlayerState.AIM:
				m_Speed = PlayerSlowSpeed;

				if (Mathf.Abs(m_CurPullBack - MaxPullBackDistance) > 0.5)
					m_CurPullBack += m_CurPullBackSpeed * Time.deltaTime;
				else
					m_CurPullBackSpeed = 0;

				LineTrail.localScale = new Vector3(1, 1, m_CurPullBack * 3);

				if (Input.GetMouseButtonUp(0))
				{
					m_PlayerState = PlayerState.SHOOT;
					m_OriginPos = CalculateOriginPos();
					AudioManager.Instance.Play2DSound("Hit");
				}

				break;
			case PlayerState.SHOOT:
				LineTrail.localScale = new Vector3(1, 1, 1);
				break;
			default:
				break;
		}
	}

	private Vector3 CalculateOriginPos()
	{
		Ray shootRay = new Ray(transform.position, transform.forward);
		return shootRay.GetPoint(m_CurPullBack);
	}

	private void FixedUpdate()
	{
		Vector3 direction;
		switch (m_PlayerState)
		{
			case PlayerState.MOVE:
				m_CurYaw = Mathf.Lerp(m_CurYaw, yaw, 0.6f);
				m_Rigidbody.MoveRotation(Quaternion.Euler(new Vector3(0, m_CurYaw, 0)));

				direction = Quaternion.AngleAxis(yaw, Vector3.up) * m_CurInputDirection;
				m_Rigidbody.MovePosition(m_Rigidbody.position + direction * m_Speed * Time.fixedDeltaTime); 
				break;
			case PlayerState.AIM:
				m_CurYaw = Mathf.Lerp(m_CurYaw, yaw, 0.6f);
				m_Rigidbody.MoveRotation(Quaternion.Euler(new Vector3(0, m_CurYaw, 0)));

				direction = Quaternion.AngleAxis(yaw, Vector3.up) * m_CurInputDirection;
				m_Rigidbody.MovePosition(m_Rigidbody.position + direction * m_Speed * Time.fixedDeltaTime);

				m_Rigidbody.MovePosition(m_Rigidbody.position - transform.forward * m_CurPullBackSpeed * Time.fixedDeltaTime);

				break;
			case PlayerState.SHOOT:
				Vector3 force = m_OriginPos - m_Rigidbody.position;
				m_Rigidbody.AddForce(force * ForceDistRatio);

				if (force.magnitude < 0.8)
				{
					float angle = Vector3.Angle(transform.forward, m_Rigidbody.velocity);
					if (angle > 90)
					{
						m_PlayerState = PlayerState.MOVE;
						m_Rigidbody.velocity = Vector3.zero;
					}
				}
				break;
			default:
				break;
		}
		
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.collider.tag == "Ball")
		{
			TimeManager.Instance.StartTimer();
		}
	}
}
