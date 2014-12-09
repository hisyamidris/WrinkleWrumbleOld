// ItemPickup.cs
//		Dependent on BasicAniController.cs. It will not work alone.
//

using UnityEngine;
using System.Collections;

public class ItemPickup : MonoBehaviour {
	
	public AudioClip PickSFX;
	static public MeshRenderer CubeRender;
	static public MeshFilter CubeFilter;

	void Start()
	{
		CubeRender = gameObject.GetComponent<MeshRenderer> ();
		CubeFilter = gameObject.GetComponent<MeshFilter> ();
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
			Debug.Log ("ASDF");
			audio.PlayOneShot (PickSFX);
			CubeRender.enabled = true;
			transform.root.collider.isTrigger = false; // Reset the trigger.
		}

//		CubeRender = BasicAniController.grabbedObject.gameObject.GetComponent<MeshRenderer> ();
//		CubeFilter = BasicAniController.grabbedObject.gameObject.GetComponent<MeshFilter> ();
	}
}
