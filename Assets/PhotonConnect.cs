using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PhotonConnect : Photon.MonoBehaviour {
	
	public string versionName = "0.1";
	public GameObject panel1, panel2, panel3;
	public GameObject player;

	public TMP_InputField createRoomField, joinRoomField, usernameField;
	public Dropdown colorDropdown;
	public string playerName;
	public Color playerColor;
	GameManager gameManager;
	[SerializeField]
	public List<Color> colors = new List<Color> ();

	public void CreateRoom () {
		if (createRoomField.text.Length >= 1 && usernameField.text.Length >= 1) {
			PhotonNetwork.CreateRoom(createRoomField.text, new RoomOptions() { MaxPlayers = 2 }, null);
		}
	}

	public void JoinRoom () {
		if (joinRoomField.text.Length >= 1 && usernameField.text.Length >= 1) {
			PhotonNetwork.JoinOrCreateRoom(joinRoomField.text, new RoomOptions() { MaxPlayers = 2 }, TypedLobby.Default);
		}
	}

	public void SaveSettings () {
		playerName = usernameField.text;
		//playerColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
		playerColor = colors[colorDropdown.value];
		PhotonNetwork.playerName = playerName;
	}

	private void OnJoinedRoom () {
		print("Connection to Room Succesfully Established!");
		panel2.SetActive(false);
		SaveSettings();

		PhotonNetwork.LoadLevel("Game");
	}

	void Awake () {
		PhotonNetwork.ConnectUsingSettings(versionName);
		DontDestroyOnLoad(this.transform);
		SceneManager.sceneLoaded += OnSceneLoaded;

		print("Connecting to Server...");
	}

	private void OnConnectedToMaster () {
		PhotonNetwork.JoinLobby(TypedLobby.Default);

		print("Connection to Server Succesfully Established.");
	}

	private void OnJoinedLobby () {
		panel1.SetActive (false);
		panel2.SetActive (true);

		print("Player has succesfully joined lobby.");
	}

	private void OnDisconnectedFromPhoton () {

		panel1.SetActive(false);
		panel2.SetActive(false);
		panel3.SetActive(true);
		print("Connection to Server has been lost.");
	}

	private void OnFailedToConnectToPhoton () {
		print("Player has failed to connect to the server.");
	}

	void OnSceneLoaded (Scene scene, LoadSceneMode mode) {
		if (scene.name == "Game") {
			gameManager = GameManager.gameManager;

			GameObject playerInstance = PhotonNetwork.Instantiate(player.name, player.transform.position, player.transform.rotation, 0);
			playerInstance.GetComponent<PlayerBehaviour> ().playerColor = playerColor;
			playerInstance.GetComponent<PlayerBehaviour>().playerName = playerName;
			PhotonNetwork.playerName = playerName;
		}
	}
}
