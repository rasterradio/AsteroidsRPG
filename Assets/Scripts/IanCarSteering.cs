using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IanCarSteering : MonoBehaviour
{
	public float speedForce = 15f;
	public float torqueForce = -200f;
	float driftFactorSticky = 0.9f;
	float driftFactorSlippy = 1;
	float maxStickyVelocity = 2.5f;
	bool isBraking = false;

	void FixedUpdate()
	{
		Rigidbody2D rb = GetComponent<Rigidbody2D>();

		float driftFactor = driftFactorSticky;
		if (RightVelocity().magnitude > maxStickyVelocity)
		{
			driftFactor = driftFactorSlippy;
		}

		rb.velocity = ForwardVelocity() + RightVelocity() * driftFactor;

		if (Input.GetButton("Accelerate"))
		{
			rb.AddForce(transform.up * speedForce);
		}
		if (Input.GetButton("Brakes"))
		{
			isBraking = true;
			//rb.drag = 20f;
			rb.AddForce(transform.up * -speedForce / 8f);
		}
        else {isBraking = false; }

		float tf = Mathf.Lerp(0, torqueForce, rb.velocity.magnitude / 2);
		rb.angularVelocity = Input.GetAxis("Horizontal") * tf;
	}

	Vector2 ForwardVelocity()
	{
		return transform.up * Vector2.Dot(GetComponent<Rigidbody2D>().velocity, transform.up);
	}

	Vector2 RightVelocity()
	{
		return transform.right * Vector2.Dot(GetComponent<Rigidbody2D>().velocity, transform.right);
	}
}
