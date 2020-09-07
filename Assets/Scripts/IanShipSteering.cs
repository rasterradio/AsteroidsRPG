using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IanShipSteering : MonoBehaviour
{
	public float m_MaxSpeed = 5.0f; // maximum speed
	public float m_MinSpeed = -1.0f;
	public float m_MaxTurnSpeed = 180f; // maximum turn rate in degrees per second
	public float m_MinTurnSpeed = 10f; // minimum turn speed
	public float m_SpeedMultiplier = 10.0f;
	public float m_BoatAcceleration = 0.1f;

	private Rigidbody2D m_Rigidbody; // rigidbody for the boat.
	[HideInInspector]
	public float m_CurrentSpeed; // current speed of the boat.
	private float m_TargetSpeed; // Target Speed of boat.
	private Rigidbody2D m_CameraTarget;
	private float speedRatio;

	void Awake()
	{
		m_Rigidbody = GetComponent<Rigidbody2D>();
		m_TargetSpeed = 0.0f;
		m_CurrentSpeed = 0.0f;
	}

	void Update()
	{
		GetTargetSpeed();
		MoveBoat();
		TurnBoat();
	}

	void MoveBoat()
	{
		if (m_CurrentSpeed >= m_TargetSpeed)
		{
			m_CurrentSpeed -= m_BoatAcceleration * Time.deltaTime;
		}
		else if (m_CurrentSpeed < m_TargetSpeed)
		{
			m_CurrentSpeed += m_BoatAcceleration * Time.deltaTime;
		}
		Vector2 movement = transform.forward * m_CurrentSpeed * Time.deltaTime;
		m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
	}

	// Speed of the boat going forward.
	void GetTargetSpeed()
	{
		var speedInput = Input.GetAxis("Vertical") * m_SpeedMultiplier * Time.deltaTime;
		m_TargetSpeed = Mathf.Clamp(m_TargetSpeed + speedInput, m_MinSpeed, m_MaxSpeed);
	}

	// Turn the boat, the boat will only move the minimum when stationary, and the maximum at full speed.
	void TurnBoat()
	{
		// turnvalue from the input.
		float turnValue = Input.GetAxis("Horizontal");
		float turnMultiplier = Mathf.Lerp(m_MinTurnSpeed, m_MaxTurnSpeed, m_CurrentSpeed / m_MaxSpeed);
		float turn = turnValue * turnMultiplier * Time.deltaTime;

		// make it into a turn rotation on the y axis
		Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);

		// apply to rotation
		//m_Rigidbody.MoveRotation(m_Rigidbody.rotation) * turnRotation);

	}
}
