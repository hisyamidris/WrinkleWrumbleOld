using UnityEngine;
using System.Collections;

public class Translate_rotate_rigidbody : MonoBehaviour {

	public float moveSpeed = 120f;
	public float turnSpeed = 50f;
	public float drag = 3f;
	
	void Update()
	{
		float movement = Input.GetAxis ("Vertical") * moveSpeed;
		float rotation = Input.GetAxis ("Horizontal") * turnSpeed;
		float t_time = Time.deltaTime;

		// Is there drag?
		if(Input.GetButton("Horizontal"))
			rigidbody.drag = 0.5f;
		else if(Input.GetButton ("Vertical"))
			rigidbody.drag = 0f;
		else if(Input.GetButton ("Jump"))
			rigidbody.drag = drag;

		// Update positions
		rigidbody.AddRelativeForce (Vector3.forward * movement * t_time); // For local space
		transform.Rotate (Vector3.up, rotation * t_time);
	}
}
