using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {
	
	public float hitPoints = 100f;
	public float currentHitPoints;
	private float respawnTimer = 0;
	
	// Use this for initialization
	void Start () {
		currentHitPoints = hitPoints;
	}
	
	[RPC]
	public void TakeDamage(int amt) {
		Debug.Log(gameObject.name + "is taking " + amt + " damage!");
		currentHitPoints -= amt;
		
		if(currentHitPoints <= 0) {
			respawnTimer = 3f;
			Die();
		}
	}
	
	/*void OnGUI() {
		if( GetComponent<PhotonView>().isMine && gameObject.tag == "Player" ) {
			if( GUI.Button(new Rect (Screen.width-100, 0, 100, 40), "Suicide!") ) {
				Die ();
			}
		}
	}*/

	void Die() 
	{

		if(respawnTimer > 0) 
		{
			respawnTimer -= Time.deltaTime;
			
			if(respawnTimer <= 0) 
			{
				string myName = gameObject.name.Replace("(Clone)", "");

				if( GetComponent<PhotonView>().instantiationId==0 ) 
				{
					Destroy(gameObject);
				}
				else 
				{
					if( GetComponent<PhotonView>().isMine ) 
					{
						if( gameObject.tag == "Player" ) 
						{		// This is my actual PLAYER object, then initiate the respawn process
							NetworkManager nm = GameObject.FindObjectOfType<NetworkManager>();
							
							nm.standbyCamera.SetActive(true);
							nm.respawnTimer = 3f;
							nm.playerName = myName;
						}
						
						PhotonNetwork.Destroy(gameObject);

					}
				}
			}
		}
	}
}
