using UnityEngine;
using System.Collections;

public class NetworkObjects : Photon.MonoBehaviour {
		
		Vector3 realPosition = Vector3.zero;
		Quaternion realRotation = Quaternion.identity;
		Vector3 realScale = Vector3.one;

		Vector3 realPosition2 = Vector3.zero;
		Vector3 rigidbodyVel = Vector3.zero;
		Vector3 rigidbodyAngVel = Vector3.zero;
		Quaternion realRotation2 = Quaternion.identity;

		
		Animator anim;
		bool gotFirstUpdate = false;
		
		
		// Use this for initialization
		void Start () {

		}
		
		// Update is called once per frame
		void Update () {
//			if( photonView.isMine ) {
//				// Do nothing -- the character motor/input/etc... is moving us
//			}
//			else {
//				transform.position = Vector3.Lerp(transform.position, realPosition, 0.1f);
//				transform.rotation = Quaternion.Lerp(transform.rotation, realRotation, 0.1f);
//				transform.localScale = realScale;
//			}
		}

		void FixedUpdate ()
		{
			if(!photonView.isMine)
			{
//			transform.position = Vector3.Lerp(transform.position, realPosition, 0.1f);
//			transform.rotation = Quaternion.Lerp(transform.rotation, realRotation, 0.1f);
			transform.localScale = realScale;

			rigidbody.position = Vector3.Lerp(rigidbody.position, realPosition2, 0.1f);
				rigidbody.velocity = Vector3.Lerp(rigidbody.velocity, rigidbodyVel, 0.1f);
				rigidbody.rotation = Quaternion.Lerp(rigidbody.rotation, realRotation2, 0.1f);
				
				rigidbody.angularVelocity = Vector3.Lerp(rigidbody.angularVelocity, rigidbodyAngVel, 0.1f);

			}
		}
	
	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
			if(stream.isWriting) {
				// This is OUR player. We need to send our actual position to the network.
//				stream.SendNext(transform.position);
//				stream.SendNext(transform.rotation);
				stream.SendNext(transform.localScale);

				stream.SendNext(rigidbody.position);
				stream.SendNext(rigidbody.rotation);
				stream.SendNext(rigidbody.velocity);
				stream.SendNext(rigidbody.angularVelocity);
		}
		else {
			// This is someone else's player. We need to receive their position (as of a few
				// millisecond ago, and update our version of that player.
				
//				realPosition = (Vector3)stream.ReceiveNext();
//				realRotation = (Quaternion)stream.ReceiveNext();
				realScale = (Vector3)stream.ReceiveNext();

				realPosition2 = (Vector3)stream.ReceiveNext();
				realRotation2 = (Quaternion)stream.ReceiveNext();
				rigidbodyVel = (Vector3)stream.ReceiveNext();
				
				rigidbodyAngVel = (Vector3)stream.ReceiveNext();

			if(gotFirstUpdate == false) {
//				transform.position = realPosition;
//				transform.rotation = realRotation;
				transform.localScale = realScale;
				rigidbody.position = realPosition2;
				rigidbody.rotation = realRotation2;
				rigidbody.velocity = rigidbodyVel;
				rigidbody.velocity = rigidbodyAngVel;
				gotFirstUpdate = true;
			}
		}
		
	}

	[RPC]
	public void killObject(){
		PhotonNetwork.Destroy (gameObject);
		Destroy (gameObject);
//				if(GetComponent<PhotonView>().instantiationId == 0)
//					Destroy (gameObject);
//				else if (PhotonNetwork.isMasterClient)
//					PhotonNetwork.Destroy (gameObject);
	}

	[RPC]
	public void moveObject(){
	}
}
