/*	MoveLightScript.cs
 * 
 */ 

using UnityEngine;
using System.Collections;

public class MovingLightScript : MonoBehaviour {

	public float rotationSpeed = 2f;
	public float Dist = 3f;
	public float delayTime = 0f;
	public bool Offset = false;
	
	private float etime = 0f;
	private float change_dir = 1f;
	private float platform_dir = 0f;
	private float delay_movement = 0f;

	void Start()
	{
		if(Offset)
			change_dir = -change_dir;
	}
	void Update()
	{
		// Reset timer and change directions
		if(etime > 3)
		{
			change_dir = -change_dir;
			etime = 0f;
			delay_movement = delayTime;
		}
		
		if(delay_movement > 0)
			delay_movement = delay_movement - Time.fixedDeltaTime;
		
		else
		{
			// Change in time
			etime = etime + 0.02f;
			
			// Get new value for platform movement
			platform_dir = change_dir * Time.deltaTime * Dist * rotationSpeed;
			
			// Update Positions
			transform.Rotate(Vector3.right, platform_dir);
			//transform.Translate (new Vector3 (0, platform_dir, 0));
		}

	}
}
