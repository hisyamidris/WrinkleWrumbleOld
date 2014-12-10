using UnityEngine;
using System.Collections;

//public class NetworkManager : MonoBehaviour {
//
//	public GameObject standbyCamera;
//	public Canvas myCanvas;
//	SpawnSpot[] spawnSpots;
//
//	public bool offlineMode = false;
//
//	// Use this for initialization
//	void Start () {
//		spawnSpots = GameObject.FindObjectsOfType<SpawnSpot>();
//		Connect ();
//	}
//
//	void Connect() {
//		if(offlineMode) {
//			PhotonNetwork.offlineMode = true;
//			OnJoinedLobby();
//		}
//		else {
//			PhotonNetwork.ConnectUsingSettings( "WrinkleWrumble v160" );
//		}
//	}
//	 
//	void OnGUI() {
//		GUILayout.Label( PhotonNetwork.connectionStateDetailed.ToString() );
//	}
//
//	void OnJoinedLobby() {
//		Debug.Log ("OnJoinedLobby");
//		PhotonNetwork.JoinRandomRoom();
//	}
//
//	void OnPhotonRandomJoinFailed() {
//		Debug.Log ("OnPhotonRandomJoinFailed");
//		PhotonNetwork.CreateRoom( null );
//	}
//
//	void OnJoinedRoom() {
//		Debug.Log ("OnJoinedRoom");
//
//		if (PhotonNetwork.isMasterClient)
//			SpawnAllObjects ();
//
//		SpawnMyPlayer();
//
//	}
//
//	void SpawnMyPlayer() {
//		if(spawnSpots == null) {
//			Debug.LogError ("WTF?!?!?");
//			return;
//		}
//
////		SpawnSpot mySpawnSpot = spawnSpots[ Random.Range (0, spawnSpots.Length) ];
//		string[] playerNum = {"Player1","Player2","Player3","Player4"};
//		if (PhotonNetwork.room.playerCount < 5) {
//						GameObject myPlayerGO = (GameObject)PhotonNetwork.Instantiate (playerNum [PhotonNetwork.room.playerCount - 1], new Vector3 (11.0f, 0.0f, 11.0f), Quaternion.identity, 0);
//			standbyCamera.SetActive(false);
//			((MonoBehaviour)myPlayerGO.GetComponent("BasicAniController")).enabled = true;
//			myPlayerGO.transform.FindChild ("Main Camera").gameObject.SetActive (true);
//			myPlayerGO.transform.FindChild ("bodyColliderCheck").gameObject.SetActive (true);
//			myPlayerGO.transform.FindChild ("objectSpaceCollider").gameObject.SetActive (true);
//				} else {
//			GameObject myPlayerGO = (GameObject)PhotonNetwork.Instantiate (playerNum [Random.Range(0, 3)], new Vector3 (11.0f, 0.0f, 11.0f), Quaternion.identity, 0);
//			standbyCamera.SetActive(false);
//			((MonoBehaviour)myPlayerGO.GetComponent("BasicAniController")).enabled = true;
//			myPlayerGO.transform.FindChild ("Main Camera").gameObject.SetActive (true);
//			myPlayerGO.transform.FindChild ("bodyColliderCheck").gameObject.SetActive (true);
//			myPlayerGO.transform.FindChild ("objectSpaceCollider").gameObject.SetActive (true);
//				}
//
//
//		//((MonoBehaviour)myPlayerGO.GetComponent("FPSInputController")).enabled = true;
//
//	}
//
//	void SpawnAllObjects(){
//		Debug.Log (spawnSpots.Length);
//		string[] objectList = {"BallA","Light01A","Book","Chair03","Chair02","Potted05","Light01A","TeaTable01", "TV", "Vase02", "BallC", "BallB", "StatueB", "Chair02", "WasteBaskets", "Chair06", "Potted05", "Vase01A", "Chair03"};
////		Vector3[] objectLocations = {new Vector3(20.0f,-8.0f,-81.0f), new Vector3(-70.5f, 0.5f, 31.5f), new Vector3(9.4f, -5.6f, 19.8f), new Vector3(5.7f,-5.38f,11.69f), new Vector3(-70f,-0.2f,-0.825f), new Vector3(20.5f,9.0f,112.05f), new Vector3(-70.0f,-8.7f,40.02f), new Vector3(-72.0f,-0.3f,15.25f)};
//		for (int i = 0; i < objectList.Length; i++) {
////			Debug.Log (spawnSpots[i].transform.position);
//			GameObject myObject = (GameObject)PhotonNetwork.Instantiate (objectList[i], spawnSpots[18-i].transform.position, spawnSpots[18-i].transform.rotation, 0);
//				myObject.transform.localScale = new Vector3 (1f, 1f, 1f);
//		}
//	}
//}

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WorkerInGame.cs" company="Exit Games GmbH">
//   Part of: Photon Unity Networking
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using UnityEngine;

public class NetworkManager: Photon.MonoBehaviour
{
		public GameObject standbyCamera;
		SpawnSpot[] spawnSpots;
	public Transform playerPrefab;
	
	public void Awake()
	{
		spawnSpots = GameObject.FindObjectsOfType<SpawnSpot>();
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
		
		//		SpawnSpot mySpawnSpot = spawnSpots[ Random.Range (0, spawnSpots.Length) ];
		string[] playerNum = {"Player1","Player2","Player3","Player4"};
		if (PhotonNetwork.room.playerCount < 5) {
			GameObject myPlayerGO = (GameObject)PhotonNetwork.Instantiate (playerNum [PhotonNetwork.room.playerCount - 1], new Vector3 (11.0f, 0.0f, 11.0f), Quaternion.identity, 0);
			standbyCamera.SetActive(false);
			((MonoBehaviour)myPlayerGO.GetComponent("BasicAniController")).enabled = true;
			myPlayerGO.transform.FindChild ("Main Camera").gameObject.SetActive (true);
			myPlayerGO.transform.FindChild ("bodyColliderCheck").gameObject.SetActive (true);
			myPlayerGO.transform.FindChild ("objectSpaceCollider").gameObject.SetActive (true);
		} else {
			GameObject myPlayerGO = (GameObject)PhotonNetwork.Instantiate (playerNum [Random.Range(0, 3)], new Vector3 (11.0f, 0.0f, 11.0f), Quaternion.identity, 0);
			standbyCamera.SetActive(false);
			((MonoBehaviour)myPlayerGO.GetComponent("BasicAniController")).enabled = true;
			myPlayerGO.transform.FindChild ("Main Camera").gameObject.SetActive (true);
			myPlayerGO.transform.FindChild ("bodyColliderCheck").gameObject.SetActive (true);
			myPlayerGO.transform.FindChild ("objectSpaceCollider").gameObject.SetActive (true);
		}
		
		
		//((MonoBehaviour)myPlayerGO.GetComponent("FPSInputController")).enabled = true;
		
	}
	
	void SpawnAllObjects(){
		Debug.Log (spawnSpots.Length);
		string[] objectList = {"BallA","Light01A","Book","Chair03","Chair02","Potted05","Light01A","TeaTable01", "TV", "Vase02", "BallC", "BallB", "StatueB", "Chair02", "WasteBaskets", "Chair06", "Potted05", "Vase01A", "Chair03"};
		//		Vector3[] objectLocations = {new Vector3(20.0f,-8.0f,-81.0f), new Vector3(-70.5f, 0.5f, 31.5f), new Vector3(9.4f, -5.6f, 19.8f), new Vector3(5.7f,-5.38f,11.69f), new Vector3(-70f,-0.2f,-0.825f), new Vector3(20.5f,9.0f,112.05f), new Vector3(-70.0f,-8.7f,40.02f), new Vector3(-72.0f,-0.3f,15.25f)};
		for (int i = 0; i < objectList.Length; i++) {
			//			Debug.Log (spawnSpots[i].transform.position);
			GameObject myObject = (GameObject)PhotonNetwork.Instantiate (objectList[i], spawnSpots[18-i].transform.position, spawnSpots[18-i].transform.rotation, 0);
			myObject.transform.localScale = new Vector3 (1f, 1f, 1f);
		}
	}
}

