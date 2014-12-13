using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class healthBarUpdate : MonoBehaviour {
	
	public bool isGameOver = false; //flag to see if game is over
	public float pushThreshold = 1.0f;
	
	private Slider healthBarSlider;  //reference for slider
	private Text gameOverText;   //reference for text
	
	public int pHealth = 100;
	public bool InCharacterCollider = false;
	
	private BasicAniController basicAniController;
	private Health health;
	
	void Awake()
	{
		healthBarSlider = GameObject.Find ("Canvas/Slider").GetComponent<Slider> ();
		gameOverText = GameObject.Find ("Canvas/GameOver").GetComponent<Text>();
		basicAniController = GetComponentInParent<BasicAniController> ();
		health = GetComponentInParent<Health> ();
		healthBarSlider.value = 1;
		gameOverText.enabled = false; //disable GameOver text on start
		isGameOver = false;
		pHealth = 100;
	}
	
	void Update () 
	{
		if(Input.GetKey(KeyCode.T))
		{
			Debug.Log ("Current Health: " + healthBarSlider.value);
		}
		healthBarSlider.value = (float) (health.currentHitPoints / 100);
	}
	
	//Check if player enters/stays on the fire
	void OnTriggerEnter(Collider other)
	{
		//if player triggers fire object and health is greater than 0
		if (!InCharacterCollider && other.gameObject.tag == "Pushable") 
		{
			if (this.healthBarSlider.value > 0 && other.rigidbody.velocity.magnitude >= pushThreshold //&& !InCharacterCollider
			    )
			{
				Debug.Log ("You got slammed by " + other.gameObject.name + " moving at " + Vector3.Magnitude (other.rigidbody.velocity));
				basicAniController.inPain = true;
				GetComponentInParent<PhotonView>().RPC ("TakeDamage", PhotonTargets.All, (int) Mathf.Round(Vector3.Magnitude (other.rigidbody.velocity) * 20f)); 
			}
		} 
		else if (other.gameObject.tag == "Throwable" && other.rigidbody.velocity.magnitude >= pushThreshold) 
		{
			Debug.Log ("You got hit by " + other.gameObject.name + " moving at " + Vector3.Magnitude (other.rigidbody.velocity));
			healthBarSlider.value -= Vector3.Magnitude (other.rigidbody.velocity) / 100;
			basicAniController.inPain = true;
			GetComponentInParent<PhotonView>().RPC ("TakeDamage", PhotonTargets.All, (int) Mathf.Round(Vector3.Magnitude (other.rigidbody.velocity) * 6f));
		}
		if (healthBarSlider.value < 0.01) 
		{
			isGameOver = true;    //set game over to true
			gameOverText.enabled = true; //enable GameOver text
		}
	}
	
}