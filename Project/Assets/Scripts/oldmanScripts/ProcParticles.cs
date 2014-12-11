/*	ProcParticles.cs
 * 
 */
using UnityEngine;
using System.Collections;

public class ProcParticles : MonoBehaviour {

	public ParticleSystem mainPS = null;
	public float ColorDuration = 3f;

	private float eTime = 0f; // elapsed time.
	private bool SwitchColor = false;
	
	void Update()
	{
		int particleCount;

		// Elapsed time
		eTime = eTime + Time.deltaTime;
		if(eTime >= ColorDuration)
		{
			SwitchColor = !SwitchColor;
			eTime = 0f;
		}

		ParticleSystem.Particle[] tempParticles = new ParticleSystem.Particle[mainPS.maxParticles];
		particleCount = mainPS.GetParticles (tempParticles);

		// Make sure we have an even number.
		if((particleCount == mainPS.maxParticles) && ((particleCount % 2) != 0))
			--particleCount;

		//Debug.Log ("Particle Count: " + particleCount);

		if(SwitchColor)
		{
			for(int i = 0; i < particleCount; i = i + 2)
				tempParticles[i].color = Color.magenta;

			for(int i = 1; i < particleCount; i = i + 2)
				tempParticles[i].color = Color.cyan;
		}
		else
		{
			for(int i = 0; i < particleCount; i = i + 2)
				tempParticles[i].color = Color.cyan;
			for(int i = 1; i < particleCount; i = i + 2)
				tempParticles[i].color = Color.magenta;
		}

		mainPS.SetParticles (tempParticles, particleCount);
	}
}
