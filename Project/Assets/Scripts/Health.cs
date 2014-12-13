using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {
	
	public float hitPoints = 40f;
	public float currentHitPoints;
	private float respawnTimer = 0;
	
	// Use this for initialization
	void Start () {
		currentHitPoints = hitPoints;
	}

	void Update()
	{
		
		if(respawnTimer > 0) 
		{
			respawnTimer -= Time.deltaTime;
			
			if(respawnTimer <= 0) 
			{
				Die ();
			}
		}
	}
	
	[RPC]
	public void TakeDamage(int amt) {
		Debug.Log(gameObject.name + "is taking " + amt + " damage!");
		currentHitPoints -= amt;
		
		if(currentHitPoints <= 0) {
			respawnTimer = 3f;
		}
	}

	void Die(){
				string myName = gameObject.name.Replace ("(Clone)", "");
		
				if (GetComponent<PhotonView> ().instantiationId == 0) {
						//Wait for 1s

						Destroy (gameObject);
				} else {
						if (GetComponent<PhotonView> ().isMine) {
								if (gameObject.tag == "Player") {		// This is my actual PLAYER object, then initiate the respawn process
										NetworkManager nm = GameObject.FindObjectOfType<NetworkManager> ();
					
										nm.standbyCamera.SetActive (true);
										nm.respawnTimer = 0.5f;
										nm.playerName = myName;
								}
								//Wait for 1s
								PhotonNetwork.Destroy (gameObject);
				
						}
				}
		}
}
