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
	public float runSpeed = 4.5f;
	public float rotationSpeed = 100f;
	public float jumpForce = 300f;
	public Transform groundCheck; // Check if we are touching the floor
	public LayerMask whatIsGround;
	
	private bool grounded = true;
	private float groundRadius = 0.2f;

	// Jump delay vars
	private float jumpTimeDelay = 1.3f;
	private float eTime = 0f;
	private bool delayJump = false;
	
	// Collect Animator component
	void Start () 
	{
		animation_vals = GetComponent<Animator>();
	}
	
	//
	// Basic Movement handler
	//		Notes: 	- Backwards animation (moving back) is still improper...
	//				- Setup whatIsGround layer mask
	//
	void FixedUpdate()
	{	
		
		// Get key presses and store in locals
		float leMovement = Input.GetAxis("Vertical");
		float leTurning = Input.GetAxis ("Horizontal");
		float leRotation = leTurning * rotationSpeed;
		
		// Check if we are grounded
		grounded = Physics.CheckSphere(groundCheck.position, groundRadius, whatIsGround);
		animation_vals.SetBool ("Ground", grounded);

		// Get relative movement
		Vector3 relativeMovement = transform.TransformDirection (Vector3.forward*runSpeed* Mathf.Abs (leMovement));
		Debug.Log ("Relative movement: " + relativeMovement);
		
		// Do run animation?
		if(grounded)
			animation_vals.SetFloat("Speed", Mathf.Abs (leMovement));

		// Delay Jump?
		if(delayJump)
		{
			if(eTime >= jumpTimeDelay)
			{
				delayJump = false;
				eTime = 0f;
			}
			else
				eTime = eTime + Time.fixedDeltaTime;
		}
		//Debug.Log ("Delay Jump? " + delayJump);

		// XZ movement
		if(leMovement > 0)
		{
			rigidbody.MovePosition (rigidbody.transform.position + relativeMovement * Time.fixedDeltaTime);
		}
		else if(leMovement < 0)
		{
			rigidbody.MovePosition (rigidbody.transform.position - relativeMovement * Time.fixedDeltaTime);
		}

		// Y Rotation
		animation_vals.SetFloat ("Rotation", leTurning);
		if((leTurning > 0.1) || (leTurning < -0.1))
		{
			transform.Rotate(Vector3.up, leRotation * Time.fixedDeltaTime);	
		}
		
	}
	
	//
	//	Jump logic
	//
	void Update()
	{
		//Debug.Log ("Grounded = " + grounded);
		if(!delayJump)
		{
			if(grounded && Input.GetButtonDown("Jump"))
			{
				//animation_vals.SetBool ("Ground", false);
				grounded = false;
				rigidbody.AddForce(new Vector3(0f, jumpForce, 0f));
				delayJump = true;
			}
		}
	}
	
	//
	//	Collision Dection
	//
	
}
