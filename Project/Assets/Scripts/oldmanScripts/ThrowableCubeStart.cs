using UnityEngine;
using System.Collections;

public class ThrowableCubeStart : MonoBehaviour {

	public float Power = 1f;
	public AudioClip SFX;

	void Start () 
	{
		rigidbody.AddRelativeForce (Vector3.forward * Power);
		audio.PlayOneShot (SFX);
	}
}
