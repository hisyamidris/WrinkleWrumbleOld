/*
*	BasicAniController.cs
*	Sahle Alturaigi
*		Dec. 8th, 2014
*
*	Notes: Ctrl + f and type "DEBUG" to find all the things we need to remove
*	before the final build.
*/

using UnityEngine;
using System.Collections;

public class BasicAniController : MonoBehaviour {
	
	Animator animation_vals;

	// Running and rotation parameters
	public float runSpeed = 4.5f;
	public float rotationSpeed = 100f;
	public float strafeSpeed = 4.5f;

	// Jump parameters
	public float jumpForce = 300f;
	public Transform groundCheck; // Check if we are touching the floor
	public LayerMask whatIsGround;
	private bool grounded = true;
	private float groundRadius = 0.2f;

	// Jump delay vars
	public float jumpTimeDelay = 1.3f;
	private float eTime = 0f;
	private bool delayJump = false;

	// SFX parameters
	public AudioClip RunningSFX;
	public AudioClip JumpSFX; 
	public AudioClip[] PainSFX;
	private int PainNumber = 0;

	// Collect Animator component
	void Start () 
	{
		animation_vals = GetComponent<Animator>();
		audio.clip = RunningSFX;

		// Declare array size. Apparently Unity does this for you.
		//Pain = new AudioClip[3];
	}
	
	//
	// Basic Movement handler
	//		Notes: 	- Backwards animation (moving back) is still improper...
	//				-
	//				- 
	void FixedUpdate()
	{	
		// Get key presses and store in locals
		float leMovement = Input.GetAxis("Vertical");
		float leTurning = Input.GetAxis ("Horizontal");
		float leStrafe = Input.GetAxis ("Strafe");

		// Remove backwards movement
		if(leMovement < 0)
		{
			leMovement = 0;
		}
		
		// Check if we are grounded
		grounded = Physics.CheckSphere(groundCheck.position, groundRadius, whatIsGround);
		animation_vals.SetBool ("Ground", grounded);

		// Get relative movement
		Vector3 relativeMovement = transform.TransformDirection (leStrafe * strafeSpeed,
		                                                         transform.position.y,
		                                                         runSpeed * leMovement);
		//Debug.Log ("Relative movement: " + relativeMovement);

		animation_vals.SetFloat ("vSpeed", rigidbody.velocity.y);
		
		// Do run animation?
		if(grounded)
		{
			animation_vals.SetFloat ("Speed", leMovement);
			animation_vals.SetFloat ("Strafe", leStrafe);
		}

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
		if((leMovement > 0) || (leStrafe != 0))
		{
			rigidbody.MovePosition (rigidbody.transform.position + relativeMovement * Time.fixedDeltaTime);
			CheckFootSteps();
		}

		// Y Rotation
		float leRotation = leTurning * rotationSpeed;
		animation_vals.SetFloat ("Rotation", leTurning);
		if((leTurning > 0.1) || (leTurning < -0.1))
		{
			transform.Rotate(Vector3.up, leRotation * Time.fixedDeltaTime);	
		}
		
	}
	
	//
	//	Jump logic
	//	DEBUG: Press P button for pain SFX. Please remove later
	//
	void Update()
	{
		if(!delayJump)
		{
			if(grounded && Input.GetButtonDown("Jump"))
			{
				grounded = false;
				rigidbody.AddForce(new Vector3(0f, jumpForce, 0f));
				delayJump = true;

				audio.PlayOneShot (JumpSFX);
			}
		}

		if(Input.GetKeyDown (KeyCode.P))
		{
			OnHitByObject();
		}
	}

	//
	//	Logic for being hit by throwables and pushables (Not implemented yet)
	//
	void OnHitByObject()
	{
		PainNumber = ((PainNumber + 1) % PainSFX.Length);
		audio.PlayOneShot(PainSFX[PainNumber]);
	}

	//
	//	Logic for making footsteps SFX
	void CheckFootSteps()
	{
		if(grounded)
		{
			if(Input.GetButton ("Vertical") || 
			   Input.GetButton ("Strafe"))
			{
				if(!audio.isPlaying)
					audio.Play ();
			}
			else
			{
				audio.Pause();
			}
		}
		else
		{
			audio.Pause ();
		}
	}
}
