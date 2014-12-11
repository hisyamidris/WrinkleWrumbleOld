// ItemPickup.cs
//		Dependent on BasicAniController.cs. It will not work alone.
//

using UnityEngine;
using System.Collections;

public class ItemPickup : MonoBehaviour {
	
	public AudioClip PickSFX;
	private MeshRenderer CubeRender;
	private MeshFilter CubeFilter;
	private BasicAniController basicAniController;

	private GameObject myGO;

	private bool myTrigger = false;

	void Start()
	{
		CubeRender = gameObject.GetComponent<MeshRenderer> ();
		CubeRender.enabled = false;
		basicAniController = GetComponentInParent<BasicAniController> ();
	}
	
	//
	//	Item pickup logic. Works with BasicAniController.cs
	//
	// DEBUG: using K for throwing object.
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.K) && basicAniController.hasCube == false)
		{
			CubeRender.enabled = false;
			myTrigger = false;
		}

		if(basicAniController.hasCube == true && myTrigger == false)
		{
			Debug.Log("Picked up an item");
			audio.PlayOneShot (PickSFX);
			//CubeRender.enabled = true;
			myTrigger = true;
		}
	}
}
