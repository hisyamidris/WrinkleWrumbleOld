/*
*	BasicAniController.cs
*	Sahle Alturaigi
*	Akmal Idris
*		Dec. 8th, 2014
*
*	Notes: Ctrl + f and type "DEBUG" to find all the things we need to remove
*	before the final build.
*/

using UnityEngine;
using System.Collections;

public class BasicAniController : Photon.MonoBehaviour {
	
	Animator animation_vals;

	public bool isGenderMale;
	// Death parameters
	private bool isDead = false;

	// Push
	private bool isPush = false;

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
	public float jumpTimeStop = 1.3f;
	private float jump_eTime = 0f;
	private bool delayJump = false;

	// SFX parameters
	public AudioClip RunningSFX;
	public AudioClip JumpSFX; 
	public AudioClip[] PainSFX;
	private int PainNumber = 0;

	// Throw Parameters
	public GameObject ThrowableCube;
	public Transform ThrowPosition;
	public float ThrowTime = 1f;
	public float ThrowForce = 500f;
	private bool isThrowing = false;

	public bool hasCube = false;
	public MeshRenderer CubeRender;
	public MeshFilter CubeFilter;

	private float eTime = 0f;


	//GameObject
	//private int objectViewID;
	public string objectName = "";
	public AudioClip ThrowingSFX;

	public bool inPain;
	private healthBarUpdate HealthBarUpdate;
	private ProcParticles procParticles;
	private Health health;

	void Awake(){
		HealthBarUpdate = gameObject.GetComponentInChildren<healthBarUpdate> ();
		procParticles = gameObject.GetComponentInChildren<ProcParticles> ();
		health = gameObject.GetComponentInChildren<Health> ();
	}
	// Collect Animator component
	void Start () 
	{

		animation_vals = GetComponent<Animator>();
		audio.clip = RunningSFX;
		audio.volume = 0.1f;

		isDead = false;

		// Declare array size. Apparently Unity does this for you.
		//Pain = new AudioClip[3];
	}
	
	//
	// Basic Movement handler, push handler, and throw logic
	// DEBUG: Death tester	
	//
	void FixedUpdate()
	{	
		CheckHealth ();
		// Run footstep SFX
		CheckFootSteps();

		// Stop everything if dead.
		if(isDead)
			return;

		if(isThrowing)
		{
			eTime = eTime + Time.fixedDeltaTime;
			if(eTime >= ThrowTime)
			{
				isThrowing = false;
				animation_vals.SetBool ("Throw", isThrowing);
				eTime = 0f;
			}
			else
				return;
		}
		// Get key presses and store in locals
		float leMovement = Input.GetAxis("Vertical");
		float leTurning = Input.GetAxis ("Horizontal");
		float leStrafe = Input.GetAxis ("Strafe");

		// Remove backwards movement
		if(leMovement < 0)
			leMovement = 0;
		
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


			jump_eTime = 0f;
			delayJump = false;
		}
		// Stop jumping after a certain amount of time. (so char does not climb up walls)
		else
		{
			jump_eTime = jump_eTime + Time.fixedDeltaTime;
			if(jump_eTime >= jumpTimeStop)
				delayJump = true;
		}

		// XZ movement
		if(((leMovement > 0) || (leStrafe != 0)) && (!delayJump))
		{
			rigidbody.MovePosition (rigidbody.transform.position + relativeMovement * Time.fixedDeltaTime);
			//CheckFootSteps();
		}

		// Y Rotation
		float leRotation = leTurning * rotationSpeed;
		animation_vals.SetFloat ("Rotation", leTurning);
		if((leTurning > 0.1) || (leTurning < -0.1))
		{
			transform.Rotate(Vector3.up, leRotation * Time.fixedDeltaTime);	
		}
			
		// Throw logic
		if((Input.GetKeyDown (KeyCode.K)) && (grounded) && (hasCube))
		{
			isThrowing = true;
			hasCube = false;
			animation_vals.SetBool ("Throw", isThrowing);
			//Instantiate(ThrowableCube, ThrowPosition.position, ThrowPosition.rotation);
			objectName = objectName.Replace("(Clone)", "");
			GameObject newObject = PhotonNetwork.Instantiate(objectName, ThrowPosition.position, ThrowPosition.rotation, 0) as GameObject;
			newObject.transform.localScale = new Vector3(1f, 1f, 1f);
			newObject.rigidbody.AddRelativeForce (new Vector3(0.0f,0.3f,1.0f) * ThrowForce);
			//audio.PlayOneShot (ThrowingSFX);
			GetComponent<PhotonView>().RPC("playSound", PhotonTargets.All, 1);
		}

		// Push Logic
		if(Input.GetKeyDown (KeyCode.I))
		{
			isPush = true;
			animation_vals.SetBool ("Push", isPush);
			objectInteraction.push = true;
		}
		else if(isPush)
		{
			isPush = false;
			animation_vals.SetBool ("Push", isPush);
			objectInteraction.push = false;
		}

		// SoundTest
		if(Input.GetKeyDown (KeyCode.V))
		{
			GetComponent<PhotonView>().RPC("playSound", PhotonTargets.All, 0);
			Debug.Log (health.hitPoints);
		}

		// DEBUG: death logic
		if(
			//Input.GetKeyDown (KeyCode.O) || 
		   (health.hitPoints < 1)
		   )
		{
			isDead = true;
			animation_vals.SetBool ("Ground", true);
			animation_vals.SetBool ("Dead", isDead);
		}

	}
	
	//
	//	Jump
	//	DEBUG: Press P button for pain SFX. Please remove later
	//
	void Update()
	{
		// Stop everything if dead.
		if(isDead)
			return;

		// Jump logic
		if(!delayJump)
		{
			if(grounded && Input.GetButtonDown("Jump"))
			{
				grounded = false;
				rigidbody.AddForce(new Vector3(0f, jumpForce, 0f));
				delayJump = true;

				if(isGenderMale)
					GetComponent<PhotonView>().RPC("playSound", PhotonTargets.All, 7);
				else GetComponent<PhotonView>().RPC("playSound", PhotonTargets.All, 3);
			}
		}

		// DEBUG
		if(Input.GetKeyDown (KeyCode.P))
			OnHitByObject();

		if (inPain) {
			//audio.PlayOneShot(PainSFX[Random.Range(0, PainSFX.Length)]);
			if(isGenderMale)
				GetComponent<PhotonView>().RPC("playSound", PhotonTargets.All, Random.Range(8, 10));
			else GetComponent<PhotonView>().RPC("playSound", PhotonTargets.All, Random.Range (4, 6));
			inPain = false;

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
	//
	void CheckFootSteps()
	{
		if((grounded)&&(!isThrowing)&&(!isDead))
		{
			if((Input.GetButton ("Vertical2")) || 
			   Input.GetButton ("Strafe"))
			{
				if(!audio.isPlaying)
					GetComponent<PhotonView>().RPC("playSoundFootsteps", PhotonTargets.All, true);
			}
			else
				GetComponent<PhotonView>().RPC("playSoundFootsteps", PhotonTargets.All, false);
		}
		else
			//audio.Pause ();
			GetComponent<PhotonView>().RPC("playSoundFootsteps", PhotonTargets.All, false);
	}

	//
	//	Trigger detection with pickupable objects
	//
	void OnTriggerEnter(Collider theTrigger)
	{
		if(theTrigger.gameObject.tag == "Throwable" && 
		   theTrigger.rigidbody.velocity.magnitude < 1.0f && 
		   !(hasCube) &&
		   Input.GetKey(KeyCode.J)
		   //&& Input.GetKey(KeyCode.J)
		   //&& theTrigger.GetComponent<PhotonView>().instantiationId != 0
		   )
		{
			hasCube = true;
			//gameObject.transform.FindChild ("Particle System").gameObject.SetActive (true);
			//Debug.Log (theTrigger.GetComponent<MeshRenderer>().name);
			//CubeRender = theTrigger.GetComponent<MeshRenderer> ();
			//CubeFilter = theTrigger.GetComponent<MeshFilter> ();
			//Debug.Log("subId: " + theTrigger.GetComponent<PhotonView>().subId);
			//objectViewID = theTrigger.GetComponent<PhotonView>().subId;
			//Debug.Log(theTrigger.gameObject.name);
			objectName = theTrigger.gameObject.name;
			//collider.isTrigger = true;
			//theTrigger.renderer.enabled = false;
			//theTrigger.collider.enabled = false;

//					if(theTrigger.GetComponent<PhotonView>().instantiationId == 0)
			//Destroy (theTrigger.gameObject);
//			if (PhotonNetwork.isMasterClient)
//			PhotonNetwork.Destroy (theTrigger.gameObject);
//
//			Destroy (theTrigger.gameObject);

			theTrigger.GetComponent<PhotonView>().RPC("killObject", PhotonTargets.All);
			GetComponent<PhotonView>().RPC("playSound", PhotonTargets.All, 0);
		}
	}

//	void OnTriggerStay(Collider other) {
//		if (other.gameObject == null)
//						HealthBarUpdate.InCharacterCollider = false;
//		else HealthBarUpdate.InCharacterCollider = true;
//		}

	void CheckHealth(){
		if(HealthBarUpdate.pHealth < 25) procParticles.lowHealth = true;
		else procParticles.lowHealth = false;

//		if (HealthBarUpdate.pHealth < 1)
//						Die ();
	}

//	void Die() {
//		if( GetComponent<PhotonView>().instantiationId==0 ) {
//			Destroy(gameObject);
//		}
//		else {
//			if( GetComponent<PhotonView>().isMine ) {
//				if( gameObject.tag == "Player" ) {		// This is my actual PLAYER object, then initiate the respawn process
//					NetworkManager nm = GameObject.FindObjectOfType<NetworkManager>();
//					
//					nm.standbyCamera.SetActive(true);
//					nm.respawnTimer = 3f;
//				}
//				else if( gameObject.tag == "Bot" ) {
//					Debug.LogError("WARNING: No bot respawn code exists!");
//				}
//				
//				PhotonNetwork.Destroy(gameObject);
//			}
//		}
//	}
//
//	public float hitPoints = 100f;
//	float currentHitPoints;
//
//	[RPC]
//	public void TakeDamage(int amt) {
//		HealthBarUpdate.pHealth -= amt;
//		
//		if(HealthBarUpdate.pHealth <= 0) {
//			Die();
//		}
//	}
//	[RPC]
//	public void killObject(Collider theTrigger){
//		if(theTrigger.GetComponent<PhotonView>().instantiationId == 0)
//			Destroy (theTrigger.gameObject);
//		else if (PhotonNetwork.isMasterClient)
//			PhotonNetwork.Destroy (theTrigger.gameObject);
//
//	}
}
