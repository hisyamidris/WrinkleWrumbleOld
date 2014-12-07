using UnityEngine;
using System.Collections;

public class objectInteraction : MonoBehaviour {

	public float pushPower = 2.0F;

	// Use this for initialization
	void Start() {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerStay(Collider other) {
		if(Input.GetKey(KeyCode.I) && other.gameObject.tag == "Pushable")
		if(other.rigidbody != null)
			other.rigidbody.AddForce ((other.transform.position - transform.parent.position).normalized * pushPower);
	}



}