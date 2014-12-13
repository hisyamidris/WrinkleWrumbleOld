using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NetworkManager: Photon.MonoBehaviour
{
	public GameObject standbyCamera;
	SpawnSpot[] spawnSpots;
	SpawnSpotPlayer[] spawnSpotsPlayer;
	public Transform playerPrefab;
	public float respawnTimer = 0;
	public int respawnNum = 0;
	private Image controlsImage;
	
	public void Awake()
	{
		controlsImage = GameObject.Find ("Canvas/Controls").GetComponent<Image>();
		spawnSpots = GameObject.FindObjectsOfType<SpawnSpot>();
		spawnSpotsPlayer = GameObject.FindObjectsOfType<SpawnSpotPlayer>();
		// in case we started this demo with the wrong scene being active, simply load the menu scene
		if (!PhotonNetwork.connected)
		{
			Application.LoadLevel("MainMenu");
			return;
		}
		
		// we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
		if (PhotonNetwork.isMasterClient)
			SpawnAllObjects ();
		
		SpawnMyPlayer();
	}

	public void OnGUI()
	{
		if (GUILayout.Button("Return to Lobby"))
		{
			PhotonNetwork.LeaveRoom();  // we will load the menu level when we successfully left the room
		}
		if (GUILayout.Button("Controls"))
		{
			if(controlsImage.enabled)
				controlsImage.enabled = false;
			else controlsImage.enabled = true;
		}
	}
	
	public void OnMasterClientSwitched(PhotonPlayer player)
	{
		Debug.Log("OnMasterClientSwitched: " + player);
		
		string message;
		InRoomChat chatComponent = GetComponent<InRoomChat>();  // if we find a InRoomChat component, we print out a short message
		
		if (chatComponent != null)
		{
			// to check if this client is the new master...
			if (player.isLocal)
			{
				message = "You are Master Client now.";
			}
			else
			{
				message = player.name + " is Master Client now.";
			}
			
			
			chatComponent.AddLine(message); // the Chat method is a RPC. as we don't want to send an RPC and neither create a PhotonMessageInfo, lets call AddLine()
		}
	}
	
	public void OnLeftRoom()
	{
		Debug.Log("OnLeftRoom (local)");
		
		// back to main menu        
		Application.LoadLevel("MainMenu");
	}
	
	public void OnDisconnectedFromPhoton()
	{
		Debug.Log("OnDisconnectedFromPhoton");
		
		// back to main menu        
		Application.LoadLevel("MainMenu");
	}
	
	public void OnPhotonInstantiate(PhotonMessageInfo info)
	{
		Debug.Log("OnPhotonInstantiate " + info.sender);    // you could use this info to store this or react
	}
	
	public void OnPhotonPlayerConnected(PhotonPlayer player)
	{
		Debug.Log("OnPhotonPlayerConnected: " + player);
	}
	
	public void OnPhotonPlayerDisconnected(PhotonPlayer player)
	{
		Debug.Log("OnPlayerDisconneced: " + player);
	}
	
	public void OnFailedToConnectToPhoton()
	{
		Debug.Log("OnFailedToConnectToPhoton");
		
		// back to main menu        
		Application.LoadLevel("MainMenu");
	}

	void SpawnMyPlayer() {
		if(spawnSpots == null) {
			Debug.LogError ("WTF?!?!?");
			return;
		}
//		Debug.Log (spawnSpotsPlayer.Length);
		//SpawnSpot mySpawnSpotPlayer = spawnSpotsPlayer[ Random.Range (0, spawnSpots.Length) ];
		string[] playerNum = {"Player1","Player2","Player3","Player4"};
		if (PhotonNetwork.room.playerCount < 5) {
			GameObject myPlayerGO = (GameObject)PhotonNetwork.Instantiate (playerNum [PhotonNetwork.room.playerCount - 1], spawnSpotsPlayer [PhotonNetwork.room.playerCount - 1].transform.position, spawnSpotsPlayer [PhotonNetwork.room.playerCount - 1].transform.rotation, 0);
			standbyCamera.SetActive(false);
			((MonoBehaviour)myPlayerGO.GetComponent("BasicAniController")).enabled = true;
			((MonoBehaviour)myPlayerGO.GetComponent("Health")).enabled = true;
			((MonoBehaviour)myPlayerGO.GetComponentInChildren<healthBarUpdate>()).enabled = true;
			myPlayerGO.transform.FindChild ("Main Camera").gameObject.SetActive (true);
			myPlayerGO.transform.FindChild ("bodyColliderCheck").gameObject.SetActive (true);
			myPlayerGO.transform.FindChild ("objectSpaceCollider").gameObject.SetActive (true);
		} else {
			GameObject myPlayerGO = (GameObject)PhotonNetwork.Instantiate (playerNum [Random.Range(0, 3)], new Vector3 (11.0f, 0.0f, 11.0f), Quaternion.identity, 0);
			standbyCamera.SetActive(false);
			((MonoBehaviour)myPlayerGO.GetComponent("BasicAniController")).enabled = true;
			((MonoBehaviour)myPlayerGO.GetComponent("Health")).enabled = true;
			myPlayerGO.transform.FindChild ("Main Camera").gameObject.SetActive (true);
			myPlayerGO.transform.FindChild ("bodyColliderCheck").gameObject.SetActive (true);
			myPlayerGO.transform.FindChild ("objectSpaceCollider").gameObject.SetActive (true);
		}
		
		
		//((MonoBehaviour)myPlayerGO.GetComponent("FPSInputController")).enabled = true;
		
	}

	void SpawnAllObjects(){
		//Debug.Log (spawnSpots.Length);
		string[] objectList = {"BallA","Light01A","Book","Chair03","Chair02","Potted05","Light01A","TeaTable01", "TV", "Vase02", "BallC", "BallB", "StatueB", "Chair02", "WasteBaskets", "Chair06", "Potted05", "Vase01A", "Chair03"};
		for (int i = 0; i < objectList.Length; i++) {
			//			Debug.Log (spawnSpots[i].transform.position);
			GameObject myObject = (GameObject)PhotonNetwork.Instantiate (objectList[i], spawnSpots[18-i].transform.position, spawnSpots[18-i].transform.rotation, 0);
			myObject.transform.localScale = new Vector3 (1f, 1f, 1f);
		}
	}

	public string playerName = "Player1";

	void Update() {
		if(respawnTimer > 0) {
			respawnTimer -= Time.deltaTime;
			
			if(respawnTimer <= 0) {
				// Time to respawn the player!
				GameObject myPlayerGO = (GameObject)PhotonNetwork.Instantiate (playerName, spawnSpotsPlayer [PhotonNetwork.room.playerCount - 1].transform.position, spawnSpotsPlayer [PhotonNetwork.room.playerCount - 1].transform.rotation, 0);
				standbyCamera.SetActive(false);
				((MonoBehaviour)myPlayerGO.GetComponent("BasicAniController")).enabled = true;
				((MonoBehaviour)myPlayerGO.GetComponent("Health")).enabled = true;
				((MonoBehaviour)myPlayerGO.GetComponentInChildren<healthBarUpdate>()).enabled = true;
				myPlayerGO.transform.FindChild ("Main Camera").gameObject.SetActive (true);
				myPlayerGO.transform.FindChild ("bodyColliderCheck").gameObject.SetActive (true);
				myPlayerGO.transform.FindChild ("objectSpaceCollider").gameObject.SetActive (true);

			}
		}
	}
}

