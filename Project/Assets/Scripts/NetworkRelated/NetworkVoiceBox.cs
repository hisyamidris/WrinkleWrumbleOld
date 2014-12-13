using UnityEngine;
using System.Collections;

public class NetworkVoiceBox : MonoBehaviour {

	public AudioClip[] SFX;

//	public AudioClip pickUp;
//	public AudioClip throwItem;
//	public AudioClip runSteps;
//
//	public AudioClip femaleJump;
//	public AudioClip femalePain1;
//	public AudioClip femalePain2;
//	public AudioClip femalePain3;
//
//	public AudioClip maleJump;
//	public AudioClip malePain1;
//	public AudioClip malePain2;
//	public AudioClip malePain3;
	
	[RPC]
	public void playSound(int soundIndex)
	{

		//Debug.Log("Play sound");
		this.audio.PlayOneShot (SFX[soundIndex]);
	}

	[RPC]
	public void playSoundFootsteps(bool play)
	{

		//Debug.Log("Play sound footsteps");
		audio.clip = SFX[2];
		if(play) audio.Play ();
		else audio.Pause();
	}
}