using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : Photon.MonoBehaviour {
	public string playerName;
	public Color playerColor;
	public int playerId = 0;

	public PhotonView myPhotonView;
	Vector3 pos;
	GameManager gameManager;

	private void Awake() {
		myPhotonView = GetComponent<PhotonView>();
		gameManager = GameManager.gameManager;
		if (myPhotonView.isMine) {
			playerName = PhotonNetwork.playerName;
		} else {
			playerName = myPhotonView.owner.NickName;
		}
		playerId = myPhotonView.ownerId - 1;
	}

	private void Start() {
		name = playerName;
	
		gameManager.AddPlayer(playerName + " : " + playerId, playerColor);
	}


	private void Update() {
		if (myPhotonView.isMine) {
			checkInput();
		}

		gameManager.turnText.text = (gameManager.currentPlayer != playerId ? "Your Turn!" : "Their Turn.");
	}

	void checkInput () {
		bool isClicking = Input.GetMouseButtonDown(0);
		if (isClicking) {
			print(playerName + " is clicking.");

			RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

			if (hit.collider != null)
			{
				if (hit.collider.gameObject.tag == "Square")
				{
					myPhotonView.RPC("Click", PhotonTargets.All, playerId, hit.transform.gameObject.name, playerColor.r, playerColor.g, playerColor.b);
				}
			}
		}
	}

	[PunRPC]
	public void IncrementId () {
		playerId++;
	}

	[PunRPC]
	public void Click(int playerid, string id, float r, float g, float b) {
		gameManager.Click(playerId, id, new Color(r, g, b));
	}

	void OnPhotonSerializeView (PhotonStream stream, PhotonMessageInfo info) {
		if (stream.isWriting) {
			stream.SendNext(transform.position);
		} else {
			pos = (Vector3)stream.ReceiveNext();
		}
	}
}
