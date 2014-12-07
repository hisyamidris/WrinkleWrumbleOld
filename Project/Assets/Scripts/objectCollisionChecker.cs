using UnityEngine;
using System.Collections;

public class objectCollisionChecker : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Pushable")
				if (other.rigidbody != null)
						Debug.Log (other.rigidbody.velocity.magnitude);
	}
}
