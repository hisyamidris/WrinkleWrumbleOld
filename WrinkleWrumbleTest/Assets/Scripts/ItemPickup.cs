// ItemPickup.cs
//		Dependent on BasicAniController.cs. It will not work alone.
//

using UnityEngine;
using System.Collections;

public class ItemPickup : MonoBehaviour {
	
	public AudioClip PickSFX;
	private MeshRenderer CubeRender;

	void Start()
	{
		CubeRender = gameObject.GetComponent<MeshRenderer> ();
		CubeRender.enabled = false;
	}
	
	//
	//	Item pickup logic. Works with BasicAniController.cs
	//
	// DEBUG: using K for throwing object.
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.K))
		{
			CubeRender.enabled = false;
		}

		if(transform.root.collider.isTrigger)
		{
			audio.PlayOneShot (PickSFX);
			CubeRender.enabled = true;
			transform.root.collider.isTrigger = false; // Reset the trigger.
		}
	}
}
