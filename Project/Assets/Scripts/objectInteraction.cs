using UnityEngine;
using System.Collections;

public class objectInteraction : MonoBehaviour {

	public float pushPower = 10.0F;
	public static bool push = false;

	string objectName = "";
	Vector3 objectPosition = Vector3.zero;
	Quaternion objectRotation = Quaternion.identity;
	
	void OnTriggerStay(Collider other) {
		if (push && (other.gameObject.tag == "Pushable")) {
				if (other.rigidbody != null)
				{
					Debug.Log ("Pushing Object: " + other.gameObject.name);
					if(!waitActive){
						StartCoroutine(Wait());   
					}

				objectName = other.gameObject.name;
				objectPosition = other.transform.position;
				objectRotation = other.transform.rotation;

				other.GetComponent<PhotonView>().RPC("killObject", PhotonTargets.All);

				objectName = objectName.Replace("(Clone)", "");
				GameObject newObject = PhotonNetwork.Instantiate(objectName, objectPosition, objectRotation, 0) as GameObject;
				newObject.transform.localScale = new Vector3(1f, 1f, 1f);
				newObject.rigidbody.AddForce(new Vector3(other.transform.position.x - transform.parent.position.x, 0.0f, other.transform.position.z - transform.parent.position.z).normalized * pushPower);

				}
		}
	}
	
	bool waitActive = false; //so wait function wouldn't be called many times per frame
	
	IEnumerator Wait(){
				waitActive = true;
				yield return new WaitForSeconds (0.867f);
//		canSwitch = true;
				waitActive = false;
		}

}