using UnityEngine;
using System.Collections;

public class objectInteraction : MonoBehaviour {

	public float pushPower = 10.0F;
	public static bool push = false;

	// Use this for initialization
	void Start() {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerStay(Collider other) {
		if (push && (other.gameObject.tag == "Pushable" || other.gameObject.tag == "Player")) {
				if (other.rigidbody != null)
				{
					Debug.Log ("Pushing Object: " + other.gameObject.name);
					if(!waitActive){
						StartCoroutine(Wait());   
					}
					//other.rigidbody.AddForce((new Vector3(other.transform.position.x - transform.parent.position.x), 0.,(other.transform.position.z - transform.parent.position.z)) * pushPower);
				other.rigidbody.AddForce(new Vector3(other.transform.position.x - transform.parent.position.x, 0.0f, other.transform.position.z - transform.parent.position.z).normalized * pushPower);
				}
		}
	}

//	bool canSwitch = false;
	bool waitActive = false; //so wait function wouldn't be called many times per frame
	
	IEnumerator Wait(){
		waitActive = true;
		yield return new WaitForSeconds (0.867f);
//		canSwitch = true;
		waitActive = false;
	}

}