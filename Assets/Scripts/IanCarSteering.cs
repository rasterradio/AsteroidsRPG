using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IanCarSteering : MonoBehaviour
{
	float speedForce = 15f;
	float torqueForce = -200f;
	float driftFactorSticky = 0.9f;
	float driftFactorSlippy = 1;
	float maxStickyVelocity = 2.5f;
	float minSlippyVelocity = 1.5f;

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
			Debug.Log("ACCEL");
			rb.AddForce(transform.up * speedForce);

			// Consider using rb.AddForceAtPosition to apply force twice, at the position
			// of the rear tires/tyres
		}
		if (Input.GetButton("Brakes"))
		{
			rb.AddForce(transform.up * -speedForce / 2f);

			// Consider using rb.AddForceAtPosition to apply force twice, at the position
			// of the rear tires/tyres
		}

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
