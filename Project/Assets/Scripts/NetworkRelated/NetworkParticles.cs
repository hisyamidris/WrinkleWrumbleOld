using UnityEngine;
using System.Collections;

public class NetworkParticles : MonoBehaviour {

	public ParticleSystem mainPS = null;

	private BasicAniController basicAniController;

	// Use this for initialization
	void Awake () {
		basicAniController = gameObject.GetComponentInParent<BasicAniController> ();
	}
	
	[RPC]
	public void showParticles()
	{
		//Debug.Log("Particle Show:" + basicAniController.hasCube);
		//mainPS.enableEmission = tf;
		mainPS.enableEmission = basicAniController.hasCube;
	}
}
