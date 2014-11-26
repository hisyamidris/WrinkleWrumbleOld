using UnityEngine;
using System.Collections;

// Needs more comments
public class RobotControllerScript : MonoBehaviour {

	public float maxSpeed = 10f;
	public float jumpForce = 700f;
	private bool facingRight = true;

	// Reference to get the animator parameters.
	Animator anim_vals;

	private bool grounded = true;
	private float groundRadius = 0.2f;
	public Transform groundCheck;
	public LayerMask whatIsGround;

	void Start () 
	{
		anim_vals = GetComponent<Animator> ();
	}

	// Movement animation is done here.
	void FixedUpdate () 
	{

		// Check if we're grounded (Are we hitting any circles in this collider?)
		grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
		anim_vals.SetBool ("Ground", grounded);

		// Get horizontal key input
		float theMovement = Input.GetAxis ("Horizontal");

		// Change the speed value in the animator parameter "Speed"
		anim_vals.SetFloat ("Speed", Mathf.Abs (theMovement));
		// Change the vSpeed value in the animator parameter "vSpeed"
		anim_vals.SetFloat ("vSpeed", rigidbody2D.velocity.y);

		if(grounded)
		{
			// Movement for character
			rigidbody2D.velocity = new Vector2 (theMovement * maxSpeed, rigidbody2D.velocity.y);
		}

		// Algorithm for handling whether or not the player is moving left or right.
		if((theMovement > 0) && (!facingRight))
			Flip ();
		else if((theMovement < 0) && (facingRight))
			Flip ();

	}

	// Handles jump logic ***Very sloppy code, refactor later***
	void Update()
	{
		// Are we grounded and player pressed space?
		if(grounded && Input.GetButtonDown("Jump"))
		{	
			anim_vals.SetBool ("Ground", false);
			grounded = false;
			rigidbody2D.AddForce (new Vector2(0, jumpForce));
		}

		// Logic for jumping left or right with full velocity. Makes the jumps nicer.
		if((grounded) && ((Input.GetAxis ("Horizontal")*maxSpeed) < maxSpeed/2))
		{
			if(Input.GetAxis("Horizontal") > 0)
				rigidbody2D.velocity = new Vector2 (maxSpeed/2, rigidbody2D.velocity.y);
			else if (Input.GetAxis("Horizontal") < 0)
				rigidbody2D.velocity = new Vector2 (-maxSpeed/2, rigidbody2D.velocity.y);
		}

	}

	// Handles collision with boxes
	void OnCollisionEnter2D(Collision2D theCollision)
	{
		if(theCollision.gameObject.tag == "Sponge")
		{
			Debug.Log ("Ouch! Got hit");
		}
	}

	// Flips robot's position between left and right.
	void Flip()
	{
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x = ((theScale.x)*(-1));
		transform.localScale = theScale;
	}
}
