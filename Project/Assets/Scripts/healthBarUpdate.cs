﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class healthBarUpdate : MonoBehaviour {

	public Slider healthBarSlider;  //reference for slider
	public Text gameOverText;   //reference for text
	static public bool isGameOver = false; //flag to see if game is over
	
	void Start(){
		healthBarSlider.value = 1;
		gameOverText.enabled = false; //disable GameOver text on start
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey(KeyCode.T)){
			Debug.Log ("Current Health: " + healthBarSlider.value);
			}
		//check if game is over i.e., health is greater than 0
//		if(!isGameOver)
//			transform.Translate(Input.GetAxis("Horizontal")*Time.deltaTime*10f, 0, 0); //get input
	}
	
	//Check if player enters/stays on the fire
	void OnTriggerEnter(Collider other){
		//if player triggers fire object and health is greater than 0
		if (other.gameObject.tag == "Pushable") {
			if (healthBarSlider.value > 0 && other.rigidbody.velocity.magnitude >= 1.0f){
				Debug.Log ("You got slammed by " + other.gameObject.name + " moving at " + Vector3.Magnitude (other.rigidbody.velocity));
				healthBarSlider.value -= Vector3.Magnitude (other.rigidbody.velocity) / 10; 
				BasicAniController.inPain = true;
				}
			} else if (other.gameObject.tag == "Throwable" && other.rigidbody.velocity.magnitude >= 1.0f) {
				Debug.Log ("You got hit by " + other.gameObject.name + " moving at " + Vector3.Magnitude (other.rigidbody.velocity));
				healthBarSlider.value -= Vector3.Magnitude (other.rigidbody.velocity) / 100;
				BasicAniController.inPain = true;
				}
		if (healthBarSlider.value < 0.01) {
			isGameOver = true;    //set game over to true
			gameOverText.enabled = true; //enable GameOver text
			}
	}
}