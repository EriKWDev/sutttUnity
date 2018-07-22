using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using TMPro;

public class GameManager : Photon.MonoBehaviour {

	public static GameManager gameManager;
	public TMP_Text turnText;
	public GameObject Canvas;
	public GameObject playerNameTextPrefab;
	int players = 0;

	public Color tieColor;
	public Color highlightedColor;
	public Color unhighlightedColor;
	public int currentPlayer = 0;
	public Dictionary<string, int> board = new Dictionary<string, int>();
	public Dictionary<string, int> medBoard = new Dictionary<string, int>();
	public Dictionary<string, int> clickable = new Dictionary<string, int>();
	public Dictionary<string, int> midClicks = new Dictionary<string, int>();

	public void Awake() {
		gameManager = GetComponent<GameManager>();
	}

	private void Start()
	{
		clickable = board;
		for (int i = 0; i <= 8; i++) {
			medBoard.Add("0" + i, 2);
		}
	}

	public void Set(string id, int value, Color color) {
		if (board.ContainsKey(id)) {
			board[id] = value;
		} else {
			board.Add(id, value);
		}

		GameObject.Find(id).GetComponent<SpriteRenderer>().color = color;
	}

	public void AddPlayer (string playerName, Color playerColor) {
		GameObject text = Instantiate <GameObject>(playerNameTextPrefab, Canvas.transform);
		text.GetComponent<TMP_Text>().text = playerName;
		text.GetComponent<TMP_Text>().color = playerColor;
		Vector3 newPos = text.transform.position;
		newPos.y -= 40 * players;
		text.transform.position = newPos;
		players++;
	}
	
	public void Click (int playerId, string id, Color color) {
		if (currentPlayer == playerId && players > 1) {
			if (clickable.ContainsKey(id)) {
				if ((!board.ContainsKey(id) || board[id] == 2) && !GameObject.Find(id.Substring(0, 2)).GetComponent<MidSquarebehaviour>().isLocked)
				{
					board[id] = currentPlayer;
					midClicks[id.Substring(0, 2)]++;
					if (midClicks[id.Substring(0, 2)] >= 9)
					{
						GameObject.Find(id.Substring(0, 2)).GetComponent<MidSquarebehaviour>().Lock(tieColor);
						medBoard[id.Substring(0, 2)] = 3;
					}
					checkWin(id, color);
					UnHighlightClickable();
					clickable = GetClickable(id);
					SwitchPlayer();
					GameObject.Find(id).GetComponent<SquareBehaviour>().ChangeColor(color);
				}
			}
		}
	}

	public Dictionary<string, int> GetClickable (string id) {
		Dictionary<string, int> tmpclickable = new Dictionary<string, int>();
		clickable = new Dictionary<string, int>();
		if (medBoard["0"+id[2]] != 2) {
			FillAll();
			tmpclickable = clickable;
		} else {
			for (int i = 0; i <= 8; i++) {
				if (board["0" + id[2] + "" + i] == 2) {
					tmpclickable.Add("0" + id[2] + "" + i, 2);
					GameObject.Find("0" + id[2] + "" + i).GetComponent<SquareBehaviour>().ChangeColor(highlightedColor);
				}
			}
		}

		return tmpclickable;
	}

	public void UnHighlightClickable () {
		foreach (KeyValuePair<string, int> pair in clickable) {
			GameObject.Find(pair.Key).GetComponent<SquareBehaviour>().ChangeColor(unhighlightedColor);
		}
	}

	public void FillAll () {
		for (int i = 0; i <= 8; i++) {
			for (int j = 0; j <= 8; j++) {
				if (medBoard["0"+i] == 2 && board["0" + i + "" + j] == 2) {
					clickable.Add("0"+i+""+j, 2);
					GameObject.Find("0" + i + "" + j).GetComponent<SquareBehaviour>().ChangeColor(highlightedColor);
				}
			}
		}
	}

	public void checkWin (string id, Color color) {
		string coord = id.Substring(0, 2);
		bool smallWin = false;
		bool medWin = false;

			   if (board[coord + "0"] == currentPlayer && board [coord + "1"] == currentPlayer && board [coord + "2"] == currentPlayer) {
			smallWin = true;
		} else if (board[coord + "3"] == currentPlayer && board[coord + "4"] == currentPlayer && board[coord + "5"] == currentPlayer) {
			smallWin = true;
		} else if (board[coord + "6"] == currentPlayer && board[coord + "7"] == currentPlayer && board[coord + "8"] == currentPlayer) {
			smallWin = true;
		} else if (board[coord + "0"] == currentPlayer && board[coord + "3"] == currentPlayer && board[coord + "6"] == currentPlayer) {
			smallWin = true;
		} else if (board[coord + "1"] == currentPlayer && board[coord + "4"] == currentPlayer && board[coord + "7"] == currentPlayer) {
			smallWin = true;
		} else if (board[coord + "2"] == currentPlayer && board[coord + "5"] == currentPlayer && board[coord + "8"] == currentPlayer) {
			smallWin = true;
		} else if (board[coord + "0"] == currentPlayer && board[coord + "4"] == currentPlayer && board[coord + "8"] == currentPlayer) {
			smallWin = true;
		} else if (board[coord + "2"] == currentPlayer && board[coord + "4"] == currentPlayer && board[coord + "6"] == currentPlayer) {
			smallWin = true;
		}

		if (smallWin) {
			GameObject.Find(coord).GetComponent<MidSquarebehaviour>().Lock(color);
			medBoard[coord] = currentPlayer;
		}

		coord = "0";

		if (medBoard[coord + "0"] == currentPlayer && medBoard[coord + "1"] == currentPlayer && medBoard[coord + "2"] == currentPlayer)
		{
			medWin = true;
		}
		else if (medBoard[coord + "3"] == currentPlayer && medBoard[coord + "4"] == currentPlayer && medBoard[coord + "5"] == currentPlayer)
		{
			medWin = true;
		}
		else if (medBoard[coord + "6"] == currentPlayer && medBoard[coord + "7"] == currentPlayer && medBoard[coord + "8"] == currentPlayer)
		{
			medWin = true;
		}
		else if (medBoard[coord + "0"] == currentPlayer && medBoard[coord + "3"] == currentPlayer && medBoard[coord + "6"] == currentPlayer)
		{
			medWin = true;
		}
		else if (medBoard[coord + "1"] == currentPlayer && medBoard[coord + "4"] == currentPlayer && medBoard[coord + "7"] == currentPlayer)
		{
			medWin = true;
		}
		else if (medBoard[coord + "2"] == currentPlayer && medBoard[coord + "5"] == currentPlayer && medBoard[coord + "8"] == currentPlayer)
		{
			medWin = true;
		}
		else if (medBoard[coord + "0"] == currentPlayer && medBoard[coord + "4"] == currentPlayer && medBoard[coord + "8"] == currentPlayer)
		{
			medWin = true;
		}
		else if (medBoard[coord + "2"] == currentPlayer && medBoard[coord + "4"] == currentPlayer && medBoard[coord + "6"] == currentPlayer)
		{
			medWin = true;
		}

		if (medWin) {
			Camera.main.backgroundColor = color;
			UnHighlightClickable();
			clickable = new Dictionary<string, int>();
		}
	}

	public void SwitchPlayer () {
		currentPlayer = (currentPlayer > 0 ? 0 : 1);
	}

	public string BoardToString () {
		string tmp = "";
		foreach (KeyValuePair<string, int> pair in board) {
			tmp += pair.Key + pair.Value;
		}
		return tmp;
	}
}
