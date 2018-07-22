using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerData {
	public static string playerName;
	public static Color playerColor;

	public static string PlayerName {
		get {
				return playerName;
		}
		set {
				playerName = value;
		}
	}

	public static Color PlayerColor {
		get {
			return playerColor;
		}
		set {
			playerColor = value;
		}
	}
}
