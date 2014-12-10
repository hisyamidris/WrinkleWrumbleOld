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
		SpawnAllObjects ();
		SpawnMyPlayer();

	}

	void SpawnMyPlayer() {
		if(spawnSpots == null) {
			Debug.LogError ("WTF?!?!?");
			return;
		}

		SpawnSpot mySpawnSpot = spawnSpots[ Random.Range (0, spawnSpots.Length) ];
		GameObject myPlayerGO = (GameObject)PhotonNetwork.Instantiate("Player1", mySpawnSpot.transform.position, mySpawnSpot.transform.rotation, 0);
		//standbyCamera.SetActive(false);

		//((MonoBehaviour)myPlayerGO.GetComponent("FPSInputController")).enabled = true;
		((MonoBehaviour)myPlayerGO.GetComponent("BasicAniController")).enabled = true;
	}

	void SpawnAllObjects(){
		SpawnSpot mySpawnSpot = spawnSpots[ Random.Range (0, spawnSpots.Length) ];
		GameObject myObject = (GameObject)PhotonNetwork.Instantiate("Book", mySpawnSpot.transform.position, mySpawnSpot.transform.rotation, 0);
		myObject.transform.localScale = new Vector3(1f, 1f, 1f);
		myObject.name = myObject.name.Replace("(Clone)", "");
	}
}
