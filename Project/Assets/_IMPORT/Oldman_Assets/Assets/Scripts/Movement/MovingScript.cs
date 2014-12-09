using UnityEngine;
using System.Collections;

public class MovingScript : MonoBehaviour {

	//public static bool vertiPlatTriggered = false; // Has the angry soul blocking this been killed?
	
	public float Dist = 3f;
	public float delayTime = 0f;
	public bool offset = false;
	public bool inverted = false;
	
	private float etime = 0f;
	private float change_dir = 1f;
	private float platform_dir = 0f;
	private float delay_movement = 0f;
	
	void Start()
	{
		if(offset)
			change_dir = -change_dir;
		if(inverted)
		{
			platform_dir = Dist;
			change_dir = -change_dir;
		}
	}
	
	void FixedUpdate()
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
			platform_dir = change_dir * Time.deltaTime * Dist;
			
			// Update Positions
			transform.Translate (new Vector3 (0, platform_dir, 0));
		}
	}
}
