/*
*	BasicAniController.cs
*	Sahle Alturaigi
*		Nov. 25th, 2014
*
*/

using UnityEngine;
using System.Collections;

public class BasicAniController : MonoBehaviour {
	
	Animator animation_vals;
	public float walkSpeed = 3f;
	public float rotationSpeed = 50f;
	
	private Vector3 Vec3WalkSpeed = new Vector3(0f, 0f, 0.1f);
	
	// Collect Animator component
	void Start () 
	{
		animation_vals = GetComponent<Animator>();
	}
	
	//
	// Basic Movement handler
	//		Notes: 	- Need Speed param in controller
	//				-
	//
	void FixedUpdate()
	{	

		float leMovement = Input.GetAxis("Vertical");
		float leTurning = Input.GetAxis ("Horizontal") * rotationSpeed;

		Vector3 relativeMovement = transform.TransformDirection (Vector3.forward*walkSpeed* Mathf.Abs (leMovement));
		//Debug.Log ("Relative movement: " + relativeMovement);
		
		animation_vals.SetFloat("Speed", Mathf.Abs (leMovement));
		//animation_vals.SetFloat("Speed", leMovement);

		// Movement
		if(leMovement > 0)
		{
			rigidbody.MovePosition (rigidbody.transform.position + relativeMovement * Time.fixedDeltaTime);
		}
		else if(leMovement < 0)
		{
			rigidbody.MovePosition (rigidbody.transform.position - relativeMovement * Time.fixedDeltaTime);
		}
		// Turning
		transform.Rotate(Vector3.up, leTurning * Time.fixedDeltaTime);
		
	}

	void Update()
	{
		
	}
	
	//
	//	Collision Dection
	//
	
}
