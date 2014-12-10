using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {

	public GameObject standbyCamera;
	SpawnSpot[] spawnSpots;

	public bool offlineMode = false;

	// Use this for initialization
	void Start () {
		spawnSpots = GameObject.FindObjectsOfType<SpawnSpot>();
		Connect ();
	}

	void Connect() {
		if(offlineMode) {
			PhotonNetwork.offlineMode = true;
			OnJoinedLobby();
		}
		else {
			PhotonNetwork.ConnectUsingSettings( "MultiFPS v001" );
		}
	}
	 
	void OnGUI() {
		GUILayout.Label( PhotonNetwork.connectionStateDetailed.ToString() );
	}

	void OnJoinedLobby() {
		Debug.Log ("OnJoinedLobby");
		PhotonNetwork.JoinRandomRoom();
	}

	void OnPhotonRandomJoinFailed() {
		Debug.Log ("OnPhotonRandomJoinFailed");
		PhotonNetwork.CreateRoom( null );
	}

	void OnJoinedRoom() {
		Debug.Log ("OnJoinedRoom");
		if (PhotonNetwork.isMasterClient)
		SpawnAllObjects ();
		SpawnMyPlayer();

	}

	void SpawnMyPlayer() {
		if(spawnSpots == null) {
			Debug.LogError ("WTF?!?!?");
			return;
		}

		SpawnSpot mySpawnSpot = spawnSpots[ Random.Range (0, spawnSpots.Length) ];
		string[] playerNum = {"Player1","Player2","Player3","Player4"};
		GameObject myPlayerGO = (GameObject)PhotonNetwork.Instantiate(playerNum[PhotonNetwork.room.playerCount-1], mySpawnSpot.transform.position, Quaternion.identity, 0);
		standbyCamera.SetActive(false);

		//((MonoBehaviour)myPlayerGO.GetComponent("FPSInputController")).enabled = true;
		((MonoBehaviour)myPlayerGO.GetComponent("BasicAniController")).enabled = true;
	}

	void SpawnAllObjects(){
		Debug.Log (spawnSpots.Length);
		string[] objectList = {"Potted05","Book","StatueB","Vase02","TV","WasteBaskets","Potted05","Vase01A"};
//		Vector3[] objectLocations = {new Vector3(20.0f,-8.0f,-81.0f), new Vector3(-70.5f, 0.5f, 31.5f), new Vector3(9.4f, -5.6f, 19.8f), new Vector3(5.7f,-5.38f,11.69f), new Vector3(-70f,-0.2f,-0.825f), new Vector3(20.5f,9.0f,112.05f), new Vector3(-70.0f,-8.7f,40.02f), new Vector3(-72.0f,-0.3f,15.25f)};
		for (int i = 0; i < objectList.Length; i++) {
			Debug.Log (spawnSpots[i].transform.position);
			GameObject myObject = (GameObject)PhotonNetwork.Instantiate (objectList[i], spawnSpots[7-i].transform.position, spawnSpots[7-i].transform.rotation, 0);
				myObject.transform.localScale = new Vector3 (1f, 1f, 1f);
		}
	}
}
