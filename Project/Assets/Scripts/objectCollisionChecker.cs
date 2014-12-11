using UnityEngine;
using System.Collections;

public class objectCollisionChecker : MonoBehaviour {

	void OnTriggerEnter(Collider other) 
	{
		if (other.gameObject.tag == "Pushable")
			if (other.rigidbody != null)
				Debug.Log (other.rigidbody.velocity.magnitude);
	}
}
