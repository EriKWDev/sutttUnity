using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour {

	public float widthToBeSeen = 10f;

	void Update () {
		Camera.main.orthographicSize = widthToBeSeen * Screen.height / Screen.width * 0.5f;
	}
}
